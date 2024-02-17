using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MapEntity
{
    public bool Placed { get; private set; }

    [SerializeField]
    public bool Destructible { get; private set; }

    public Bonus ContainingBonus { get; private set; }

    [SerializeField]
    public bool NotPassable { get; private set; } = false;

    private void DropBonus()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        throw new System.NotImplementedException();
    }

    public void BlowUp()
    {
        throw new System.NotImplementedException();
    }
}