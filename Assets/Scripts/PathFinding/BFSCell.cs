using System;

namespace PathFinding
{
    /// <summary>
    /// A single cell of the BFS algorithm
    /// </summary>
    public class BFSCell
    {

        /// <summary>
        /// Parent cell of the cell
        /// </summary>
        public BFSCell Parent { get; private set; } = null;

        /// <summary>
        /// How many steps did it take to reach this cell
        /// </summary>
        public int Step { get; private set; } = -1;
        /// <summary>
        /// The row index of the cell
        /// </summary>
        public int Row { get; private set; }
        /// <summary>
        /// The col index of the cell
        /// </summary>
        public int Col { get; private set; }

        /// <summary>
        /// Visit the cell
        /// </summary>
        /// <param name="step">How many steps does the visit take</param>
        /// <param name="parent">The parent of the cell</param>
        /// <exception cref="Exception">Can't visit an already visited node</exception>
        public void Visit(int step, BFSCell parent)
        {
            if (Step > -1)
            {
                throw new Exception("Can't visit an already visited node");
            }

            this.Step = step;
            this.Parent = parent;
        }

        /// <summary>
        /// Creates a new cell
        /// </summary>
        /// <param name="row">The row of the cell</param>
        /// <param name="col">The col of the cell</param>
        public BFSCell(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        /// <summary>
        /// Resets the cell's parent and step to base state
        /// </summary>
        public void Reset()
        {
            this.Step = -1;
            this.Parent = null;
        }
    }
}