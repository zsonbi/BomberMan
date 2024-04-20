using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using DataTypes;

namespace Bomberman
{
    public abstract class MovingEntity : MapEntity
    {
        [SerializeField]
        private float speed = 1f;

        [SerializeField]
        private int hp = 3;

        //Where will the entity move
        private Vector3? targetPos;

        //Where to start the calculation from for the movement
        private Vector3? startPos;

        //Immunity counter if it been killed
        public float ImmuneTime { get; protected set; } = 0f;

        /// <summary>
        /// Speed of the entity (how fast it will move on the board)
        /// </summary>
        public float Speed { get; protected set; }

        /// <summary>
        /// How many times can the entity be "killed"
        /// </summary>
        public int Hp { get; protected set; }

        /// <summary>
        /// Is the entity alive
        /// </summary>
        public bool Alive { get; protected set; } = true;

        /// <summary>
        /// The current moving direction of the entity
        /// </summary>
        public Direction CurrentDirection { get; private set; } = Direction.Left;

        //What will be the entity's new moving direction after it changed tiles
        protected Direction NewDirection = Direction.None;

        /// <summary>
        /// How is the move progressing (0.0-1.0)
        /// </summary>
        protected float moveProgress { get; private set; }

        /// <summary>
        /// How long does the movement take
        /// </summary>
        public float timeToMove { get; protected set; } = 1f;

        /// <summary>
        /// Event which is called when it changed tiles completely
        /// </summary>
        public EventHandler ReachedTargetEvent;

        private bool ghost = false;

        /// <summary>
        /// Inits the entity should be called right after creating the entity
        /// </summary>
        /// <param name="entityType">The type of the entity</param>
        /// <param name="gameBoard">The parent board</param>
        /// <param name="CurrentPos">The current position of the entity</param>
        public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
        {
            base.Init(entityType, gameBoard, CurrentPos);
            this.Alive = true;
            this.Hp = hp;
            this.Speed = speed;
            this.targetPos = null;
            this.startPos = null;
            CurrentDirection = Direction.Left;
            NewDirection = Direction.None;
            moveProgress = 0f;
            this.timeToMove = 1f / this.Speed;
        }

        //Called every frame update
        protected void Update()
        {
            if (GameBoard.Paused)
            {
                return;
            }

            if (!Alive)
            {
                return;
            }

            if (ImmuneTime > 0f)
            {
                ImmuneTime -= Time.deltaTime;
            }

            Move(CurrentDirection);
        }

        //Get the next target determined by the direction
        private Vector3 GetNextTarget(Direction dir)
        {
            Obstacle obstacle;
            switch (dir)
            {
                case Direction.Left:
                    obstacle = GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col - 1];
                    break;

                case Direction.Up:
                    obstacle = GameBoard.Cells[CurrentBoardPos.Row - 1, CurrentBoardPos.Col];
                    break;

                case Direction.Right:
                    obstacle = GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col + 1];
                    break;

                case Direction.Down:
                    obstacle = GameBoard.Cells[CurrentBoardPos.Row + 1, CurrentBoardPos.Col];
                    break;

                default:
                    Debug.LogError("Can't get obstacle in Move function");
                    throw new Exception("Invalid direction in GetNextTarget()");
            }

