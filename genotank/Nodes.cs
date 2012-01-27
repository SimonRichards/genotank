using System.Collections.Generic;
using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
namespace genotank {

    abstract class Node {
        internal Node[] children;

        protected Node () {
             children = new Node[this.Arity];
        }

        abstract internal double Solve();
        abstract override public string ToString();
        abstract internal int Arity {get;}

        internal virtual Node DeepClone() {
            Node head = (Node)this.MemberwiseClone();
            head.children = new Node[head.Arity];
            for (int i = 0; i < children.Length; i++) {
                head.children[i] = children[i].DeepClone();
            }
            return head;
        }

        internal void AddToList(List<Node> allNodes) {
            allNodes.Add(this);
            foreach (Node child in children) {
                child.AddToList(allNodes);
            }
        }

    };

    abstract class BinaryOperator : Node {
        abstract internal string Symbol { get; }

        override internal int Arity {
            get {
                return 2;
            }
        }

        override public string ToString() {
            return
                '(' +
                children[0].ToString() +
                Symbol +
                children[1].ToString() +
                ')';
        }
    }

    class AddOperator : BinaryOperator {
        override internal double Solve() {
            double a = children[0].Solve();
            double b = children[1].Solve();
            return a + b;
        }

        override internal string Symbol {
            get {
                return " + ";
            }
        }
    };
    
    class SubtractOperator : BinaryOperator {
        override internal double Solve() {
            double a = children[0].Solve();
            double b = children[1].Solve();
            return a - b;
        }

        override internal string Symbol {
            get {
                return " - ";
            }
        }
    };
    
    class MultiplyOperator : BinaryOperator {
        override internal double Solve() {
            double a = children[0].Solve();
            double b = children[1].Solve();
            return a * b;
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
            double result = children[0].Solve() / children[1].Solve();
            return Double.IsInfinity(result) ? 1 : result;
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

    class Constant : Node {
        readonly double _value;

        internal Constant(double value) {
            _value = value;
        }

        override internal double Solve() {
            return _value;
        }

        override internal int Arity {
            get {
                return 0;
            }
        }

        override public string ToString() {
            return _value.ToString("F");
        }
    };

    class Variable : Node {
        string _name;
        double _value;

        internal Variable(string name) {
            _name = name;
        }

        internal override Node DeepClone() {
            return this;
        }

        internal override double Solve() {
            return _value;
        }

        internal double Value {
            get {
                return _value;
            }
            set {
                _value = value;
            }
        }

        override internal int Arity {
            get {
                return 0;
            }
        }

        override public string ToString() {
            return _name;
        }
    };
}