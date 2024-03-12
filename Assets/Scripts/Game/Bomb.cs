using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MapEntity
{
    public int BlastRadius { get; private set; }
    public bool Placed { get; private set; }
    public float BombTimer { get; private set; }

    public float TimeTillBlow { get; private set; } = Config.BOMBBLOWTIME;

    // Update is called once per frame
    private void Update()
    {
        if(Placed)
        {
            BombTimer += Time.deltaTime;
            if(BombTimer>=TimeTillBlow)
            {
                BlowUp();
            }
        }
    }

    public void Place(Position whereToPlace)
    {
        this.Placed = true;
        this.gameObject.SetActive(true);
        this.CurrentBoardPos = whereToPlace;
        this.gameObject.transform.localPosition = new Vector3(whereToPlace.Col * Config.CELLSIZE, -2.5f - whereToPlace.Row * Config.CELLSIZE, 1);

        BombTimer = 0;
        
        //throw new System.NotImplementedException();
    }

    public void BlowUp()
    {
        Debug.Log("Bumm");
        this.gameObject.SetActive(false);
        this.Placed=false;
        //throw new System.NotImplementedException();
    }

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(entityType, gameBoard, CurrentPos);
        //throw new System.NotImplementedException();
    }
}