using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MovingEntity
{
    [SerializeField]
    private MonsterType monsterType = MonsterType.Basic;

    public MonsterType Type { get => monsterType; private set => monsterType = value; }
    private MonsterBrain Brain;

    private new void Update()
    {
        if (!Alive)
        {
            return;
        }
        if (timeToMove * 2 < moveProgress && !DirectionPassable(CurrentDirection))
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
                this.Brain = new GhostBrain();

                break;

            case MonsterType.Smarty:
                this.Brain = new SmartyBrain();
                break;

            case MonsterType.Stalker:
                this.Brain=new StalkerBrain();
                break;

            default:
                break;
        }
        this.Brain.InitBrain(this, 0.9f);
        ReachedTargetEvent = ReachedTarget;
        base.ChangeDir(Brain.ChangedCell());
    }

    private void ReachedTarget(object o, EventArgs args)
    {
        base.ChangeDir(Brain.ChangedCell());
    }
}