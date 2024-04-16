using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Bomberman;
using DataTypes;
using System.Linq;

public class Player : MovingEntity
{
    //How long should the player wait between actions
    private float actionCooldown = 0f;

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

    public int PlayerId { get => playerId; }

    /// <summary>
    /// Event to call when the player died
    /// </summary>
    public EventHandler PlayerDiedEventHandler;

    private void Awake()
    {
        if (playerId > 2)
        {
            Debug.LogError("PlayerId can't be higher than 2");
        }

        Controls.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpButton" + playerId, Config.PLAYERDEFAULTKEYS[playerId, 0].ToString())), MoveUp);
        Controls.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownButton" + playerId, Config.PLAYERDEFAULTKEYS[playerId, 1].ToString())), MoveDown);
        Controls.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButton" + playerId, Config.PLAYERDEFAULTKEYS[playerId, 2].ToString())), MoveRight);
        Controls.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButton" + playerId, Config.PLAYERDEFAULTKEYS[playerId, 3].ToString())), MoveLeft);
        Controls.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton" + playerId, Config.PLAYERDEFAULTKEYS[playerId, 4].ToString())), PlaceBomb);
        Controls.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DetonateButton" + playerId, Config.PLAYERDEFAULTKEYS[playerId, 5].ToString())), Detonate);
        Controls.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingObstacleButton" + playerId, Config.PLAYERDEFAULTKEYS[playerId, 6].ToString())), PlaceObstacle);

        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("PlayerSkins/" + MainMenuConfig.PlayerSkins[playerId]);
        }
        else
        {
            Debug.LogError("No sprite renderer connected to the player script's gameobject");
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private new void Update()
    {
        if (GameBoard.Paused)
        {
            return;
        }
        if (actionCooldown > 0f)
        {
            actionCooldown -= Time.deltaTime;
        }

        for (int i = 0; i < Bonuses.Keys.Count; i++)
        {
            Bonus bonus = Bonuses.ElementAt(i).Value;
            if (bonus.Decaying)
            {
                if (bonus.DecreaseDuration(Time.deltaTime))
                {
                    switch (bonus.Type)
                    {
                        case BonusType.Slowness:
                            this.timeToMove = 1f / this.Speed;

                            break;

                        default:
                            break;
                    }
                    this.GameBoard.MenuController.RemoveBonus(bonus.Type, this);
                    Destroy(bonus.gameObject);
                    Bonuses.Remove(bonus.Type);
                }
            }
        }

        if (Bonuses.ContainsKey(BonusType.InstantBomb))
        {
            PlaceBomb();
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
        switch (collision.gameObject.tag)
        {
            case "Monster":
                this.Kill();
                break;

            case "Bonus":
                Bonus bonus = collision.gameObject.GetComponent<Bonus>();
                Debug.Log("Picked up " + bonus.name);

                if (!Bonuses.ContainsKey(bonus.Type))
                {
                    Bonuses.Add(bonus.Type, bonus);
                    GameBoard.MenuController.AddBonus(bonus.Type, this);
                }
                switch (bonus.Type)
                {
                    case BonusType.BonusBomb:

                        if (Bonuses[bonus.Type].IncreaseTier())
                        {
                            Bomb bomb1 = Instantiate(bombPrefab, this.GameBoard.gameObject.transform).GetComponent<Bomb>();
                            bomb1.Init(MapEntityType.Bomb, this.GameBoard, this.CurrentBoardPos);
                            this.Bombs.Add(bomb1);
                        }
                        break;

                    case BonusType.BombRange:
                        Bonuses[bonus.Type].IncreaseTier();
                        break;

                    case BonusType.Slowness:
                        Debug.Log("Slowness effect started");
                        this.timeToMove = 1 / (float)(this.Speed * 0.6f);
                        //Missing: This effect lasts for a period of time
                        break;

                    case BonusType.SmallExplosion:
                        Debug.Log("SmallExplosion effect started");
                        //Missing: This effect lasts for a period of time
                        break;

                    case BonusType.NoBomb:
                        Debug.Log("No bomb activated");
                        break;

                    case BonusType.InstantBomb:
                        Debug.Log("InstantBomb effect started");
                        break;
                    case BonusType.Detonator:
                        Debug.Log("Detonator bonus picked up");
                        foreach (Bomb bomb in Bombs)
                        {
                            bomb.Detonable = true;
                        }
                        break;
                    case BonusType.Skate:
                        Debug.Log("Skate bonus picked up");
                        this.timeToMove = 1 / (float)(this.Speed * 1.3f);
                        break;
                    case BonusType.Immunity:
                        Debug.Log("Immunity bonus picked up");
                        break;
                    default:
                        break;
                }
                if (bonus.Decaying)
                {
                    this.GameBoard.Entites.Remove(bonus);

                    if (bonus != Bonuses[bonus.Type])
                    {
                        Destroy(bonus.gameObject);
                    }
                    else
                    {
                        bonus.Hide();
                    }

                    Bonuses[bonus.Type].ResetDecayingBonus(bonus.Duration);
                }
                else if (Bonuses[bonus.Type].Tier == 1)
                {
                    bonus.Hide();
                }
                else
                {
                    this.GameBoard.Entites.Remove(bonus);
                    Destroy(bonus.gameObject);
                }

                break;

            default:
                break;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Circle")
        {
            InstantKill();
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

        if (Bonuses.ContainsKey(BonusType.NoBomb) || GameBoard.Cells[this.CurrentBoardPos.Row, this.CurrentBoardPos.Col].Placed)
        {
            return;
        }

        actionCooldown = Config.PLAYERACTIONCOOLDOWN;

        if (Bonuses.ContainsKey(BonusType.Detonator))
        {
            if (AllTheBombsPlaced())
            {
                foreach (Bomb bomb in Bombs)
                {
                    bomb.BlowUp();
                }
            }            
        }

        foreach (Bomb bomb in Bombs)
        {
            if (!bomb.Placed)
            {
                int bombRange = Config.BOMBDEFAULTEXPLOSIONRANGE;
                if (Bonuses.ContainsKey(BonusType.BombRange))
                {
                    bombRange += Bonuses[BonusType.BombRange].Tier;
                }
                if (Bonuses.ContainsKey(BonusType.SmallExplosion))
                {
                    bombRange = 1;
                }

                bomb.Place(new Position(this.CurrentBoardPos.Row, this.CurrentBoardPos.Col), bombRange);
                GameBoard.Cells[this.CurrentBoardPos.Row, this.CurrentBoardPos.Col].PlaceBomb(bomb);
                return;
            }
        }
        //throw new System.NotImplementedException();
    }

    //The player places a bomb on the board if it has a wall available
    private void PlaceObstacle()
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

    public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
        base.Init(entityType, gameBoard, CurrentPos);

        //Reset the player's components
        while (Bombs.Count != 0)
        {
            Destroy(Bombs[0].gameObject);
            Bombs.RemoveAt(0);
        }
        foreach (BonusType bonus in Enum.GetValues(typeof(BonusType)))
        {
            if (Bonuses.ContainsKey(bonus))
            {
                Bonuses.Remove(bonus);
            }
        }

        Bomb bomb1 = Instantiate(bombPrefab, this.GameBoard.gameObject.transform).GetComponent<Bomb>();
        bomb1.Init(MapEntityType.Bomb, this.GameBoard, this.CurrentBoardPos);
        Bombs.Add(bomb1);
    }

    /// <summary>
    /// Handles the keypresses
    /// </summary>
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
    /// Override the kill event so we can check for game over
    /// </summary>
    public override bool Kill()
    {
        if (Bonuses.ContainsKey(BonusType.Immunity))
        {
            return false;
        }

        bool tookDamage = base.Kill();

        if (!this.Alive)
        {
            PlayerDiedEventHandler?.Invoke(this, EventArgs.Empty);
        }
        else
        {   
            GameBoard.MenuController.RemoveHealth(this);
        }

        return tookDamage;
    }

    /// <summary>
    /// Changes the player's name
    /// </summary>
    /// <param name="newName">The new name of the player</param>
    public void ChangeName(string newName)
    {
        this.PlayerName = newName;
    }

    /// <summary>
    /// Increase the score of the player
    /// </summary>
    public void AddScore()
    {
        ++this.Score;
    }

    /// <summary>
    /// Check if all the bombs placed
    /// </summary>
    private bool AllTheBombsPlaced()
    {
        bool allTheBombsPlaced = true;
        foreach(Bomb bomb in Bombs)
        {    
            allTheBombsPlaced = allTheBombsPlaced && bomb.Placed;
        }
        return allTheBombsPlaced;
    }
}