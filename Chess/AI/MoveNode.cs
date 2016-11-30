using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.AI
{
    public class MoveNode
    {
        public List<MoveNode> Children { get; set; }
        public float Score { get; set; }

        public Move Move { get; set; }

        public float GetBestChild()
        {
            var random = new Random();
            return Children.OrderByDescending(child => child.Score).ThenBy(child => random.Next()).First().Score;
        }

        public float GetWorstChild()
        {
            var random = new Random();
            return Children.OrderBy(child => child.Score).ThenBy(child => random.Next()).First().Score;
        }

        public override string ToString()
        {
            return $"{Move} - {Score}";
        }
    }
}
