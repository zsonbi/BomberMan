using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MovingEntity
{
    [SerializeField]
    private int playerId;

    [SerializeField]
    public string PlayerName { get; private set; }

    public Dictionary<BonusType, Bonus> Bonuses { get; private set; } = new Dictionary<BonusType, Bonus>();
    public Dictionary<KeyCode, Action> Controls { get; private set; } = new Dictionary<KeyCode, Action>();
    public List<Bomb> Bombs { get; private set; } = new List<Bomb>();
    public int Score { get; private set; } = 0;
    public SkinType Skin { get; private set; } = SkinType.Basic;


    // Start is called before the first frame update
    private void Start()
    {
        switch (playerId)
        {
            case 0:
                Controls.Add(KeyCode.A, MoveLeft);
                Controls.Add(KeyCode.W, MoveUp);
                Controls.Add(KeyCode.D, MoveRight);
                Controls.Add(KeyCode.S, MoveDown);
                Controls.Add(KeyCode.Space, PlaceBomb);
                break;

            case 1:
                Controls.Add(KeyCode.A, MoveLeft);
                Controls.Add(KeyCode.W, MoveUp);
                Controls.Add(KeyCode.D, MoveRight);
                Controls.Add(KeyCode.S, MoveDown);
                Controls.Add(KeyCode.Space, PlaceBomb);
                break;

            case 2:
                Controls.Add(KeyCode.A, MoveLeft);
                Controls.Add(KeyCode.W, MoveUp);
                Controls.Add(KeyCode.D, MoveRight);
                Controls.Add(KeyCode.S, MoveDown);
                Controls.Add(KeyCode.Space, PlaceBomb);
                break;

            default:
                Debug.LogError("Inalid player id");
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HandleKeys();
        base.Move(CurrentDirection);
    }

    private void MoveLeft() => base.ChangeDir(Direction.Left);

    private void MoveUp() => base.ChangeDir(Direction.Up);

    private void MoveRight() => base.ChangeDir(Direction.Right);

    private void MoveDown() => base.ChangeDir(Direction.Down);

    private void PlaceBomb()
    {
        throw new System.NotImplementedException();
    }

    private void Detonate()
    {
        throw new System.NotImplementedException();
    }

    private void Reset()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
      base.Init(entityType,gameBoard,CurrentPos);
    }

    private void HandleKeys()
    {
        foreach (var item in Controls)
        {
            if (Input.GetKey(item.Key))
            {
                item.Value.Invoke();
            }
        }
    }

    public void ChangeName(string newName)
    {
        this.PlayerName = newName;
    }
}