using PathFinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SmartyBrain : MonsterBrain
{
    private PathFindingInterface pathFinder;

    public override void InitBrain(Monster body, float accuracy = 0.9F)
    {
        base.InitBrain(body, accuracy);
        this.pathFinder = new BFS(body.GameBoard.Cells);
    }

    public override Direction NextTargetDir()
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
            return (Direction)((byte)(body.CurrentDirection + 2) % 4);
        }

        return possDir[Config.RND.Next(0, possDir.Count)];
    }

    private Direction NearestPlayerDir()
    {
        Stack<BFSCell> path =pathFinder.GetPathToSearched(this.body.CurrentBoardPos, this.body.GameBoard.Players.Select(x => x.CurrentBoardPos));
        if (path is null || path.Count == 0)
        {
            return NextTargetDir();
        }
        else
        {
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
                return NearestPlayerDir();
            }
            else
            {
                return body.CurrentDirection;
            }
        }
    }
}