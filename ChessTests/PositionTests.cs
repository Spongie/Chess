using Chess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessTests
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void Equals_BothSame_True()
        {
            var pos1 = new Position(1, 1);
            var pos2 = new Position(1, 1);

            Assert.IsTrue(pos1.Equals(pos2));
        }

        [TestMethod]
        public void Equals_OnlyXSame_False()
        {
            var pos1 = new Position(1, 12);
            var pos2 = new Position(1, 1);

            Assert.IsFalse(pos1.Equals(pos2));
        }

        [TestMethod]
        public void Equals_OnlyYSame_False()
        {
            var pos1 = new Position(1, 12);
            var pos2 = new Position(12, 12);

            Assert.IsFalse(pos1.Equals(pos2));
        }

        [TestMethod]
        public void Equals_NoneSame_False()
        {
            var pos1 = new Position(1, 12);
            var pos2 = new Position(31, 133);

            Assert.IsFalse(pos1.Equals(pos2));
        }
    }
}
