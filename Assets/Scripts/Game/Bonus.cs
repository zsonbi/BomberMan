using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MapEntity
{
    [SerializeField]
    public int Tier { get; private set; } = 0;

    [SerializeField]
    public BonusType Type { get; private set; }

    [SerializeField]
    public float Duration { get; private set; }

    [SerializeField]
    public bool Decaying { get; private set; } = false;

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(entityType, gameBoard, CurrentPos);
        throw new System.NotImplementedException();
    }
}