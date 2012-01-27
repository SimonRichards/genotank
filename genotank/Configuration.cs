using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace genotank {
    internal struct Configuration {
        public static int Seed = new Random().Next();

        internal readonly int PopSize,
                   TournamentSize,
                   NumCopy,
                   NumCrossover,
                   NumMutate,
                   MinDepth,
                   MaxDepth,
                   Generations;

        internal readonly double MaxConstant;
        internal readonly bool Compile;

        internal Configuration (bool dummy) {
                PopSize = 100;
                TournamentSize = 10;
                NumCopy = 50;
                NumCrossover = 25;
                NumMutate = 25;
                MinDepth = 2;
                MaxDepth = 6;
                Generations = 1000;
                MaxConstant = 10;
                Compile = false;
        }
    };
}
