using System.Collections.Generic;

namespace Chess.Pieces
{
    public enum Color
    {
        White,
        Black
    }

    public abstract class Piece
    {
        protected Piece(Color color)
        {
            Color = color;
            MaxMoveLength = 1;
        }

        public Color Color { get; protected set; }

        public int MaxMoveLength { get; protected set; }

        public abstract IEnumerable<Move> GetLegalMoves();

        protected IEnumerable<Move> GetHorizontalMoves()
        {
            var startPosition = ChessBoard.Instance.GetPiecePosition(this);
            var legalMoves = new List<Move>();

            for (int moveLength = 1; moveLength < MaxMoveLength; moveLength++)
            {
                AddMove(GetMoveToPosition(startPosition.X + moveLength, startPosition.Y), legalMoves);
                AddMove(GetMoveToPosition(startPosition.X - moveLength, startPosition.Y), legalMoves);
            }

            return legalMoves;
        }

        protected IEnumerable<Move> GetVerticalUpMoves()
        {
            var startPosition = ChessBoard.Instance.GetPiecePosition(this);
            var legalMoves = new List<Move>();

            for (int moveLength = 1; moveLength < MaxMoveLength; moveLength++)
            {
                AddMove(GetMoveToPosition(startPosition.X, startPosition.Y - moveLength), legalMoves);
            }

            return legalMoves;
        }

        protected IEnumerable<Move> GetVerticalDownMoves()
        {
            var startPosition = ChessBoard.Instance.GetPiecePosition(this);
            var legalMoves = new List<Move>();

            for (int moveLength = 1; moveLength < MaxMoveLength; moveLength++)
            {
                AddMove(GetMoveToPosition(startPosition.X, startPosition.Y + moveLength), legalMoves);
            }

            return legalMoves;
        }

        protected IEnumerable<Move> GetDiagonalMoves()
        {
            var startPosition = ChessBoard.Instance.GetPiecePosition(this);
            var legalMoves = new List<Move>();

            for (int moveLength = 1; moveLength < MaxMoveLength; moveLength++)
            {
                AddMove(GetMoveToPosition(startPosition.X + moveLength, startPosition.Y + moveLength), legalMoves);
                AddMove(GetMoveToPosition(startPosition.X - moveLength, startPosition.Y + moveLength), legalMoves);
                AddMove(GetMoveToPosition(startPosition.X + moveLength, startPosition.Y - moveLength), legalMoves);
                AddMove(GetMoveToPosition(startPosition.X - moveLength, startPosition.Y - moveLength), legalMoves);
            }

            return legalMoves;
        }



        protected Move GetMoveToPosition(int x, int y)
        {
            if (ChessBoard.Instance.IsPositionInsideBoard(x, y))
            {
                var pieceOnTarget = ChessBoard.Instance.GetPieceAtPosition(x, y);

                if (pieceOnTarget == null || pieceOnTarget.Color != Color)
                    return new Move { Piece = this, TargetPosition = new Position(y, x) };
            }

            return null;
        }

        public void AddMove(Move move, List<Move> legalMoves)
        {
            if (move != null)
                legalMoves.Add(move);
        }
    }
}
