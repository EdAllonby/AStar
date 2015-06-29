using System.Collections.Generic;
using System.Threading.Tasks;

namespace AStar
{
    public interface IPathFinder
    {
        Task<IEnumerable<Node>> FindBestPathAsync(Grid grid);
        IEnumerable<Node> FindBestPath(Grid grid);
    }
}