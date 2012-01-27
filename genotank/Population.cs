using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Diagnostics;

namespace genotank {
    class Population : Dictionary<Genome, Double> {
        internal delegate double FitnessFunction(Genome g);
        int _size;
        FitnessFunction _fitness;
        Random _random = new Random(Configuration.Seed);
        List<Genome> _popList;
        Configuration _config;
        GeneticTask _task;


        private Population FromDict(IEnumerable<KeyValuePair<Genome, Double>> dict) {
            Population pop = new Population(_config, _task);
            foreach (var pair in dict) {
                pop.Add(pair.Key, pair.Value);
            }
            return pop;
        }

        private Population FromGenomes(IEnumerable<Genome> genomes) {
            Generation generation = Compile(genomes);
            List<Genome> genomeList = genomes.ToList();
            Population pop = new Population(_config, _task);
            for (int i = 0; i < _config.PopSize; i++) {
                pop.Add(genomeList[i], _task.Fitness(generation.Solutions[i]));
            }
            return pop;
        }

        internal Population(Configuration config, GeneticTask task)
            : base(config.PopSize) {
            _size = config.PopSize;
            _config = config;
            _task = task;
            _fitness = task.Fitness;
        }


        internal Population NextGeneration() {
            if (_config.Compile) {
                var next = new ConcurrentBag<Genome>();
                Parallel.For(0, _size, (i, loop) => {
                    Genome generated;
                    if (i < _config.NumCopy) {
                        generated = TournamentSelect().Clone();
                    } else if (i < _config.NumCrossover + _config.NumCopy) {
                        generated = TournamentSelect().Crossover(TournamentSelect());
                    } else {
                        generated = TournamentSelect().Clone().Mutate();
                    }
                    next.Add(generated);
                });
                return FromGenomes(next);
            } else {
                var next = new ConcurrentBag<KeyValuePair<Genome, Double>>();
                Parallel.For(0, _size, (i, loop) => {
                    Genome generated;
                    if (i < _config.NumCopy) {
                        generated = TournamentSelect().Clone();
                    } else if (i < _config.NumCrossover + _config.NumCopy) {
                        generated = TournamentSelect().Crossover(TournamentSelect());
                    } else {
                        generated = TournamentSelect().Clone().Mutate();
                    }
                    next.Add(new KeyValuePair<Genome, Double>(generated, _fitness(generated)));
                });
                return FromDict(next);
            }
        }

        private Genome TournamentSelect() {
            if (_popList == null) {
                _popList = this.OrderBy((pair) => pair.Value).Select((pair) => pair.Key).ToList();
            }
            int min = _size;
            _config.TournamentSize.Times(() => {
                int attempt = _random.Next(_size);
                if (attempt < min) {
                    min = attempt;
                }
            });
            return _popList[min];
        }

        public Generation Compile(IEnumerable<Genome> genomes) {
            StringBuilder program = new StringBuilder(5000);
            program.Append(@"namespace genotank {
                    public class CompiledGeneration : Generation {");

            int i = 0;
            foreach (Genome genome in genomes) {
                program.Append(@"private double Solve");
                program.Append(i++);
                program.Append("(double x) {return ");
                program.Append(genome.Outputs[0].ToString());
                program.Append(";}");
            }

            program.Append(@"


                        public Solver[] _solutions;

                        public CompiledGeneration() {
                            _solutions = new Solver[] {");
            for (i = 0; i < 100; i++) {
                program.Append("Solve" + i + ",");
            }

            program.Append(@"
                            };
                        }

                        public override Solver[] Solutions {
                            get {
                                return _solutions;
                            }
                        }
                    }
                }
                ");

            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new string[] { "Tree.dll" });
            parameters.GenerateInMemory = true;
            CompilerResults results = csc.CompileAssemblyFromSource(parameters, program.ToString());
            if (results.Errors.HasErrors) {
                Debugger.Break();
            }
            var types = results.CompiledAssembly.GetTypes();
            var type = types[0];
            return (Generation)type.GetConstructor(Type.EmptyTypes).Invoke(null);
        }
    }
    
}
