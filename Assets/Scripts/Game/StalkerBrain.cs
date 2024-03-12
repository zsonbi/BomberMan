using PathFinding;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The stalker monster's brain
/// </summary>
public class StalkerBrain : MonsterBrain
{
    private PathFindingInterface pathFinder;

    /// <summary>
    /// Inits the brain
    /// </summary>
    /// <param name="body">The parent monster</param>
    /// <param name="accuracy">The accuracy of the brain</param>
    public override void InitBrain(Monster body, float accuracy = 0.9F)
    {
        base.InitBrain(body, accuracy);
        this.pathFinder = new BFS(body.GameBoard.Cells);
    }



    //Gets what direction to move to get to the nearest player
    private Direction NearestPlayerDir()
    {
        //Get the path to the player
        Stack<BFSCell> path = pathFinder.GetPathToSearched(this.body.CurrentBoardPos, this.body.GameBoard.Players.Where(x => x.Alive).Select(x => x.CurrentBoardPos));
        //If there is no path
        if (path is null || path.Count == 0)
        {
            return NextTargetDir();
        }
        else
        {
            //Get the direction
            BFSCell newTarget = path.Pop();
            if (newTarget.Row == this.body.CurrentBoardPos.Row)
            {
                if (newTarget.Col == this.body.CurrentBoardPos.Col + 1)
                {
                    return Direction.Right;
                }
                else
                {
                    return Direction.Left;
                }
            }
            else
            {
                if (newTarget.Row == this.body.CurrentBoardPos.Row + 1)
                {
                    return Direction.Down;
                }
                else
                {
                    return Direction.Up;
                }
            }
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
                if (Config.RND.NextDouble() < Config.STALKERMISSCHANCE)
                {
                    return NextTargetDir();

                }
                else
                {
                    return NearestPlayerDir();
                }
            }
            //Check if it is a juncktion if it is move towards the nearest player
            else
            {
                List<Direction> possDir = new List<Direction>();

                for (int i = 0; i < 4; i++)
                {
                    if (body.DirectionPassable((Direction)i))
                    {
                        possDir.Add((Direction)i);
                    }
                }
                if (possDir.Count == 0)
                {
                    return Direction.Left;
                }
                else if(possDir.Count == 1)
                {
                    return possDir.First();
                }
                else if (possDir.Contains(this.body.CurrentDirection) && possDir.Count==2)
                {
                    return body.CurrentDirection;
                }
                else
                {
                    return NearestPlayerDir();
                }
            }
        }
    }
}