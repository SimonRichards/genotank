using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace genotank {
    class Population : Dictionary<Genome, Double> {
        internal delegate double FitnessFunction(Genome g);
        int _size;
        FitnessFunction _fitness;
        Random _random = new Random(Configuration.Seed);
        List<Genome> _popList;
        private Configuration _config;

        private Population FromDict(IEnumerable<KeyValuePair<Genome, Double>> dict) {
            Population pop = new Population(_config, _fitness);
            foreach (var pair in dict) {
                pop.Add(pair.Key, pair.Value);
            }
            return pop;
        }

        internal Population(Configuration config, FitnessFunction fitness)
            : base(config.PopSize) {
            _size = config.PopSize;
            _config = config;
            _fitness = fitness;
        }


        internal Population NextGeneration() {
            var next = new ConcurrentBag<KeyValuePair<Genome, Double>>();
            Parallel.For(0, _size, (i, loop) => {
                double method = _random.NextDouble();
                Genome generated;
                if (method < 0.5) {
                    generated = TournamentSelect().Clone();
                } else if (method < 0.75) {
                    generated = TournamentSelect().Crossover(TournamentSelect());
                } else {
                    generated = TournamentSelect().Clone().Mutate();
                }
                next.Add(new KeyValuePair<Genome, Double>(generated, _fitness(generated)));
            });
            return FromDict(next);
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

        public Generation Compile() {
            StringBuilder program = new StringBuilder(5000);
            program.Append(@"namespace genotank {
                    public class DynamicTree : Tree {");

            var genomes = Keys.ToArray();
            for (int i = 0; i < Count; i++) {
                program.Append(@"private double Solve");
                program.Append(i);
                program.Append("(double x) {return ");
                program.Append(genomes[i].Outputs[0].ToString());
                program.Append(";}");
            }

            program.Append(@"


                        public Solver[] _solutions;

                        public DynamicTree() {
                            _solutions = new Solver[] {");
            for (int i = 0; i < 100; i++) {
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
            var types = results.CompiledAssembly.GetTypes();
            var type = types[0];
            return (Generation)type.GetConstructor(Type.EmptyTypes).Invoke(null);
        }
    }
    
}
