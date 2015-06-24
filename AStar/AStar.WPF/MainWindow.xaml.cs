﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AStar.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int GridSize = 35;
        private const int NodeSize = 40;

        private readonly List<NodeControl> nodeControls = new List<NodeControl>();

        public MainWindow()
        {
            InitializeComponent();

            SetupGrid();
        }

        private void SetupGrid()
        {
            NodeGrid.Children.Clear();

            for (int stackPanelRow = 0; stackPanelRow < GridSize; stackPanelRow++)
            {
                RowDefinition rowDefinition = new RowDefinition
                {
                    Height = new GridLength()
                };

                NodeGrid.RowDefinitions.Add(rowDefinition);
                StackPanel stackPanel = new StackPanel {Orientation = Orientation.Horizontal};

                for (int nodeColumn = 0; nodeColumn < GridSize; nodeColumn++)
                {
                    NodeControl nodeControl = new NodeControl(nodeColumn, stackPanelRow) { Height = NodeSize, Width = NodeSize };

                    nodeControls.Add(nodeControl);

                    stackPanel.Children.Add(nodeControl);
                }

                stackPanel.SetValue(Grid.RowProperty, stackPanelRow);

                NodeGrid.Children.Add(stackPanel);
            }
        }

        private async void StartSearch(object sender, RoutedEventArgs e)
        {
            List<Node> nodes = new List<Node>();
            Node startNode = null;
            Node endNode = null;

            foreach (NodeControl nodeControl in nodeControls)
            {
                nodes.Add(nodeControl.Node);

                switch (nodeControl.NodeType)
                {
                    case NodeType.StartNode:
                        startNode = nodeControl.Node;
                        break;
                    case NodeType.EndNode:
                        endNode = nodeControl.Node;
                        break;
                }
            }

            AStarGrid grid = new AStarGrid(nodes) {StartNode = startNode, EndNode = endNode};

            grid.IterationComplete += PathFindIterationComplete;

            Task<IEnumerable<Node>> bestPathTask = grid.CalculatePathAsync(new ManhattanCalculator());

            foreach (NodeControl nodeControl in nodeControls)
            {
                nodeControl.Heuristic.Content = nodeControl.Node.Heuristic.ToString();
            }

            IEnumerable<Node> bestPath = await bestPathTask;
            
            foreach (NodeControl nodeInPath in bestPath.Select(node => nodeControls.First(x => x.Node.Equals(node))))
            {
                if (nodeInPath.NodeType != NodeType.StartNode && nodeInPath.NodeType != NodeType.EndNode)
                {
                    nodeInPath.NodeType = NodeType.Path;
                }
            }
        }

        private void PathFindIterationComplete(object sender, IterationDetails e)
        {
            foreach (Node openNode in e.OpenNodes)
            {
                NodeControl node = nodeControls.First(x => x.Node.Equals(openNode));
                Application.Current.Dispatcher.Invoke(() => node.NodeType = NodeType.OpenNode);
            }

            foreach (Node openNode in e.ClosedNodes)
            {
                NodeControl node = nodeControls.First(x => x.Node.Equals(openNode));
                Application.Current.Dispatcher.Invoke(() => node.NodeType = NodeType.ClosedNode);
            }
        }

        private void ClearGrid(object sender, RoutedEventArgs e)
        {
            foreach (NodeControl nodeControl in nodeControls)
            {
                nodeControl.Reset();
            }
        }
    }
}