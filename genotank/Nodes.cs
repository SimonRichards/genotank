using System.Collections.Generic;
using System;
using System.Diagnostics;
namespace genotank {

    abstract class Node {
        private static Random _rng = new Random();

        internal abstract void Accept(NodeVisitor vistor);

        internal delegate void NodeVisitor(Node node);

        protected Node (int arity) {
            Arity = arity;
        }

        internal int Count {
            get {
                int sum = 0;
                NodeVisitor counter = node => sum++;
                Accept(counter);
                return sum;
            }
        }


        internal abstract Node this[int i] { get; }
        internal abstract bool Find(Stack<Node> stack, Node target);
        abstract internal double Solve();
        abstract override public string ToString();
        internal int Arity { get; private set; }

        internal abstract Node Clone(Stack<Node> chainToReplace, Node newNode);

    };

    abstract class BinaryOperator : Node {
        protected readonly Node Left;
        protected readonly Node Right;
        internal BinaryOperator(Node left, Node right) : base(2) {
            Left = left;
            Right = right;
        }

        
        internal override bool Find(Stack<Node> stack, Node target) {
            bool result =  this == target || Left.Find(stack, target) || Right.Find(stack, target);
            if (result) {
                stack.Push(this);
            }
            return result;
        }

        internal override void Accept(NodeVisitor visitor) {
            Left.Accept(visitor);
            Right.Accept(visitor);
            visitor(this);
        }

        internal override Node this[int i] { get { return i == 0 ? Left : Right; } }

        abstract internal string Symbol { get; }

        override public string ToString() {
            return "(" + Left + Symbol + Right + ")";
        }
    }

    class AddOperator : BinaryOperator {
        internal AddOperator(Node left, Node right) : base(left, right) {}

        internal override Node Clone(Stack<Node> chainToReplace, Node newNode) {
            var replacedChild = chainToReplace.Pop();
            if (chainToReplace.Count == 0) {
                return replacedChild == Left ? new AddOperator(newNode, Right) : new AddOperator(Left, newNode);
            }
            return replacedChild == Left ? new AddOperator(Left.Clone(chainToReplace, newNode), Right) : new AddOperator(Left, Right.Clone(chainToReplace, newNode));
        }

        override internal double Solve() {
            return Left.Solve() + Right.Solve();
        }

        override internal string Symbol {
            get {
                return " + ";
            }
        }
    };
    
    class SubtractOperator : BinaryOperator {
        internal SubtractOperator(Node left, Node right) : base(left, right) {}

        internal override Node Clone(Stack<Node> chainToReplace, Node newNode) {
            var replacedChild = chainToReplace.Pop();
            if (chainToReplace.Count == 0) {
                return replacedChild == Left ? new SubtractOperator(newNode, Right) : new SubtractOperator(Left, newNode);
            }
            return replacedChild == Left ? new SubtractOperator(Left.Clone(chainToReplace, newNode), Right) : new SubtractOperator(Left, Right.Clone(chainToReplace, newNode));
        }

        override internal double Solve() {
            return Left.Solve() - Right.Solve();
        }

        override internal string Symbol {
            get {
                return " - ";
            }
        }
    };
    
    class MultiplyOperator : BinaryOperator {
        internal MultiplyOperator(Node left, Node right) : base(left, right) { }  // all these could be cast from BinaryOperator

        internal override Node Clone(Stack<Node> chainToReplace, Node newNode) {
            var replacedChild = chainToReplace.Pop();
            if (chainToReplace.Count == 0) {
                return replacedChild == Left ? new MultiplyOperator(newNode, Right) : new MultiplyOperator(Left, newNode);
            }
            return replacedChild == Left ? new MultiplyOperator(Left.Clone(chainToReplace, newNode), Right) : new MultiplyOperator(Left, Right.Clone(chainToReplace, newNode));
        }

        override internal double Solve() {
            return Left.Solve() * Right.Solve();
        }

        override internal string Symbol {
            get {
                return " * ";
            }
        }
    };
    
       
    class ProtectedDivideOperator : BinaryOperator {
        internal ProtectedDivideOperator(Node left, Node right) : base(left, right) { }

