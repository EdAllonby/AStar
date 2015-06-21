using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStar
{
    public sealed class AStarGrid
    {
        private const int DiagonalMoveCost = 14;
        private const int SimpleMoveCost = 10;
        private readonly List<Node> closedNodes = new List<Node>();
        private readonly List<Node> nodes;
        private readonly List<Node> openNodes = new List<Node>();

        public AStarGrid(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public Node EndNode { get; set; }
        public Node StartNode { get; set; }

        public IEnumerable<Node> Nodes
        {
            get { return nodes; }
        }

        public event EventHandler<IterationDetails> IterationComplete;

        public Task<IEnumerable<Node>> CalculatePathAsync(IHeuristicCalculator heuristicCalculator)
        {
            return Task.Factory.StartNew(() => CalculatePath(heuristicCalculator));
        }

        private IEnumerable<Node> CalculatePath(IHeuristicCalculator heuristicCalculator)
        {
            if (EndNode == null || StartNode == null)
            {
                return new List<Node>();
            }

            Reset();

            CalculateHeuristics(heuristicCalculator);

            NewIteration(StartNode);

            bool foundPath = false;

            while (!foundPath)
            {
                Node bestNode = FindBestNode();
                NewIteration(bestNode);

                var endNodeParent = IsClosedNodeNextToEndNode();

                if (endNodeParent != null)
                {
                    foundPath = true;
                    Reparent(endNodeParent, EndNode);
                }
            }

            return GetBestPath();
        }

        private void NewIteration(Node bestNode)
        {
            AddBestNodeToClosedNodes(bestNode);
            CalculateMoveCostsToSurroundingNodes(bestNode);

            if (IterationComplete != null)
            {
                IterationComplete(this, new IterationDetails(openNodes, closedNodes));
            }
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
            var surroundingNodes = GetSurroundingNodes(currentNode).Where(node => !node.IsWall);

            foreach (Node surroundingNode in surroundingNodes)
            {
                if (!openNodes.Contains(surroundingNode) && !closedNodes.Contains(surroundingNode))
                {
                    openNodes.Add(surroundingNode);
                }

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
    }
}