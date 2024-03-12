using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MovingEntity
{
    [SerializeField]
    private MonsterType monsterType = MonsterType.Basic;

    /// <summary>
    /// The type of the monster
    /// </summary>
    public MonsterType Type { get => monsterType; private set => monsterType = value; }
    //The monster's brain for the pathfinding
    private MonsterBrain Brain;

    //Called on every frame
    private new void Update()
    {
        if (!Alive)
        {
            return;
        }
        //If it is enclosed from all 4 sides
        if (timeToMove * 2 < moveProgress && !DirectionPassable(CurrentDirection))
        {
            base.ChangeDir(Brain.ChangedCell());
        }
        base.Update();
    }

    /// <summary>
    /// Inits the entity should be called right after creating the entity
    /// </summary>
    /// <param name="entityType">The type of the entity</param>
    /// <param name="gameBoard">The parent board</param>
    /// <param name="CurrentPos">The current position of the entity</param>
    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(entityType, gameBoard, CurrentPos);

        //Inits the appropiate brain for the monster
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

    /// <summary>
    /// Event for when the monster reached the target, so it can change dir if needed
    /// </summary>
    private void ReachedTarget(object o, EventArgs args)
    {
        base.ChangeDir(Brain.ChangedCell());
    }
}