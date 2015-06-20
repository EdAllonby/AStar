using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void CalculateHeuristics_WhenANodeIsTwoNodesLeftOfTarget_UsingManhattan()
        {
            Node endNode = new Node(new Coordinate(3, 2));
            Node startNode = new Node(new Coordinate(1, 2));

            List<Node> nodes = new List<Node>
            {
                new Node(new Coordinate(1, 1)),
                startNode,
                new Node(new Coordinate(2, 1)),
                new Node(new Coordinate(2, 2)),
                new Node(new Coordinate(3, 1)),
                endNode
            };
            Grid grid = new Grid(nodes) { EndNode = endNode };
            grid.CalculateHeuristics(new ManhattanCalculator());

            Assert.AreEqual(startNode.Heuristic, 2);
        }

        [Test]
        public void CalculateHeuristics_EndNodeShouldHaveAHeuristicOf0_UsingManhattan()
        {
            Node endNode = new Node(new Coordinate(2, 2));

            List<Node> nodes = new List<Node>
            {
                new Node(new Coordinate(1, 1)),
                new Node(new Coordinate(1, 2)),
                new Node(new Coordinate(2, 1)),
                endNode
            };

            Grid grid = new Grid(nodes) { EndNode = endNode };

            grid.CalculateHeuristics(new ManhattanCalculator());

            Assert.AreEqual(endNode.Heuristic, 0);
        }

        [Test]
        public void CalculateHeuristics_WithA2By2Grid_UsingManhattan()
        {
            Node endNode = new Node(new Coordinate(2, 2));

            List<Node> nodes = new List<Node>
            {
                new Node(new Coordinate(1, 1)),
                new Node(new Coordinate(1, 2)),
                new Node(new Coordinate(2, 1)),
                endNode
            };

            Grid grid = new Grid(nodes) {EndNode = endNode};

            grid.CalculateHeuristics(new ManhattanCalculator());

            foreach (Node node in grid.Nodes)
            {
                int xDifference = Math.Abs(node.Position.X - endNode.Position.X);
                int yDifference = Math.Abs(node.Position.Y - endNode.Position.Y);

                Assert.AreEqual(node.Heuristic, xDifference + yDifference);
            }
        }
    }
}