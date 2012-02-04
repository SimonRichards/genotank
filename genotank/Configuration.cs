using System;

namespace genotank {
    internal struct Configuration {
        public static int Seed = new Random().Next();

        internal readonly int 
            PopSize,
            TournamentSize,
            NumCopy,
            NumCrossover,
            NumMutate,
            MinDepth,
            MaxDepth,
            Generations;

        internal readonly double 
            MaxConstant,
            Threshold;

        internal readonly bool Compile;

        internal Configuration (bool dummy) {
            PopSize = 1000;
            TournamentSize = 10;
            NumCopy = 100;
            NumCrossover = 300;
            NumMutate = 600;
            MinDepth = 1;
            MaxDepth = 2;
            Generations = 100;
            MaxConstant = 10;
            Threshold = 3;
            Compile = false;
        }
    };
}
