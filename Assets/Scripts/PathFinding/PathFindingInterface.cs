using System.Collections.Generic;
using DataTypes;

namespace PathFinding
{
    /// <summary>
    /// Interface for the pathfindings
    /// </summary>
    public interface PathFindingInterface
    {
        /// <summary>
        /// Gets the path to the targets returns when it found one of the targets
        /// </summary>
        /// <param name="startingPos">The starting position of the algorithm</param>
        /// <param name="targets">The targets for the algorithm</param>
        /// <returns>The path to one of the target</returns>
        public Stack<BFSCell> GetPathToSearched(Position startingPos, IEnumerable<Position> targets);
    }
}