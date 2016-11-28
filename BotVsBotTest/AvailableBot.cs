using Chess.AI;

namespace BotVsBotTest
{
    public class AvailableBot
    {
        public string Name { get; set; }
        public IBoardEvaluator BoardEvaluator { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}