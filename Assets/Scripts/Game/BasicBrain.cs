using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBrain : MonsterBrain
{
    public override Direction NextTargetDir()
    {
        return Direction.Right;

    }


}
