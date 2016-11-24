using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Color color) : base(color)
        {
            MaxMoveLength = 2;
        }

        public override IEnumerable<Move> GetLegalMoves(ChessBoard board)
        {
            return Color == Color.White ? GetVerticalUpMoves(board) : GetVerticalDownMoves(board);
        }

        public override void OnMoved()
        {
            MaxMoveLength = 1;
            base.OnMoved();
        }
    }
}