using System.Windows.Input;
using System.Windows.Media;

namespace AStar.WPF
{
    /// <summary>
    /// Interaction logic for NodeControl.xaml
    /// </summary>
    public partial class NodeControl
    {
        private NodeType type = NodeType.Empty;

        public NodeControl(int column, int row)
        {
            Node = new Node(new Coordinate(column, row));

            InitializeComponent();
        }

        public Node Node { get; private set; }

        public NodeType NodeType
        {
            get { return type; }
            set
            {
                type = value;
                UpdateView();
            }
        }

        private void NodeControl_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeType();
        }

        private void ChangeType()
        {
            type = NodeType.Empty;

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                type = NodeType.StartNode;
            }
            else if (Keyboard.IsKeyDown(Key.RightCtrl))
            {
                type = NodeType.EndNode;
            }

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                type = NodeType.Wall;
                Node.IsWall = true;
            }
            else
            {
                Node.IsWall = false;
            }

            UpdateView();
        }

        private void UpdateView()
        {
            switch (type)
            {
                case NodeType.StartNode:
                    NodeControlGrid.Background = Brushes.Orange;
                    break;
                case NodeType.EndNode:
                    NodeControlGrid.Background = Brushes.Orchid;
                    break;
                case NodeType.Empty:
                    NodeControlGrid.Background = Brushes.Transparent;
                    break;
                case NodeType.Path:
                    NodeControlGrid.Background = Brushes.Aqua;
                    break;
                case NodeType.Wall:
                    NodeControlGrid.Background = Brushes.Red;
                    break;
            }
        }

        public void Reset()
        {
            Node.Reset();
            NodeType = NodeType.Empty;
        }
    }
}