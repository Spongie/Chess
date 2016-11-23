﻿using System.Collections.Generic;

namespace Chess.Pieces
{
    internal class Pawn : Piece
    {
        public Pawn(Color color) : base(color)
        {
            MaxMoveLength = 2;
        }

        public override IEnumerable<Move> GetLegalMoves()
        {
            throw new System.NotImplementedException();
        }
    }
}