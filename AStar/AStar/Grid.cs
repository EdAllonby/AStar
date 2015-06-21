using System.Collections.Generic;
using System.Linq;

namespace AStar
{
    public sealed class Grid
    {
        private const int DiagonalMoveCost = 14;
        private const int SimpleMoveCost = 10;
        private readonly List<Node> closedNodes = new List<Node>();
        private readonly List<Node> nodes;
        private readonly List<Node> openNodes = new List<Node>();

        public Grid(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public Node EndNode { get; set; }
        public Node StartNode { get; set; }

        public IEnumerable<Node> Nodes
        {
            get { return nodes; }
        }

        public IEnumerable<Node> CalculatePath(IHeuristicCalculator heuristicCalculator)
        {
            if (EndNode == null || StartNode == null)
            {
                return new List<Node>();
            }

            Reset();

            CalculateHeuristics(heuristicCalculator);

            AddBestNodeToClosedNodes(StartNode);
            CalculateMoveCostsToSurroundingNodes(StartNode);

            bool foundPath = false;

            while (!foundPath)
            {
                Node bestNode = FindBestNode();
                AddBestNodeToClosedNodes(bestNode);
                CalculateMoveCostsToSurroundingNodes(bestNode);

                var endNodeParent = IsClosedNodeNextToEndNode();

                if (endNodeParent != null)
                {
                    foundPath = true;
                    Reparent(endNodeParent, EndNode);
                }
            }

            return GetBestPath();
        }

        private IEnumerable<Node> GetBestPath()
        {
            List<Node> path = new List<Node> {EndNode};

            bool startNodeFound = false;

            Node lastNode = EndNode;

            while (!startNodeFound)
            {
                lastNode = lastNode.Parent;

                path.Add(lastNode);

                if (lastNode.Equals(StartNode))
                {
                    startNodeFound = true;
                }
            }

            return path;
        }

        private void Reset()
        {
            openNodes.Clear();
            closedNodes.Clear();
        }

        private Node IsClosedNodeNextToEndNode()
        {
            return closedNodes.FirstOrDefault(x => x.Position.IsCoordinateNextTo(EndNode.Position));
        }

        private void CalculateHeuristics(IHeuristicCalculator calculator)
        {
            foreach (Node node in nodes)
            {
                calculator.CalculateHeuristic(node, EndNode);
            }
        }

        private void AddBestNodeToClosedNodes(Node bestNode)
        {
            openNodes.Remove(bestNode);
            closedNodes.Add(bestNode);
        }

        private Node FindBestNode()
        {
            return openNodes.OrderBy(node => node.F).First();
        }

        private void CalculateMoveCostsToSurroundingNodes(Node currentNode)
        {
            List<Node> surroundingNodes = GetSurroundingNodes(currentNode).ToList();

            AddSurroundingNodesToOpenList(surroundingNodes);

            foreach (Node surroundingNode in surroundingNodes)
            {
                if (openNodes.Contains(surroundingNode))
                {
                    int moveCost = surroundingNode.Position.IsCoordinateDiagonal(currentNode.Position) ? DiagonalMoveCost : SimpleMoveCost;
                    int newMovement = moveCost + currentNode.MoveCost;

                    if (surroundingNode.IsNew || newMovement < surroundingNode.MoveCost)
                    {
                        Reparent(currentNode, surroundingNode);
                        surroundingNode.MoveCost = newMovement;
                    }
                }
            }
        }

        private static void Reparent(Node parent, Node child)
        {
            child.Parent = parent;
        }

        private IEnumerable<Node> GetSurroundingNodes(Node node)
        {
            return Nodes.Where(possibleSurroundingNode => possibleSurroundingNode.Position.IsCoordinateNextTo(node.Position));
        }

        private void AddSurroundingNodesToOpenList(IEnumerable<Node> surroundingNodes)
        {
            foreach (Node surroundingNode in surroundingNodes.Where(surroundingNode => !openNodes.Contains(surroundingNode)))
            {
                if (!closedNodes.Contains(surroundingNode))
                {
                    openNodes.Add(surroundingNode);
                }
            }
        }
    }
}