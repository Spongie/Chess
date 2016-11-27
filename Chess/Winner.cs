using System;
using Chess.Pieces;

namespace Chess
{
    [Serializable]
    public class Winner
    {
        public Color Color { get; set; }
        public bool HasWinner { get; set; }
    }
}