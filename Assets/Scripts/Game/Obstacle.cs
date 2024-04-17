using Bomberman;
using DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public bool NotPassable { get => notPassable; private set => notPassable = value; }

    public EventHandler BlownUp;

    private void Awake()
    {
        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// This function is for testing purposes only!!!
    /// Spawn a specific bonus at the obstacle's position
    /// </summary>
    public void SpawnBonus(BonusType bonusToSpawn)
    {
        int index = -1;

        for (int i = 0; i < bonusPrefabs.Count; i++)
        {
            if (bonusPrefabs[i].GetComponent<Bonus>().Type == bonusToSpawn)
            {
                index = i;
                break;
            }
        }

        if (index < 0)
        {
            throw new System.Exception("No such bonus we can spawn!");

        }

        Debug.Log(bonusToSpawn.ToString() + index);

        Bonus bonus = Instantiate(bonusPrefabs[index], this.GameBoard.gameObject.transform).GetComponent<Bonus>();
        bonus.gameObject.transform.transform.localPosition = new Vector3(CurrentBoardPos.Col * Config.CELLSIZE, -2.5f - CurrentBoardPos.Row * Config.CELLSIZE, 1);
        bonus.Init(MapEntityType.Bonus, this.GameBoard, new Position(this.CurrentBoardPos.Row, this.CurrentBoardPos.Col));
        bonus.Show();
    }

    private void DropBonus()
    {
        if (ContainingBonus is not null)
        {
            ContainingBonus.Show();
            this.ContainingBonus = null;
        }
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
            this.ContainingBonus.Init(MapEntityType.Bonus, this.GameBoard, new Position(this.CurrentBoardPos.Row, this.CurrentBoardPos.Col));
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

        if (BlownUp is not null)
        {
            BlownUp.Invoke(this, EventArgs.Empty);
            BlownUp = null;
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