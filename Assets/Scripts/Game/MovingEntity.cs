using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MovingEntity : MapEntity
{
    [SerializeField]
    private float speed = 1f;

    public float Speed { get => speed; protected set => speed = value; }

    [SerializeField]
    public int Hp { get; protected set; } = 3;

    public bool Alive { get; protected set; } = true;
    public Direction CurrentDirection { get; private set; } = Direction.Left;
    protected Direction NewDirection = Direction.None;
    private Vector3? targetPos;
    private Vector3? startPos;
    protected float moveProgress { get; private set; }
    protected float timeToMove { get; private set; } = 1f;
    private float immuneTime = 0f;

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        this.Alive = true;
        base.Init(entityType, gameBoard, CurrentPos);
    }

    private void Awake()
    {
        this.timeToMove = 1 / Speed;
        targetPos = null;
    }

    protected void Update()
    {
        if (!Alive)
        {
            return;
        }

        if (immuneTime > 0f)
        {
            immuneTime -= Time.deltaTime;
        }

        Move(CurrentDirection);
    }


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

    public void Kill()
    {
        if (immuneTime > 0)
        {
            return;
        }
        else
        {
            if (Hp > 0)
            {
                Hp--;
                immuneTime = Config.IMMUNETIME;
            }
            else
            {

                this.Alive = false;
                Debug.Log("Unit died");
                this.gameObject.SetActive(false);
                
            }
        }

    }



    public bool DirectionPassable(Direction dir)
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
                return false;
        }

        if (obstacle.NotPassable || obstacle.Placed)
        {
            return false;
        }
        return true;
    }

    protected bool Move(Direction dir)
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

            if (DirectionPassable(CurrentDirection))
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

    protected void ChangeDir(Direction dir)
    {

        if (dir == CurrentDirection)
        {
            return;
        }
        NewDirection = dir;

    }

    public virtual void ChangedCell(int boardRow, int boardCol)
    {

        this.CurrentBoardPos.Change(boardRow, boardCol);
    }
}