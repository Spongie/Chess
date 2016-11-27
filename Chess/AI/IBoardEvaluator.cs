using Chess.Pieces;

namespace Chess.AI
{
    public interface IBoardEvaluator
    {
        float EvaluateBoard(ChessBoard board, Color color);
    }
}
