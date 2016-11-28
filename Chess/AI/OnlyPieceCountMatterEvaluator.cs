using System.Linq;
using Chess.Pieces;

namespace Chess.AI
{
    public class OnlyPieceCountMatterEvaluator  : IBoardEvaluator
    {
        public OnlyPieceCountMatterEvaluator()
        {
            Name = "Only Piece Count";
        }

        public string Name { get; set; }

        public float EvaluateBoard(ChessBoard board, Color color)
        {
            var myPieces = board.GetPiecesForColor(color).Count();
            var opponentPieces = board.GetPiecesForColor(ChessBoard.InvertColor(color)).Count();

            return myPieces - opponentPieces;
        }
    }
}
