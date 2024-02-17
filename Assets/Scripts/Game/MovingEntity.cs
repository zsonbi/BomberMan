using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingEntity : MapEntity
{
    [SerializeField]
    public float Speed { get; protected set; }

    [SerializeField]
    public int Hp { get; protected set; }

    public bool Alive { get; protected set; }
    public Direction CurrentDirection { get; private set; } = Direction.Left;
    public Position PrevBoardPos { get; protected set; }

    private void Update()
    {
        Move(CurrentDirection);
    }

    public void Kill()
    {
        this.Alive = false;
    }

    protected void Move(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                this.transform.position = new Vector3(this.transform.position.x - Time.deltaTime, this.transform.position.y, this.transform.position.z);

                break;

            case Direction.Up:
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Time.deltaTime, this.transform.position.z);

                break;

            case Direction.Right:
                this.transform.position = new Vector3(this.transform.position.x + Time.deltaTime, this.transform.position.y, this.transform.position.z);

                break;

            case Direction.Down:
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - Time.deltaTime, this.transform.position.z);

                break;

            default:
                break;
        }
    }

    protected void ChangeDir(Direction dir)
    {
        this.CurrentDirection = dir;
    }

    public virtual void ChangedCell()
    {
        throw new NotImplementedException();
    }
}