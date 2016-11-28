using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;
using Newtonsoft.Json;

namespace Chess.AI
{
    public class ValueByPieceEvalutator : IBoardEvaluator, ISavableConfigation
    {
        private Dictionary<Type, float> pieceValues;

        public ValueByPieceEvalutator()
        {
            pieceValues = new Dictionary<Type, float>
            {
                { typeof(Pawn), 1f },
                { typeof(Rook), 3.5f },
                { typeof(Bishop), 3f },
                { typeof(Knight), 4f },
                { typeof(Queen), 6f },
                { typeof(King), 10f }
            };
        }

        public Dictionary<Type, float> PieceValues => pieceValues;

        public string Name { get; set; }

        public float EvaluateBoard(ChessBoard board, Color color)
        {
            var myPieces = board.GetPiecesForColor(color).Select(piece => pieceValues[piece.GetType()]).Sum();
            var opponentPieces = board.GetPiecesForColor(ChessBoard.InvertColor(color)).Select(piece => pieceValues[piece.GetType()]).Sum();

            return myPieces - opponentPieces;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(pieceValues);
        }

        public void DeSerialize(string data)
        {
            pieceValues = JsonConvert.DeserializeObject<Dictionary<Type, float>>(data);
        }
    }
}
