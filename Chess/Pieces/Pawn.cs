using System;
using System.Collections.Generic;

namespace Chess.Pieces
{
    [Serializable]
    public class Pawn : Piece
    {
        public Pawn()
        {
            
        }

        public Pawn(Color color) : base(color)
        {
            MaxMoveLength = 2;
        }

        public override IEnumerable<Move> GetLegalMoves(ChessBoard board)
        {
            var moves = GetAttackingMoves(board);

            moves.AddRange(Color == Color.White ? GetVerticalUpMoves(board, true) : GetVerticalDownMoves(board, true));

            return moves;
        }

        private List<Move> GetAttackingMoves(ChessBoard board)
        {
            var moves = new List<Move>();
            var position = board.GetPiecePosition(this);
            var attackingDirection = (int) Color;

            var attackPieceRight = board.GetPieceAtPosition(position.X + 1, position.Y + attackingDirection);
            var attackPieceLight = board.GetPieceAtPosition(position.X - 1, position.Y + attackingDirection);

            if (attackPieceRight != null && attackPieceRight.Color != Color)
                moves.Add(new Move
                {
                    Piece = this,
                    TargetPosition = new Position(position.Y + attackingDirection, position.X + 1)
                });

            if (attackPieceLight != null && attackPieceLight.Color != Color)
                moves.Add(new Move
                {
                    Piece = this,
                    TargetPosition = new Position(position.Y + attackingDirection, position.X - 1)
                });

            return moves;
        }

        public override void OnMoved()
        {
            MaxMoveLength = 1;
            base.OnMoved();
        }

        public override string GetFenRepresentation()
        {
            return Color == Color.Black ? "p" : "P";
        }

        public override string GetName()
        {
            return "Pawn";
        }
    }
}