            return new Vector3(obstacle.CurrentBoardPos.Col * Config.CELLSIZE, obstacle.CurrentBoardPos.Row * -Config.CELLSIZE - Config.CELLSIZE / 2, this.transform.localPosition.z);
        }

        /// <summary>
        /// Kills the entity if it isn't immune and have 0 Health kill it
        /// </summary>
        public virtual bool Kill()
        {
            if (ImmuneTime > 0)
            {
                return false;
            }
            else
            {
                Debug.Log("Got dmg");
                if (Hp > 0)
                {
                    Hp--;
                    ImmuneTime = Config.IMMUNETIME;
                }
                else
                {
                    this.Alive = false;
                    Debug.Log("Unit died");
                    this.gameObject.SetActive(false);
                }
                return true;
            }
        }

        /// <summary>
        /// Instantly kill the entity it's health and immuneTime doesn't matter
        /// </summary>
        public void InstantKill()
        {
            this.Hp = 0;
            ImmuneTime = 0f;
            this.Kill();
        }

        /// <summary>
        /// Checks if the given direction is passable for the entity
        /// </summary>
        /// <param name="dir">Which direction to check</param>
        /// <returns>true-passable false-impassable</returns>
        public bool DirectionPassable(Direction dir)
        {
            if (ghost)
            {
                bool edge = false;
                switch (dir)
                {
                    case Direction.Left:
                        edge = CurrentBoardPos.Col <= 1;
                        break;

                    case Direction.Up:
                        edge = CurrentBoardPos.Row <= 1;
                        break;

                    case Direction.Right:
                        edge = CurrentBoardPos.Col >= GameBoard.ColCount - 2;
                        break;

                    case Direction.Down:
                        edge = CurrentBoardPos.Row >= GameBoard.RowCount - 2;
                        break;

                    default:
                        return false;
                }
                return !edge;
            }
            Obstacle obstacle;

            switch (dir)
            {
                case Direction.Left:
                    obstacle = GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col - 1];
                    break;

                case Direction.Up:
                    obstacle = GameBoard.Cells[CurrentBoardPos.Row - 1, CurrentBoardPos.Col];
                    break;

                case Direction.Right:
                    obstacle = GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col + 1];
                    break;

                case Direction.Down:
                    obstacle = GameBoard.Cells[CurrentBoardPos.Row + 1, CurrentBoardPos.Col];
                    break;

                default:
                    Debug.LogError("Can't get obstacle in Move function");
                    return false;
            }

            if (obstacle.NotPassable || obstacle.Placed)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// What Moves the entity at the direction
        /// </summary>
        /// <param name="dir">The direction to move the entity</param>
        /// <returns>true-if it could move, false-if it failed</returns>
        protected virtual bool Move(Direction dir)
        {
            moveProgress += Time.deltaTime;
            if (targetPos is not null)
            {
                if (NewDirection != Direction.None && (byte)dir % 2 == (byte)NewDirection % 2)
                {
                    Vector3? temp = (Vector3)startPos;
                    startPos = targetPos;
                    targetPos = temp;

                    this.CurrentDirection = NewDirection;

                    NewDirection = Direction.None;

                    moveProgress = timeToMove - moveProgress;
                }
                if (moveProgress >= timeToMove)
                {
                    this.transform.localPosition = (Vector3)targetPos;
                    ReachedTargetEvent?.Invoke(this, EventArgs.Empty);
                    targetPos = null;
                }
            }
            //Not else because the previous if may modify the targetPos
            if (targetPos is null)
            {
                this.startPos = this.gameObject.transform.localPosition;
                if (NewDirection != Direction.None)
                {
                    this.CurrentDirection = NewDirection;

                    NewDirection = Direction.None;
                }

                if (DirectionPassable(CurrentDirection) || (this.EntityType == MapEntityType.Monster && ((Monster)(this)).Type == MonsterType.Ghost))
                {
                    targetPos = GetNextTarget(CurrentDirection);
                    moveProgress = 0f;
                }
                else
                {
                    return false;
                }
            }

            if (targetPos is not null && startPos is not null)
            {
                this.transform.localPosition = Vector3.MoveTowards((Vector3)startPos, (Vector3)targetPos, (moveProgress / timeToMove) * Config.CELLSIZE);
            }
            else
            {
                Debug.LogError("No starting position or targer position in movement");
            }

            //Cell position update
            float boardPosX = Mathf.Round(this.transform.localPosition.x / Config.CELLSIZE);
            float boardPosY = Mathf.Round((this.transform.localPosition.y + Config.CELLSIZE / 2) / -Config.CELLSIZE);

            if (boardPosX != this.CurrentBoardPos.Col || boardPosY != this.CurrentBoardPos.Row)
            {
                ChangedCell((int)boardPosY, (int)boardPosX);
            }
            return true;
        }

        /// <summary>
        /// Changes the direction of the entity
        /// </summary>
        /// <param name="dir">The dir to change</param>
        protected void ChangeDir(Direction dir)
        {
            if (dir == CurrentDirection)
            {
                return;
            }
            NewDirection = dir;
        }

        /// <summary>
        /// Called when an entity changed it's cell on the board
        /// </summary>
        /// <param name="boardRow">New row index</param>
        /// <param name="boardCol">New col index</param>
        public virtual void ChangedCell(int boardRow, int boardCol)
        {
            this.CurrentBoardPos.Change(boardRow, boardCol);
        }

        public void SetGhost(bool newValue)
        {
            ghost = newValue;
        }
    }
}