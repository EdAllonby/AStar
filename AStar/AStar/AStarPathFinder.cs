using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStar
{
    public class AStarPathFinder : IPathFinder, IIteratable<AStarPathFinderDetails>
    {
        private const int DiagonalMoveCost = 2;
        private const int SimpleMoveCost = 1;
        private readonly List<Node> closedNodes = new List<Node>();
        private readonly IHeuristicCalculator heuristicCalculator;
        private readonly List<Node> openNodes = new List<Node>();

        public AStarPathFinder(IHeuristicCalculator heuristicCalculator)
        {
            this.heuristicCalculator = heuristicCalculator;
        }

        public event EventHandler<AStarPathFinderDetails> IterationComplete;

        public Task<IEnumerable<Node>> FindBestPathAsync(Grid grid)
        {
            try
            {
                return Task.Factory.StartNew(() => FindBestPath(grid));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Node> FindBestPath(Grid grid)
        {
            if (grid.EndNode == null || grid.StartNode == null)
            {
                return new List<Node>();
            }

            Reset();

            CalculateNodeHeuristics(grid);
            NewIteration(grid, grid.StartNode);

            bool foundPath = false;

            while (!foundPath)
            {
                Node bestNode = FindBestNode();
                NewIteration(grid, bestNode);

                Node endNodeParent = IsClosedNodeNextToEndNode(grid.EndNode);

                if (endNodeParent != null)
                {
                    foundPath = true;
                    Reparent(endNodeParent, grid.EndNode);
                }
            }

            return GetBestPath(grid);
        }

        private void Reset()
        {
            openNodes.Clear();
            closedNodes.Clear();
        }

        private void NewIteration(Grid grid, Node bestNode)
        {
            AddBestNodeToClosedNodes(bestNode);

            CalculateMoveCostsToSurroundingNodes(grid, bestNode);

            if (IterationComplete != null)
            {
                IterationComplete(this, new AStarPathFinderDetails(openNodes, bestNode));
            }
        }

        private void CalculateNodeHeuristics(Grid grid)
        {
            foreach (Node currentNode in grid.Nodes)
            {
                heuristicCalculator.CalculateHeuristic(currentNode, grid.EndNode);
            }
        }

        private static IEnumerable<Node> GetBestPath(Grid grid)
        {
            List<Node> path = new List<Node> {grid.EndNode};

            bool startNodeFound = false;

            Node lastNode = grid.EndNode;

            while (!startNodeFound)
            {
                lastNode = lastNode.Parent;

                path.Add(lastNode);

                if (lastNode.Equals(grid.StartNode))
                {
                    startNodeFound = true;
                }
            }

            return path;
        }

        private Node IsClosedNodeNextToEndNode(Node endNode)
        {
            return closedNodes.FirstOrDefault(x => x.Position.IsCoordinateNextTo(endNode.Position));
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

        private void CalculateMoveCostsToSurroundingNodes(Grid grid, Node currentNode)
        {
            IEnumerable<Node> surroundingNodes = GetSurroundingNodes(grid, currentNode).Where(node => !node.IsWall);

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

        private static IEnumerable<Node> GetSurroundingNodes(Grid grid, Node node)
        {
            return grid.Nodes.Where(possibleSurroundingNode => possibleSurroundingNode.Position.IsCoordinateNextTo(node.Position));
        }
    }
}