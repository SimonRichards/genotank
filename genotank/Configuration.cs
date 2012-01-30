using System;

namespace genotank {
    internal struct Configuration {
        public static int Seed = new Random().Next();

        internal int 
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
            NumCopy = 200;
            NumCrossover = 350;
            NumMutate = 450;
            MinDepth = 1;
            MaxDepth = 3;
            Generations = 100;
            MaxConstant = 10;
            Threshold = 100;
            Compile = false;
        }
    };
}
