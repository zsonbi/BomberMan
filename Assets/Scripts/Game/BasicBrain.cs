using System.Collections;
using System.Collections.Generic;


public class BasicBrain : MonsterBrain
{
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
            return Direction.None;
        }

        return possDir[Config.RND.Next(0,possDir.Count)];
    }

    public override Direction ChangedCell()
    {
        if(Accuracy < Config.RND.NextDouble())
        {
            return NextTargetDir();
        }
        else
        {
            if (!body.DirectionPassable(body.CurrentDirection))
            {
                return NextTargetDir();
            }
            else
            {
                return body.CurrentDirection;
            }
        }
    }

}
