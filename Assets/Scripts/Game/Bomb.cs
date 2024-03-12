using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MapEntity
{
    public int BlastRadius { get; private set; }
    public bool Placed { get; private set; }
    public float TimeToBlow { get; private set; }

    public float TimeTillBlow { get; private set; }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Place(Position whereToPlace)
    {
        throw new System.NotImplementedException();
    }

    public void BlowUp()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(MapEntityType.Bomb, gameBoard, CurrentPos);
        throw new System.NotImplementedException();
    }
}