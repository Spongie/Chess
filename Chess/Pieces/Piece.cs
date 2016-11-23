using System.Collections.Generic;

namespace Chess.Pieces
{
    public enum Color
    {
        White,
        Black
    }

    public enum Directions
    {
        Left,
        Right,
        Up,
        Down,
        RightUp,
        RightDown,
        LeftUp,
        LeftDown
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
            var blockedDirections = new List<Directions>();

            for (int moveLength = 1; moveLength <= MaxMoveLength; moveLength++)
            {
                if(!blockedDirections.Contains(Directions.Right) && !AddMove(GetMoveToPosition(startPosition.X + moveLength, startPosition.Y), legalMoves))
                    blockedDirections.Add(Directions.Right);

                if (!blockedDirections.Contains(Directions.Left) && !AddMove(GetMoveToPosition(startPosition.X - moveLength, startPosition.Y), legalMoves))
                    blockedDirections.Add(Directions.Left);
            }

            return legalMoves;
        }

        protected IEnumerable<Move> GetVerticalUpMoves()
        {
            var startPosition = ChessBoard.Instance.GetPiecePosition(this);
            var legalMoves = new List<Move>();

            for (int moveLength = 1; moveLength <= MaxMoveLength; moveLength++)
            {
                if (!AddMove(GetMoveToPosition(startPosition.X, startPosition.Y - moveLength), legalMoves))
                    break;
            }

            return legalMoves;
        }

        protected IEnumerable<Move> GetVerticalDownMoves()
        {
            var startPosition = ChessBoard.Instance.GetPiecePosition(this);
            var legalMoves = new List<Move>();

            for (int moveLength = 1; moveLength <= MaxMoveLength; moveLength++)
            {
                if (!AddMove(GetMoveToPosition(startPosition.X, startPosition.Y + moveLength), legalMoves))
                    break;
            }

            return legalMoves;
        }

        protected IEnumerable<Move> GetDiagonalMoves()
        {
            var startPosition = ChessBoard.Instance.GetPiecePosition(this);
            var legalMoves = new List<Move>();
            var blockedDirections = new List<Directions>();

            for (int moveLength = 1; moveLength <= MaxMoveLength; moveLength++)
            {
                if (!blockedDirections.Contains(Directions.RightDown) && !AddMove(GetMoveToPosition(startPosition.X + moveLength, startPosition.Y + moveLength), legalMoves))
                    blockedDirections.Add(Directions.RightDown);
                if (!blockedDirections.Contains(Directions.LeftDown) && !AddMove(GetMoveToPosition(startPosition.X - moveLength, startPosition.Y + moveLength), legalMoves))
                    blockedDirections.Add(Directions.LeftDown);
                if (!blockedDirections.Contains(Directions.RightUp) && !AddMove(GetMoveToPosition(startPosition.X + moveLength, startPosition.Y - moveLength), legalMoves))
                    blockedDirections.Add(Directions.RightUp);
                if (!blockedDirections.Contains(Directions.LeftUp) && !AddMove(GetMoveToPosition(startPosition.X - moveLength, startPosition.Y - moveLength), legalMoves))
                    blockedDirections.Add(Directions.LeftUp);
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

        public bool AddMove(Move move, List<Move> legalMoves)
        {
            if (move != null)
            {
                legalMoves.Add(move);
                return true;
            }

            return false;
        }
    }
}
