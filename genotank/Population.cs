#define MULTI_THREADED
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Diagnostics;
using Tree;



namespace genotank {
    class Population {
        internal delegate void FitnessFunction(Population g);

        readonly FitnessFunction _fitnessFunc;
        readonly Random _random = new Random(Configuration.Seed);
        Configuration _config;
        readonly GeneticTask _task;
        readonly Genome [] _genomes;
        private readonly Genome[] _next;

        internal KeyValuePair<Genome, double> Best {
            get {
                double bestFitness = double.MaxValue;
                int bestIndex = -1;
                for (int i = 0; i < Size; i++) {
                    if (this[i].Fitness < bestFitness) {
                        bestIndex = i;
                        bestFitness = this[i].Fitness;
                    }
                }
                return new KeyValuePair<Genome, double>(this[bestIndex], bestFitness);
            }
        }

        internal Genome this[int i] {
           get { return _genomes[i]; }
           set { _genomes[i] = value; }
        }

        internal Genome[] Genomes {
            set {
                for (int i = 0; i < Size; i++) {
                    this[i] = value[i];
                }
            }
        }

        internal Population(Configuration config, GeneticTask task) {
            Size = config.PopSize;
            _config = config;
            _task = task;
            _fitnessFunc = task.Fitness;
            _genomes = new Genome[Size];
            _next = new Genome[Size];
        }


        //TODO Benchmark this parallised to see if worthwhile
        internal void Evaluate() {
            Debug.Assert(_config.NumMutate + _config.NumCrossover + _config.NumCopy == Size, "Make mutation counts match");

            _fitnessFunc(this);
            int i, index = 0; 
            for (; index < _config.NumCopy; index++) {
                _next[index] = TournamentSelect();
            }

            for (i = 0; i < _config.NumMutate; index++, i++) {
                _next[index] = TournamentSelect().Mutate();
            }

            for (i = 0; i < _config.NumCrossover; index++, i++) {
                _next[index] = TournamentSelect().Crossover(TournamentSelect());
            }
        }

        private Genome TournamentSelect() {
            double min = double.MaxValue;
            int best = -1;
            for (int i = 0; i < _config.TournamentSize; i++) {
                var currentIndex = _random.Next(Size);
                var current = this[currentIndex];

                if (current.Fitness < min) {
                    min = current.Fitness;
                    best = currentIndex;
                }
            }
            Debug.Assert(best != -1, "Failed to find a finite fitness");
            return this[best];
        }

        public Population Next {
            get {
                var next = new Population(_config, _task) { Genomes = _next };
                return next;
            }
        }

#if COMPILED
        public Generation Compile(IEnumerable<Genome> genomes) {
            var program = new StringBuilder(5000);
            program.Append(@"namespace genotank {
                    public class CompiledGeneration : Generation {");

            int i = 0;
            foreach (var genome in genomes) {
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
            var parameters = new CompilerParameters(new string[] {"Tree.dll"}) {GenerateInMemory = true};
            var results = csc.CompileAssemblyFromSource(parameters, program.ToString());
            if (results.Errors.HasErrors) {
                Debugger.Break();
            }
            var types = results.CompiledAssembly.GetTypes();
            var type = types[0];
// ReSharper disable PossibleNullReferenceException
            return (Generation)type.GetConstructor(Type.EmptyTypes).Invoke(null);
// ReSharper restore PossibleNullReferenceException
        }
#endif


        public int Size { get; set; }
    }
    
}
