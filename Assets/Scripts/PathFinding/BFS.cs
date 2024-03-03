using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace PathFinding
{
    public class BFS
    {
        private Obstacle[,] Cells;
        private BFSCell[,] SearchGrid;

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

        public BFS(Obstacle[,] Cells)
        {
            this.Cells = Cells;
        }

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

        public Stack<BFSCell> GetPathToSearched(Position startingPos, List<Position> targets)
        {
            if (SearchGrid[startingPos.Row, startingPos.Col] is null)
            {
                return null;
            }

            ResetGrid();

            Queue<BFSCell> whatToCheck = new Queue<BFSCell>();
            SearchGrid[startingPos.Row, startingPos.Col].Visit(0, null);
            whatToCheck.Enqueue(SearchGrid[startingPos.Row, startingPos.Col]);

            while (whatToCheck.Count != 0)
            {
                BFSCell currNode = whatToCheck.Dequeue();
                foreach (var target in targets)
                {
                    if (currNode.Row == target.Row && currNode.Col == target.Col)
                    {
                        Stack<BFSCell> path = new Stack<BFSCell>();
                        while (currNode.Parent is not null)
                        {
                            path.Push(currNode);
                        }

                        return path;
                    }
                    BFSCell temp = null;
                    if (currNode.Row > 0)
                    {
                        temp = SearchGrid[currNode.Row - 1, currNode.Col];
                        if (temp is not null && !Cells[temp.Row, temp.Col].Placed && temp.Step == -1)
                        {
                            temp.Visit(currNode.Step, currNode);
                            whatToCheck.Enqueue(temp);
                        }
                    }
                    if (currNode.Col > 0)
                    {
                        temp = SearchGrid[currNode.Row, currNode.Col - 1];
                        if (temp is not null && !Cells[temp.Row, temp.Col].Placed && temp.Step == -1)
                        {
                            temp.Visit(currNode.Step, currNode);
                            whatToCheck.Enqueue(temp);
                        }
                    }
                    if (currNode.Row > Cells.GetLength(0) - 1)
                    {
                        temp = SearchGrid[currNode.Row + 1, currNode.Col];
                        if (temp is not null && !Cells[temp.Row, temp.Col].Placed && temp.Step == -1)
                        {
                            temp.Visit(currNode.Step, currNode);
                            whatToCheck.Enqueue(temp);
                        }
                    }
                    if (currNode.Col < Cells.GetLength(1) - 1)
                    {
                        temp = SearchGrid[currNode.Row, currNode.Col + 1];
                        if (temp is not null && !Cells[temp.Row, temp.Col].Placed && temp.Step == -1)
                        {
                            temp.Visit(currNode.Step, currNode);
                            whatToCheck.Enqueue(temp);
                        }
                    }
                }
            }

            return null;
        }
    }
}