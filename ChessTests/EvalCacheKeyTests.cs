using Chess;
using Chess.AI;
using Chess.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessTests
{
    [TestClass]
    public class EvalCacheKeyTests
    {
        [TestMethod]
        public void EqualityTest()
        {
            var c = new ChessBoard();

            var cacheKey = new EvalCacheKey(c.GetFenString(), Color.White);
            var cacheKey2 = new EvalCacheKey(c.GetFenString(), Color.White);

            Assert.AreEqual(cacheKey, cacheKey2);
        }
    }
}
