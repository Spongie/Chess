using Chess.Pieces;

namespace Chess.AI
{
    public interface IBoardEvaluator
    {
        string Name { get; set; }

        float EvaluateBoard(ChessBoard board, Color color);

        string GetBotId(int depth);
    }
}
