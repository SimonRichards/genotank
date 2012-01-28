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
            NumCopy = 500;
            NumCrossover = 250;
            NumMutate = 250;
            MinDepth = 2;
            MaxDepth = 6;
            Generations = 1000;
            MaxConstant = 10;
            Threshold = 100;
            Compile = false;
        }
    };
}
