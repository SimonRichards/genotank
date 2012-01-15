using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace genotank {
    [TestClass]
    internal class NodesTest {
        [TestMethod]
        internal void TestSum() {
            AddOperator adder = new AddOperator();
            adder.children[0] = new Constant(1);
            adder.children[1] = new Constant(2);
            Assert.AreEqual(3, adder.Solve());
        }
        /*
        [TestMethod]
        internal void TestDivide() {
            ProtectedDivideOperator divider = new ProtectedDivideOperator();
            divider.children[0] = new Constant(1);
            divider.children[1] = new Constant(2);
            Assert.AreEqual(0.5, divider.Solve());
        }

        [TestMethod]
        internal void TestDivideByZero() {
            ProtectedDivideOperator divider = new ProtectedDivideOperator();
            divider.children[0] = new Constant(1);
            divider.children[1] = new Constant(0);
            Assert.AreEqual(1, divider.Solve());
        }

        [TestMethod]
        internal void TestSubtract() {
            SubtractOperator subtracter = new SubtractOperator();
            subtracter.children[0] = new Constant(1);
            subtracter.children[1] = new Constant(2);
            Assert.AreEqual(-1, subtracter.Solve());
        }
          
        [TestMethod]
        internal void TestExponent() {
            ExponentOperator twoToTheFive = new ExponentOperator();
            twoToTheFive.children[0] = new Constant(5);
            twoToTheFive.children[1] = new Constant(2);
            Assert.AreEqual(25, twoToTheFive.Solve());
        }
          
        [TestMethod]
        internal void TestCompare() {
            Node comp = new CompareOperator();
            comp.children[0] = new Constant(1);
            comp.children[1] = new Constant(2);
            comp.children[2] = new Constant(3);
            comp.children[3] = new Constant(4);
            Assert.AreEqual(3, comp.Solve());
        }*/
    }
}
