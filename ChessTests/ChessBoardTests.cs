using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Chess;
using Chess.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessTests
{
    [TestClass]
    public class ChessBoardTests
    {
        [TestMethod]
        public void GetAllAvailableMoves_Start20()
        {
            var c = new ChessBoard();

            Assert.AreEqual(20, c.GetAllAvailableMoves(Color.White).Count());
        }
    }
}
