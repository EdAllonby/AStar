using System;

namespace AStar
{
    public class ManhattanCalculator : IHeuristicCalculator
    {
        public void CalculateHeuristic(Node currentNode, Node endNode)
        {
            int xDifference = Math.Abs(currentNode.Position.X - endNode.Position.X);
            int yDifference = Math.Abs(currentNode.Position.Y - endNode.Position.Y);

            currentNode.Heuristic = 10*(xDifference + yDifference);
        }
    }
}