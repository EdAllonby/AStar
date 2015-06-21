namespace AStar
{
    public interface IHeuristicCalculator
    {
        void CalculateHeuristic(Node currentNode, Node endNode);
    }
}