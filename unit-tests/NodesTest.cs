using Microsoft.VisualStudio.TestTools.UnitTesting;
using genotank;

namespace unit_tests {

    [TestClass]
    public class NodesTest {
        [TestMethod]
        public void TestSum() {
            var adder = new AddOperator(new Constant(1), new Constant(2));
            Assert.AreEqual(3, adder.Solve());
        }
        
        [TestMethod]
        public void TestDivide() {
            var divider = new ProtectedDivideOperator(new Constant(1), new Constant(2));
            Assert.AreEqual(0.5, divider.Solve());
        }

        [TestMethod]
        public void TestDivideByZero() {
            var divider = new ProtectedDivideOperator(new Constant(1), new Constant(0));
            Assert.AreEqual(1, divider.Solve());
        }

        [TestMethod]
        public void TestSubtract() {
            var subtracter = new SubtractOperator(new Constant(1), new Constant(2));
            Assert.AreEqual(-1, subtracter.Solve());
        }
          
        [TestMethod]
        public void TestExponent() {
            var twoToTheFive = new ExponentOperator(new Constant(5), new Constant(2));
            Assert.AreEqual(25, twoToTheFive.Solve());
        }
          
        [TestMethod]
        public void TestCompare() {
            Node comp = new CompareOperator(new Constant(1),  new Constant(2), new Constant(3), new Constant(4));
            Assert.AreEqual(3, comp.Solve());
        }
    }
}
