using System;
using UnityEngine;
using DataTypes;
using Persistance;

namespace Bomberman
{
    /// <summary>
    /// The monsters class which defines their behavior
    /// </summary>
    public class Monster : MovingEntity
    {
        [SerializeField]
        private MonsterType monsterType = MonsterType.Basic;

        /// <summary>
        /// The type of the monster
        /// </summary>
        public MonsterType Type { get => monsterType; private set => monsterType = value; }

        /// <summary>
        /// The type of the monster
        /// </summary>
        private MonsterBrain Brain;

        /// <summary>
        /// The type of the monster
        /// </summary>
        private new void Update()
        {
            if (!Alive || GameBoard.Paused)
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
                    this.Brain = new StalkerBrain();
                    break;

                default:
                    break;
            }
            this.Brain.InitBrain(this, 0.9f);
            ReachedTargetEvent = ReachedTarget;
            base.ChangeDir(Brain.ChangedCell());
        }

        /// <summary>
        /// Controls the movement of the monster
        /// </summary>
        protected override bool Move(Direction dir)
        {
            if (Type == MonsterType.Ghost)
            {
                Position possibleDir = Position.CreateCopyAndMoveDir(this.CurrentBoardPos, this.CurrentDirection);
                if (this.GameBoard.Cells[possibleDir.Row, possibleDir.Col].HasBomb)
                {
                    return false;
                }
            }

            return base.Move(dir);
        }

        /// <summary>
        /// Loads the monster
        /// </summary>
        /// <param name="monsterSave">The saved monster data</param>
        public void LoadMonster(MonsterSave monsterSave, GameBoard gameBoard)
        {
            this.Type = monsterSave.Type;
            this.Alive = monsterSave.Alive;

            this.CurrentDirection = monsterSave.CurrentDirection;

            Init(MapEntityType.Monster, gameBoard, monsterSave.CurrentBoardPos);
            //Kill it if it is already dead
            if (!monsterSave.Alive)
            {
                this.InstantKill();
            }
        }

        /// <summary>
        /// Event for when the monster reached the target, so it can change dir if needed
        /// </summary>
        private void ReachedTarget(object o, EventArgs args)
        {
            base.ChangeDir(Brain.ChangedCell());
        }
    }
}