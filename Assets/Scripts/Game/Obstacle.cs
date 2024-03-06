using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MapEntity
{
    

    public bool Placed = false;
    //public bool Placed { get; private set; } = false;

    [SerializeField]
    private bool destructible;

    [SerializeField]
    private bool notPassable;

    [SerializeField]
    private Sprite spriteWhenPlaced;

    [SerializeField]
    private Sprite spriteWhenBlownUp;

    private SpriteRenderer spriteRenderer;

    public bool Destructible { get => destructible; private set => destructible = value; }

    public Bonus ContainingBonus { get; private set; }

    public bool NotPassable { get; private set; } = false;

    private void Awake()
    {
        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void DropBonus()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(entityType, gameBoard, CurrentPos);
    }

    public bool Place(bool containBonus)
    {
        if (this.Placed)
        {
            return false;
        }
        this.Placed = true;

        if (spriteWhenPlaced is not null)
        {
            spriteRenderer.sprite = spriteWhenPlaced;
        }
        else
        {
            Debug.LogError("Sprite when placed is not set!");
        }

        return true;
    }

    public bool BlowUp(bool dropBonus = true)
    {
        if (!this.Placed)
        {
            return false;
        }

        if (dropBonus)
        {
            DropBonus();
        }

        this.Placed = false;
        if (spriteWhenBlownUp is not null)
        {
            spriteRenderer.sprite = spriteWhenBlownUp;
        }
        else
        {
            Debug.LogError("Sprite when blown up is not set!");
        }
        return true;
    }
}