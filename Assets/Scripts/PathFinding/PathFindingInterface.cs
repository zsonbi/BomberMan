using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathFinding
{
    public interface PathFindingInterface
    {
        public Task<Stack<BFSCell>> GetPathToSearched(Position startingPos, IEnumerable<Position> targets);
    }
}