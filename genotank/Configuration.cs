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

        internal Configuration (bool dummy) {
                PopSize = 100;
                TournamentSize = 10;
                NumCopy = 50;
                NumCrossover = 30;
                NumMutate = 20;
                MinDepth = 2;
                MaxDepth = 6;
                Generations = 1000;
        }
    };
}
