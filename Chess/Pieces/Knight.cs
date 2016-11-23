using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color) : base(color)
        {
            MaxMoveLength = 7;
        }

        public override IEnumerable<Move> GetLegalMoves()
        {
            var legalMoves = new List<Move>();
            var startPosition = ChessBoard.Instance.GetPiecePosition(this);

            AddMove(GetMoveToPosition(startPosition.X + 1, startPosition.Y + 2), legalMoves);
            AddMove(GetMoveToPosition(startPosition.X - 1, startPosition.Y + 2), legalMoves);
            AddMove(GetMoveToPosition(startPosition.X + 1, startPosition.Y - 2), legalMoves);
            AddMove(GetMoveToPosition(startPosition.X - 1, startPosition.Y - 2), legalMoves);
            AddMove(GetMoveToPosition(startPosition.X + 2, startPosition.Y + 1), legalMoves);
            AddMove(GetMoveToPosition(startPosition.X - 2, startPosition.Y + 1), legalMoves);
            AddMove(GetMoveToPosition(startPosition.X + 2, startPosition.Y - 1), legalMoves);
            AddMove(GetMoveToPosition(startPosition.X - 2, startPosition.Y - 1), legalMoves);

            return legalMoves;
        }
    }
}