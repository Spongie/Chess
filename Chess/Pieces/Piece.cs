using System.Collections.Generic;

namespace Chess.Pieces
{
    public enum Color
    {
        White = -1,
        Black = 1
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

        public int AmountOfMoves { get; protected set; }

        public abstract IEnumerable<Move> GetLegalMoves(ChessBoard board);

        public virtual void OnMoved()
        {
            AmountOfMoves++;
        }

        protected IEnumerable<Move> GetHorizontalMoves(ChessBoard board)
        {
            var startPosition = board.GetPiecePosition(this);
            var legalMoves = new List<Move>();
            var blockedDirections = new List<Directions>();

            for (int moveLength = 1; moveLength <= MaxMoveLength; moveLength++)
            {
                if(!blockedDirections.Contains(Directions.Right) && !AddMove(GetMoveToPosition(startPosition.X + moveLength, startPosition.Y, board), legalMoves, board))
                    blockedDirections.Add(Directions.Right);

                if (!blockedDirections.Contains(Directions.Left) && !AddMove(GetMoveToPosition(startPosition.X - moveLength, startPosition.Y, board), legalMoves, board))
                    blockedDirections.Add(Directions.Left);
            }

            return legalMoves;
        }

        protected IEnumerable<Move> GetVerticalUpMoves(ChessBoard board)
        {
            var startPosition = board.GetPiecePosition(this);
            var legalMoves = new List<Move>();

            for (int moveLength = 1; moveLength <= MaxMoveLength; moveLength++)
            {
                if (!AddMove(GetMoveToPosition(startPosition.X, startPosition.Y - moveLength, board), legalMoves, board))
                    break;
            }

            return legalMoves;
        }

        protected IEnumerable<Move> GetVerticalDownMoves(ChessBoard board)
        {
            var startPosition = board.GetPiecePosition(this);
            var legalMoves = new List<Move>();

            for (int moveLength = 1; moveLength <= MaxMoveLength; moveLength++)
            {
                if (!AddMove(GetMoveToPosition(startPosition.X, startPosition.Y + moveLength, board), legalMoves, board))
                    break;
            }

            return legalMoves;
        }

        protected IEnumerable<Move> GetDiagonalMoves(ChessBoard board)
        {
            var startPosition = board.GetPiecePosition(this);
            var legalMoves = new List<Move>();
            var blockedDirections = new List<Directions>();

            for (int moveLength = 1; moveLength <= MaxMoveLength; moveLength++)
            {
                if (!blockedDirections.Contains(Directions.RightDown) && !AddMove(GetMoveToPosition(startPosition.X + moveLength, startPosition.Y + moveLength, board), legalMoves, board))
                    blockedDirections.Add(Directions.RightDown);
                if (!blockedDirections.Contains(Directions.LeftDown) && !AddMove(GetMoveToPosition(startPosition.X - moveLength, startPosition.Y + moveLength, board), legalMoves, board))
                    blockedDirections.Add(Directions.LeftDown);
                if (!blockedDirections.Contains(Directions.RightUp) && !AddMove(GetMoveToPosition(startPosition.X + moveLength, startPosition.Y - moveLength, board), legalMoves, board))
                    blockedDirections.Add(Directions.RightUp);
                if (!blockedDirections.Contains(Directions.LeftUp) && !AddMove(GetMoveToPosition(startPosition.X - moveLength, startPosition.Y - moveLength, board), legalMoves, board))
                    blockedDirections.Add(Directions.LeftUp);
            }

            return legalMoves;
        }

        protected Move GetMoveToPosition(int x, int y, ChessBoard board)
        {
            if (board.IsPositionInsideBoard(x, y))
            {
                var pieceOnTarget = board.GetPieceAtPosition(x, y);

                if (pieceOnTarget == null || pieceOnTarget.Color != Color)
                    return new Move { Piece = this, TargetPosition = new Position(y, x) };
            }

            return null;
        }

        public bool AddMove(Move move, List<Move> legalMoves, ChessBoard board)
        {
            if (move != null)
            {
                legalMoves.Add(move);

                var pieceAtTarget = board.GetPieceAtPosition(move.TargetPosition.X, move.TargetPosition.Y);
                if (pieceAtTarget != null && pieceAtTarget.Color == ChessBoard.InvertColor(Color))
                    return false;

                return true;
            }

            return false;
        }
    }
}
