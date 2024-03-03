using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinding
{
    public interface PathFindingInterface
    {
        public Stack<BFSCell> GetPathToSearched(Position startingPos, IEnumerable<Position> targets);
    }
}