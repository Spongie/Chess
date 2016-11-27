using System;
using System.Collections.Generic;

namespace Chess.Pieces
{
    [Serializable]
    public class Knight : Piece
    {
        public Knight(Color color) : base(color)
        {
            MaxMoveLength = 7;
        }

        public override IEnumerable<Move> GetLegalMoves(ChessBoard board)
        {
            var legalMoves = new List<Move>();
            var startPosition = board.GetPiecePosition(this);

            AddMove(GetMoveToPosition(startPosition.X + 1, startPosition.Y + 2, board), legalMoves, board);
            AddMove(GetMoveToPosition(startPosition.X - 1, startPosition.Y + 2, board), legalMoves, board);
            AddMove(GetMoveToPosition(startPosition.X + 1, startPosition.Y - 2, board), legalMoves, board);
            AddMove(GetMoveToPosition(startPosition.X - 1, startPosition.Y - 2, board), legalMoves, board);
            AddMove(GetMoveToPosition(startPosition.X + 2, startPosition.Y + 1, board), legalMoves, board);
            AddMove(GetMoveToPosition(startPosition.X - 2, startPosition.Y + 1, board), legalMoves, board);
            AddMove(GetMoveToPosition(startPosition.X + 2, startPosition.Y - 1, board), legalMoves, board);
            AddMove(GetMoveToPosition(startPosition.X - 2, startPosition.Y - 1, board), legalMoves, board);

            return legalMoves;
        }

        public override string GetFenRepresentation()
        {
            return Color == Color.Black ? "n" : "N";
        }
        public override string GetName()
        {
            return "Knight";
        }
    }
}