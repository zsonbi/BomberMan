using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MovingEntity
{
    [SerializeField]
    private MonsterType monsterType = MonsterType.Basic;

    public MonsterType Type { get => monsterType; private set => monsterType = value; }
    private MonsterBrain Brain;


    private void Update()
    {

        if (!DirectionPassable(CurrentDirection))
        {
            base.ChangeDir(Brain.ChangedCell());

        }
        base.Update();
    }


    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(entityType, gameBoard, CurrentPos);

        switch (Type)
        {
            case MonsterType.Basic:
                this.Brain = new BasicBrain();
                break;
            case MonsterType.Ghost:
                throw new System.NotImplementedException();
                break;
            case MonsterType.Smarty:
                throw new System.NotImplementedException();
                break;
            case MonsterType.Dumber:
                throw new System.NotImplementedException();
                break;
            default:
                break;
        }
        this.Brain.InitBrain(this,0.2f);
    }


    public override void ChangedCell(int boardRow, int boardCol)
    {
        base.ChangedCell(boardRow,boardCol);

        base.ChangeDir(Brain.ChangedCell());

    }
}