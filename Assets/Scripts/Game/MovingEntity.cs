using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingEntity : MapEntity
{
    [SerializeField]
    private float speed = 1f;

    public float Speed { get => speed; protected set => speed = value; }

    [SerializeField]
    public int Hp { get; protected set; }

    public bool Alive { get; protected set; }
    public Direction CurrentDirection { get; private set; } = Direction.Left;
    private bool goingTowardsWall = false;

    private void Awake()
    {
    }

    private void Update()
    {
        Move(CurrentDirection);
    }

    public void Kill()
    {
        this.Alive = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Collided with: " + collision.gameObject.name);
        }
    }

    protected bool DirectionPassable(Direction dir)
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
                break;
        }

        if (obstacle.NotPassable || obstacle.Placed)
        {
            return false;
        }
        return true;
    }

    protected bool Move(Direction dir)
    {
        if (goingTowardsWall)
        {
            return false;
        }

        //Cap if the game is lagging
        float moveAmount;
        if (Config.CELLSIZE / 5 < Time.deltaTime * speed)
        {
            moveAmount = Config.CELLSIZE / 5;
        }
        else
        {
            moveAmount = Time.deltaTime * speed;
        }
        float distanceToPositionCenter = Mathf.Sqrt(Mathf.Pow(this.transform.localPosition.x - this.CurrentBoardPos.Col * Config.CELLSIZE, 2) + Mathf.Pow(this.transform.localPosition.y - (this.CurrentBoardPos.Row * -Config.CELLSIZE - Config.CELLSIZE / 2), 2));

        Debug.Log("Dist" + distanceToPositionCenter);
        //Wall detection
        if (distanceToPositionCenter < Time.deltaTime * Speed)
        {
            if (!DirectionPassable(CurrentDirection))
            {
                this.transform.localPosition = new Vector3(this.CurrentBoardPos.Col * Config.CELLSIZE, this.CurrentBoardPos.Row * -Config.CELLSIZE - Config.CELLSIZE / 2, this.transform.localPosition.z);
                goingTowardsWall = true;
                return false;
            }
        }

        switch (dir)
        {
            case Direction.Left:
                this.transform.position = new Vector3(this.transform.position.x - moveAmount, this.transform.position.y, this.transform.position.z);

                break;

            case Direction.Up:
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + moveAmount, this.transform.position.z);

                break;

            case Direction.Right:
                this.transform.position = new Vector3(this.transform.position.x + moveAmount, this.transform.position.y, this.transform.position.z);

                break;

            case Direction.Down:
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - moveAmount, this.transform.position.z);

                break;

            default:
                break;
        }

        float boardPosX = Mathf.Round(this.transform.localPosition.x / Config.CELLSIZE);
        float boardPosY = Mathf.Round((this.transform.localPosition.y + Config.CELLSIZE / 2) / -Config.CELLSIZE);

        if (boardPosX != this.CurrentBoardPos.Col || boardPosY != this.CurrentBoardPos.Row)
        {
            this.CurrentBoardPos.Change((int)boardPosY, (int)boardPosX);
            //   Debug.Log($"{this.gameObject.name} changed to cell Row:{this.CurrentBoardPos.Row} Col:{this.CurrentBoardPos.Col}");
        }
        return transform;
    }

    protected void ChangeDir(Direction dir)
    {
        if (DirectionPassable(dir))
        {
            float distanceToPositionCenter = Mathf.Sqrt(Mathf.Pow(this.transform.localPosition.x - this.CurrentBoardPos.Col * Config.CELLSIZE, 2) + Mathf.Pow(this.transform.localPosition.y - (this.CurrentBoardPos.Row * -Config.CELLSIZE - Config.CELLSIZE / 2), 2));

            Debug.Log("Dist" + distanceToPositionCenter);
            //Wall detection
            if (distanceToPositionCenter < Time.deltaTime * Speed)
            {
                goingTowardsWall = false;
                this.transform.localPosition = new Vector3(this.CurrentBoardPos.Col * Config.CELLSIZE, this.CurrentBoardPos.Row * -Config.CELLSIZE - Config.CELLSIZE / 2, this.transform.localPosition.z);

                this.CurrentDirection = dir;
            }
        }
    }

    public virtual void ChangedCell()
    {
        float boardPosX = Mathf.Round(this.transform.localPosition.x / Config.CELLSIZE);
        float boardPosY = Mathf.Round((this.transform.localPosition.y + Config.CELLSIZE / 2) / -Config.CELLSIZE);

        this.CurrentBoardPos.Change((int)boardPosY, (int)boardPosX);
    }
}