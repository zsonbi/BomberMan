using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    public class BFSCell
    {
        public BFSCell Parent { get; private set; } = null;

        public int Step { get; private set; } = -1;
        public int Row { get; private set; }

        public int Col { get; private set; }

        public void Visit(int step, BFSCell parent)
        {
            if (Step > -1)
            {
                throw new Exception("Can't visit an already visited node");
            }

            this.Step = step;
            this.Parent = parent;
        }

        public BFSCell(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public void Reset()
        {
            this.Step = -1;
            this.Parent = null;
        }
    }
}