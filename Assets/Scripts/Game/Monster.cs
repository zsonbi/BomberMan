using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MovingEntity
{
    public MonsterType Type { get; private set; } = MonsterType.Basic;
    private MonsterBrain Brain;

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        throw new System.NotImplementedException();
    }

    public override void ChangedCell()
    {
        base.ChangedCell();
    }
}