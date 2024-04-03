using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MapEntity
{
    [SerializeField]
    private bool placed = false;

    public bool Placed { get => placed; private set => placed = value; }

    public bool HasBomb { get => placedBomb is not null; }
    //public bool Placed { get; private set; } = false;

    [SerializeField]
    private bool destructible;

    [SerializeField]
    private bool notPassable;

    [SerializeField]
    private Sprite spriteWhenPlaced;

    [SerializeField]
    private Sprite spriteWhenBlownUp;

    [SerializeField]
    private List<GameObject> bonusPrefabs;

    private SpriteRenderer spriteRenderer;

    private Bomb placedBomb = null;

    public bool Destructible { get => destructible; private set => destructible = value; }

    public Bonus ContainingBonus { get; private set; }

    public bool NotPassable { get; private set; } = false;

    private void Awake()
    {
        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void DropBonus()
    {
        ContainingBonus.Show();
        this.ContainingBonus = null;
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

        if (containBonus)
        {
            this.ContainingBonus = Instantiate(bonusPrefabs[Config.RND.Next(0, bonusPrefabs.Count)], this.GameBoard.gameObject.transform).GetComponent<Bonus>();
            this.ContainingBonus.gameObject.transform.transform.localPosition = new Vector3(CurrentBoardPos.Col * Config.CELLSIZE, -2.5f - CurrentBoardPos.Row * Config.CELLSIZE, 1);
        }

        return true;
    }

    public bool BlowUp(bool dropBonus = true)
    {
        if (!this.Placed)
        {
            return false;
        }

        if (HasBomb)
        {
            placedBomb.BlowUp();
            placedBomb = null;
        }
        else if (dropBonus)
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

    public void PlaceBomb(Bomb bombToPlace)
    {
        this.placedBomb = bombToPlace;
        this.Placed = true;
    }

    public void EraseBomb()
    {
        this.placedBomb = null;
        this.Placed = false;
    }
}