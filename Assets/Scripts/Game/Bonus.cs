using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MapEntity
{
    [SerializeField]
    private int tier;
    [SerializeField]
    private BonusType type;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool decaying=false;

    public int Tier { get=>tier; private set=>tier=value; }

    public BonusType Type { get=>type; private set=>type=value; }

    public float Duration { get=>duration; private set=>duration=value; }

    public bool Decaying { get=>decaying; private set=>decaying=value; }

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(entityType, gameBoard, CurrentPos);
        throw new System.NotImplementedException();
    }
}