using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public class GhostBrain : MonsterBrain
{
    private bool prevWall = false;

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

    private bool DirectionBorder()
    {
        switch (body.CurrentDirection)
        {
            case Direction.Left:
                return body.CurrentBoardPos.Col <= 1;

            case Direction.Up:
                return body.CurrentBoardPos.Row <= 1;

            case Direction.Right:
                return body.CurrentBoardPos.Col >= body.GameBoard.ColCount - 2;

            case Direction.Down:
                return body.CurrentBoardPos.Row >= body.GameBoard.RowCount - 2;

            default:
                return false;
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
                if (!DirectionBorder() && (prevWall || Config.RND.NextDouble() <= Config.GHOSTPASSTHROUGHCHANCE))
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