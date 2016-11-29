using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Pieces
{
    [Serializable]
    public class King : Piece
    {
        public King()
        {
            
        }

        public King(Color color) : base(color)
        {
            MaxMoveLength = 1;
        }

        public override IEnumerable<Move> GetLegalMoves(ChessBoard board)
        {
            var legalMoves = new List<Move>();

            legalMoves.AddRange(GetVerticalUpMoves(board));
            legalMoves.AddRange(GetVerticalDownMoves(board));
            legalMoves.AddRange(GetHorizontalMoves(board));
            legalMoves.AddRange(GetDiagonalMoves(board));

            if (!IgnoreCastle && AmountOfMoves == 0 && !board.IsInCheck(Color))
            {
                AddCastleMoves(board, legalMoves);
            }

            return legalMoves;
        }

        private void AddCastleMoves(ChessBoard board, List<Move> legalMoves)
        {
            var myPos = board.GetKingPosition(Color);
            var rooks = board.GetPiecesForColor(Color).Where(piece => piece is Rook && piece.AmountOfMoves == 0 && board.GetPiecePosition(piece).Y == myPos.Y).ToList();

            if (myPos.Y != 0 && myPos.Y != 7)
                return;

            var rightRook = rooks.FirstOrDefault(rook => board.GetPiecePosition(rook).X > myPos.X);

            if (rightRook != null)
            {
                AddRightCastleMove(board, myPos, rightRook, legalMoves);
            }

            var leftRook = rooks.FirstOrDefault(rook => board.GetPiecePosition(rook).X < myPos.X);

            if (leftRook != null)
            {
                AddLeftCastleMove(board, myPos, leftRook, legalMoves);
            }
        }

        private void AddRightCastleMove(ChessBoard board, Position myPos, Piece rightRook, List<Move> legalMoves)
        {
            bool movePossible = true;

            for (int x = myPos.X + 1; x < board.GetPiecePosition(rightRook).X; x++)
            {
                var piece = board.GetPieceAtPosition(x, myPos.Y);

                if (piece != null)
                    movePossible = false;
            }

            if (movePossible)
            {
                AddMove(
                    new Move
                    {
                        Piece = this,
                        TargetPosition = new Position(myPos.Y, myPos.X + 2),
                        IsCastleMove = true,
                        CastleRook = (Rook) rightRook,
                        RookTargetPosition = new Position(myPos.Y, myPos.X + 1)
                    }, legalMoves, board);
            }
        }

        private void AddLeftCastleMove(ChessBoard board, Position myPos, Piece leftRook, List<Move> legalMoves)
        {
            bool movePossible = true;

            for (int x = myPos.X - 1; x > board.GetPiecePosition(leftRook).X; x--)
            {
                var piece = board.GetPieceAtPosition(x, myPos.Y);

                if (piece != null)
                    movePossible = false;
            }

            if (movePossible)
            {
                AddMove(
                    new Move
                    {
                        Piece = this,
                        TargetPosition = new Position(myPos.Y, myPos.X - 2),
                        IsCastleMove = true,
                        CastleRook = (Rook)leftRook,
                        RookTargetPosition = new Position(myPos.Y, myPos.X - 1)
                    }, legalMoves, board);
            }
        }

        public override string GetFenRepresentation()
        {
            return Color == Color.Black ? "k" : "K";
        }
        public override string GetName()
        {
            return "King";
        }

        public bool IgnoreCastle { get; set; }
    }
}