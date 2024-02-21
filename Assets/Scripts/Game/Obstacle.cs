using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MapEntity
{
    public bool Placed { get; private set; } =false;

    [SerializeField]
    private bool destructible;

    [SerializeField]
    private bool notPassable;

    public bool Destructible { get => destructible; private set => destructible = value; }

    public Bonus ContainingBonus { get; private set; }

    public bool NotPassable { get; private set; } = false;

    private void DropBonus()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
       base.Init(entityType, gameBoard, CurrentPos);

    }

    public bool Place(bool containBonus)
    {
        if (this.Placed)
        {
            return false;
        }
        this.Placed=true;
        return true;
    }

    public bool BlowUp(bool dropBonus = true)
    {
        if (!this.Placed)
        {
            return false;
        }


        if (dropBonus)
        {
            DropBonus();
        }

        this.Placed = false;
        return true;
    }


}