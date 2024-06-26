using DataTypes;

namespace Bomberman
{
    /// <summary>
    /// The ghost monster's brain
    /// </summary>
    public class GhostBrain : MonsterBrain
    {
        private bool prevWall = false;
        Obstacle obstacle;



        /// <summary>
        /// Gets it the ghost is currently facing towards an outer wall
        /// </summary>
        private bool DirectionTotallyImpassable()
        {
            if (body.GameBoard.Cells[body.CurrentBoardPos.Row, body.CurrentBoardPos.Col].HasBomb)
            {
                return true;
            }

            switch (body.CurrentDirection)
            {
                case Direction.Left:

                    return body.GameBoard.Cells[body.CurrentBoardPos.Row, body.CurrentBoardPos.Col - 1].HasBomb || body.CurrentBoardPos.Col <= 1;

                case Direction.Up:
                    return body.GameBoard.Cells[body.CurrentBoardPos.Row-1, body.CurrentBoardPos.Col ].HasBomb || body.CurrentBoardPos.Row <= 1;

                case Direction.Right:
                    return body.GameBoard.Cells[body.CurrentBoardPos.Row, body.CurrentBoardPos.Col + 1].HasBomb || body.CurrentBoardPos.Col >= body.GameBoard.ColCount - 2;

                case Direction.Down:
                    return body.GameBoard.Cells[body.CurrentBoardPos.Row+1, body.CurrentBoardPos.Col].HasBomb || body.CurrentBoardPos.Row >= body.GameBoard.RowCount - 2;

                default:
                    return false;
            }
        }

        /// <summary>
        /// What direction to move towards when changed cells
        /// </summary>
        /// <returns>A new direction to move towards</returns>
        public override Direction ChangedCell()
        {
            if (Accuracy < Config.RND.NextDouble())
            {
                return NextTargetDir();
            }
            else
            {
               
                if (!body.DirectionPassable(body.CurrentDirection))
                {
                    //Determine if the ghost should go through the wall
                    if (!DirectionTotallyImpassable() && (prevWall || Config.RND.NextDouble() <= Config.GHOSTPASSTHROUGHCHANCE))
                    {
                        prevWall = true;
                        
                        return body.CurrentDirection;
                    }

                    return NextTargetDir();
                }
                else
                {
                    prevWall = false;
                    return body.CurrentDirection;
                }
            }
        }
    }
}