using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace genotank {
    class God {
        private delegate Expression ExpressionFactory(int selection, int depth);
        readonly Random _random = new Random(Configuration.Seed);
        readonly List<ParameterExpression>  _inputs;

        private const int NonTerminalExpressions = 3;
        private const int AllExpressions = NonTerminalExpressions + 2;

        readonly int _numInputs;
        readonly int _numOutputs;

        Configuration _config;

        internal God(Configuration config, IEnumerable<string> inputs, int numOutputs) {
            _numOutputs = numOutputs;
            _numInputs = inputs.Count();
            _config = config;
            _inputs = new List<ParameterExpression>(_numInputs);
            foreach (var input in inputs) {
                _inputs.Add(Expression.Parameter(typeof(double), input));
            }
        }

        internal Genome BuildGenome() {
            return new Genome(
                Expression.Lambda<Func<double, double>>(
                    (_random.Next(2) == 1 ? Full() : Grow()),
                    _inputs.ToArray()), this);
        }


        private Expression Full() {
            return RandomExpression(AllExpressions, 0);
        }

        private Expression Grow() {
            return RandomExpression(NonTerminalExpressions, 0);
        }

        private Expression RandomExpression(int selection, int depth) {
            var factory = depth == _config.MaxDepth ?
                (ExpressionFactory)RandomTerminal : RandomExpression;
            switch (_random.Next(selection)) {
                case 0:
                    return Expression.Add(factory(selection, depth + 1), factory(selection, depth + 1));
                case 1:
                    return Expression.Subtract(factory(selection, depth + 1), factory(selection, depth + 1));
                case 2:
                    return Expression.Multiply(factory(selection, depth + 1), factory(selection, depth + 1));
                default:
                    return RandomTerminal(0, 0);
            }
        }

        private Expression RandomTerminal(int selection, int depth) {
            switch (_random.Next(2)) {
                case 0:
                    return _inputs[_random.Next(_inputs.Count)];
                case 1:
                    return Expression.Constant(_random.NextDouble(), typeof(double));
                default:
                    throw new Exception("You fucked up");
            }
        }
    }
}
