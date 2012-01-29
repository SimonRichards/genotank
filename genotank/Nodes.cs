using System.Collections.Generic;
using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
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

        abstract internal double Solve();
        abstract override public string ToString();
        internal int Arity { get; private set; }
    };

    abstract class BinaryOperator : Node {
        protected readonly Node Left;
        protected readonly Node Right;
        internal BinaryOperator(Node left, Node right) : base(2) {
            Left = left;
            Right = right;
        }

        internal override void Accept(NodeVisitor visitor) {
            Left.Accept(visitor);
            Right.Accept(visitor);
            visitor(this);
        }

        abstract internal string Symbol { get; }

        override public string ToString() {
            return "(" + Left + Symbol + Right + ")";
        }
    }

    class AddOperator : BinaryOperator {
        internal AddOperator(Node left, Node right) : base(left, right) {}

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
        internal SubtractOperator(Node left, Node right) : base(left, right) { }
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
        override internal double Solve() {
            return Left.Solve() * Right.Solve();
        }

        override internal string Symbol {
            get {
                return " * ";
            }
        }
    };
    /*
       
    class ProtectedDivideOperator : BinaryOperator {
        override internal double Solve() {
            double result = Children[0].Solve() / Children[1].Solve();
            return Double.IsInfinity(result) || Double.IsInfinity(result) ? 1 : result;
        }

        override internal string Symbol {
            get {
                return " / ";
            }
        }
    };
    */
    /*
    class ExponentOperator : BinaryOperator {
        override internal double Solve() {
            try {
                return Math.Pow(children[0].Solve(), children[1].Solve());
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
    */
    /*
    class CompareOperator : Node {

        internal override double Solve() {
            return children[0].Solve() < children[1].Solve() ? children[2].Solve() : children[3].Solve();
        }

        override internal int Arity {
            get {
                return 4;
            }
        }

        public override string ToString() {
            return 
                "(" + children[0].ToString() + " < " + children[1].ToString() + " ? " + children[2].ToString() + " : " + children[3].ToString() + ")";
        }

    }
    */

    abstract class Terminal : Node {
        internal override void Accept(NodeVisitor visitor) {
            visitor(this);
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