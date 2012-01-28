namespace Tree {
    public abstract class Generation {

        public delegate double Solver(double x);
        public abstract Solver[] Solutions { get; }
    }
}
