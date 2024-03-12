using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MovingEntity
{
    //How long should the player wait between actions
    private float actionCooldown = 0.5f;


    //The id of the player (set it in the editor)
    [SerializeField]
    private int playerId;

    [SerializeField]
    private string playerName = "player";

    /// <summary>
    /// The player's name
    /// </summary>
    public string PlayerName { get => playerName; private set => playerName = value; }

    [SerializeField]
    private GameObject bombPrefab;

    /// <summary>
    /// The bonuses active for the player
    /// </summary>
    public Dictionary<BonusType, Bonus> Bonuses { get; private set; } = new Dictionary<BonusType, Bonus>();
    /// <summary>
    /// The control's for the player
    /// </summary>
    public Dictionary<KeyCode, Action> Controls { get; private set; } = new Dictionary<KeyCode, Action>();
    /// <summary>
    /// The bomb's of the player
    /// </summary>
    public List<Bomb> Bombs { get; private set; } = new List<Bomb>();
    /// <summary>
    /// The current score of the player
    /// </summary>
    public int Score { get; private set; } = 0;
    /// <summary>
    /// What skin does the player use
    /// </summary>
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
                Controls.Add(KeyCode.LeftArrow, MoveLeft);
                Controls.Add(KeyCode.UpArrow, MoveUp);
                Controls.Add(KeyCode.RightArrow, MoveRight);
                Controls.Add(KeyCode.DownArrow, MoveDown);
                Controls.Add(KeyCode.RightShift, PlaceBomb);
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
    private new void Update()
    {
        if (actionCooldown > 0f)
        {
            actionCooldown -= Time.deltaTime;
        }

        HandleKeys();
        base.Update();
    }

    /// <summary>
    /// Check for collision with the elements
    /// </summary>
    /// <param name="collision">What it collided with</param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            this.Kill();
        }
    }

    //Changes the player's direction to left
    private void MoveLeft() => base.ChangeDir(Direction.Left);

    //Changes the player's direction to up
    private void MoveUp() => base.ChangeDir(Direction.Up);

    //Changes the player's direction to right
    private void MoveRight() => base.ChangeDir(Direction.Right);
    
    //Changes the player's direction to down
    private void MoveDown() => base.ChangeDir(Direction.Down);

    //The player places a bomb on the board if it has a bomb available
    private void PlaceBomb()
    {
        if (actionCooldown > 0)
        {
            return;
        }
        actionCooldown = Config.PLAYERACTIONCOOLDOWN;
        
        foreach(Bomb bomb in Bombs)
        {
            if(!bomb.Placed)
            {
                bomb.Place(new Position(this.CurrentBoardPos.Row, this.CurrentBoardPos.Col));             
            }
        }
        //throw new System.NotImplementedException();
    }

    //The player places a bomb on the board if it has a wall available
    private void PlaceWall()
    {
        if (actionCooldown > 0)
        {
            return;
        }
        actionCooldown = Config.PLAYERACTIONCOOLDOWN;

        throw new System.NotImplementedException();
    }

    //The player detonates all of it's bombs
    private void Detonate()
    {
        if (actionCooldown > 0)
        {
            return;
        }
        actionCooldown = Config.PLAYERACTIONCOOLDOWN;

        throw new System.NotImplementedException();
    }

    //Resets the player
    private void Reset()
    {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// Handles the keypresses
    /// </summary>

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(entityType, gameBoard, CurrentPos);
        
        Bomb bomb1 = Instantiate(bombPrefab,this.GameBoard.gameObject.transform).GetComponent<Bomb>();
        bomb1.Init(MapEntityType.Bomb, this.GameBoard, this.CurrentBoardPos);
        Bombs.Add(bomb1);
        
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

    /// <summary>
    /// Changes the player's name
    /// </summary>
    /// <param name="newName">The new name of the player</param>
    public void ChangeName(string newName)
    {
        this.PlayerName = newName;
    }
}