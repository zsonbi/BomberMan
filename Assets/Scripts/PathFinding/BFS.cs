using System.Collections.Generic;


namespace PathFinding
{
    /// <summary>
    /// Algoritm for bfs pathfinding
    /// </summary>
    public class BFS : PathFindingInterface
    {
        //Reference to the Gameboard's cell
        private Obstacle[,] Cells;
        //Grid for the bfs algorithm
        private BFSCell[,] SearchGrid;

        //Creates the search grid does not create cells for the impassable walls
        private void CreateSearchGrid()
        {
            SearchGrid = new BFSCell[Cells.GetLength(0), Cells.GetLength(1)];
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    if (!Cells[i, j].NotPassable)
                        SearchGrid[i, j] = new BFSCell(i, j);
                }
            }
        }

        /// <summary>
        /// Creates a new BFS class
        /// </summary>
        /// <param name="Cells">The gameboard's cells 2d array</param>
        public BFS(Obstacle[,] Cells)
        {
            this.Cells = Cells;
            CreateSearchGrid();
        }
        
        /// <summary>
        /// Resets the BFS search grid table
        /// </summary>
        private void ResetGrid()
        {
            for (int i = 0; i < SearchGrid.GetLength(0); i++)
            {
                for (int j = 0; j < SearchGrid.GetLength(1); j++)
                {
                    if (SearchGrid[i, j] is not null)
                    {
                        SearchGrid[i, j].Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the path to the targets returns when it found one of the targets
        /// </summary>
        /// <param name="startingPos">The starting position of the algorithm</param>
        /// <param name="targets">The targets for the algorithm</param>
        /// <returns>The path to one of the target</returns>
        public Stack<BFSCell> GetPathToSearched(Position startingPos, IEnumerable<Position> targets)
        {
            //If the starting position is not valid return
            if (SearchGrid[startingPos.Row, startingPos.Col] is null)
            {
                return null;
            }

            //Reset the search grid
            ResetGrid();

            Queue<BFSCell> whatToCheck = new Queue<BFSCell>();
            SearchGrid[startingPos.Row, startingPos.Col].Visit(0, null);
            whatToCheck.Enqueue(SearchGrid[startingPos.Row, startingPos.Col]);

            //While the bfs still got nodes to check
            while (whatToCheck.Count != 0)
            {

                BFSCell currNode = whatToCheck.Dequeue();
                //Check if we reached target
                foreach (var target in targets)
                {
                    if (currNode.Row == target.Row && currNode.Col == target.Col)
                    {
                        Stack<BFSCell> path = new Stack<BFSCell>();
                        //Put the path into the path stack
                        while (currNode.Parent is not null)
                        {
                            path.Push(currNode);
                            currNode = currNode.Parent;
                        }

                        return path;
                    }
                    //Check the neighbouring cells if it is a new cell put it in the queue
                    BFSCell temp = null;
                    if (currNode.Row > 0)
                    {
                        temp = SearchGrid[currNode.Row - 1, currNode.Col];
                        if (temp is not null && !Cells[temp.Row, temp.Col].Placed && temp.Step == -1)
                        {
                            temp.Visit(currNode.Step + 1, currNode);
                            whatToCheck.Enqueue(temp);
                        }
                    }
                    if (currNode.Col > 0)
                    {
                        temp = SearchGrid[currNode.Row, currNode.Col - 1];
                        if (temp is not null && !Cells[temp.Row, temp.Col].Placed && temp.Step == -1)
                        {
                            temp.Visit(currNode.Step + 1, currNode);
                            whatToCheck.Enqueue(temp);
                        }
                    }
                    if (currNode.Row < Cells.GetLength(0) - 1)
                    {
                        temp = SearchGrid[currNode.Row + 1, currNode.Col];
                        if (temp is not null && !Cells[temp.Row, temp.Col].Placed && temp.Step == -1)
                        {
                            temp.Visit(currNode.Step + 1, currNode);
                            whatToCheck.Enqueue(temp);
                        }
                    }
                    if (currNode.Col < Cells.GetLength(1) - 1)
                    {
                        temp = SearchGrid[currNode.Row, currNode.Col + 1];
                        if (temp is not null && !Cells[temp.Row, temp.Col].Placed && temp.Step == -1)
                        {
                            temp.Visit(currNode.Step + 1, currNode);
                            whatToCheck.Enqueue(temp);
                        }
                    }
                }
            }

            return null;
        }
    }
}