        internal override Node Clone(Stack<Node> chainToReplace, Node newNode) {
            var replacedChild = chainToReplace.Pop();
            return chainToReplace.Count == 0 ? (
                    replacedChild == Left ? 
                        new ProtectedDivideOperator(newNode, Right) :
                        new ProtectedDivideOperator(Left, newNode)) :
                    replacedChild == Left
                        ? new ProtectedDivideOperator(Left.Clone(chainToReplace, newNode), Right)
                        : new ProtectedDivideOperator(Left, Right.Clone(chainToReplace, newNode));
        }

        override internal double Solve() {
            double result = Left.Solve() / Right.Solve();
            return Double.IsInfinity(result) || Double.IsInfinity(result) ? 1 : result;
        }

        override internal string Symbol {
            get {
                return " / ";
            }
        }
    };
    
   
    class ExponentOperator : BinaryOperator {
        internal ExponentOperator(Node left, Node right) : base(left, right) { }

        internal override Node Clone(Stack<Node> chainToReplace, Node newNode) {
            var replacedChild = chainToReplace.Pop();
            return chainToReplace.Count == 0 ? (
                    replacedChild == Left ?
                        new ExponentOperator(newNode, Right) :
                        new ExponentOperator(Left, newNode)) :
                    replacedChild == Left
                        ? new ExponentOperator(Left.Clone(chainToReplace, newNode), Right)
                        : new ExponentOperator(Left, Right.Clone(chainToReplace, newNode));
        }

        override internal double Solve() {
            try {
                return Math.Pow(Left.Solve(), Right.Solve());
            } catch {
                return 1;
            }
        }

        override internal string Symbol {
            get {
                return "^";
            }
        }
    };
    
    
    class CompareOperator : Node {
        private readonly Node _a, _b, _c, _d;
        private readonly Node[] _children;
        internal CompareOperator(Node a, Node b, Node c, Node d) : base(4) {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
            _children = new[] {_a, _b, _c, _d};
        }

        internal override Node Clone(Stack<Node> chainToReplace, Node newNode) {
            throw new OperationCanceledException();
            /*
            var replacedChild = chainToReplace.Pop();
            Node a, b, c, d;
            return chainToReplace.Count == 0 ? (
                    replacedChild == Left ?
                        new ExponentOperator(newNode, Right) :
                        new ExponentOperator(Left, newNode)) :
                    replacedChild == Left ?
                        new ExponentOperator(Left.Clone(chainToReplace, newNode), Right) :
                        new ExponentOperator(Left, Right.Clone(chainToReplace, newNode));
             */
        }

        internal override Node this[int i] { get { return _children[i]; } }

        internal override double Solve() {
            return _a.Solve() < _b.Solve() ? _c.Solve() : _d.Solve();
        }

        public override string ToString() {
            return 
                "(" + _a + " < " + _b + " ? " + _c + " : " + _d + ")";
        }

        internal override bool Find(Stack<Node> stack, Node target) {
            bool result = this == target || _a.Find(stack, target) || _b.Find(stack, target) || _c.Find(stack, target) || _d.Find(stack, target);
            if (result) {
                stack.Push(this);
            }
            return result;
        }

        internal override void Accept(NodeVisitor visitor) {
            foreach (var child in _children) {
                child.Accept(visitor);
            }
            visitor(this);
        }

    }
    

    abstract class Terminal : Node {
        internal override void Accept(NodeVisitor visitor) {
            visitor(this);
        }

        internal override Node Clone(Stack<Node> chainToReplace, Node newNode) {
            Debugger.Break();
            throw new Exception("Can't replace child of a terminal");
        }

        internal override Node this[int i] {
            get { throw new NotImplementedException(); }
        }

        internal override bool Find(Stack<Node> stack, Node target) {
            if (this == target) {
                stack.Push(this);
                return true;
            }
            return false;
        }

        internal Terminal() : base(0) {}
    }

    class Constant : Terminal {
        readonly double _value;

        internal Constant(double value) {
            _value = value;
        }


        override internal double Solve() {
            return _value;
        }

        override public string ToString() {
            return _value.ToString("F");
        }
    };

    class Variable : Terminal {
        readonly string _name;

        internal Variable(string name) {
            _name = name;
        }

        internal override double Solve() {
            return Value;
        }

        internal double Value { get; set; }

        override public string ToString() {
            return _name;
        }
    };
}