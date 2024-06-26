using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Bomberman;
using DataTypes;
using System.Linq;
using Persistance;
using Menu;

namespace Bomberman
{
    /// <summary>
    /// Every player will be derived from this class
    /// </summary>
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

        /// <summary>
        /// The id of the player
        /// </summary>
        public int PlayerId { get => playerId; }

        /// <summary>
        /// How much placeable obstacle does the player has
        /// </summary>
        public int AvailableObstacle { get; private set; } = 0;

        /// <summary>
        /// The max duration of the players immunity
        /// </summary>
        public float ImmunityMaxDuration { get; private set; }

        /// <summary>
        /// The max duration of how much time the player can be a ghost
        /// </summary>
        public float GhostMaxDuration { get; private set; }

        /// <summary>
        /// Event to call when the player died
        /// </summary>
        public EventHandler PlayerDiedEventHandler;

        /// <summary>
        /// The player's display
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// When the script is loaded this method is called
        /// </summary>
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
            Controls.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingObstacleButton" + playerId, Config.PLAYERDEFAULTKEYS[playerId, 5].ToString())), PlaceObstacle);

            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("PlayerSkins/" + MainMenuConfig.PlayerSkins[playerId]);
            }
            else
            {
                Debug.LogError("No sprite renderer connected to the player script's gameobject");
            }
        }

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
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
                                if (Bonuses.ContainsKey(BonusType.Skate))
                                {
                                    this.timeToMove = 1 / (float)(this.Speed * 1.3f);
                                }
                                else
                                {
                                    this.timeToMove = 1f / this.Speed;
                                }
                                break;

                            case BonusType.Ghost:
                                if (GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col].Placed && !GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col].HasBomb)
                                {
                                    InstantKill();
                                }
                                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
                                this.SetGhost(false);
                                break;

                            case BonusType.Immunity:
                                spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a);
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
            if (Bonuses.ContainsKey(BonusType.Immunity))
            {
                spriteRenderer.color = new Color(1, 1 - Bonuses[BonusType.Immunity].Duration / ImmunityMaxDuration, 1 - Bonuses[BonusType.Immunity].Duration / ImmunityMaxDuration, spriteRenderer.color.a);
            }
            if (Bonuses.ContainsKey(BonusType.Ghost))
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1 - Bonuses[BonusType.Ghost].Duration / (GhostMaxDuration * 2));
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
                        //If this bonus is picked up it increases the player bomb count by one
                        case BonusType.BonusBomb:

                            if (Bonuses[bonus.Type].IncreaseTier())
                            {
                                Bomb bomb1 = Instantiate(bombPrefab, this.GameBoard.gameObject.transform).GetComponent<Bomb>();
                                bomb1.Init(MapEntityType.Bomb, this.GameBoard, this.CurrentBoardPos);
                                this.Bombs.Add(bomb1);
                            }
                            break;
                        //If this bonus is picked up it increases the player bombs range by one
                        case BonusType.BombRange:
                            Bonuses[bonus.Type].IncreaseTier();
                            break;
                        //If this bonus is picked up the player slowes down
                        case BonusType.Slowness:
                            Bonuses[bonus.Type].IncreaseTier();
                            Debug.Log("Slowness effect started");
                            this.timeToMove = 1 / (float)(this.Speed * 0.6f);
                            //Missing: This effect lasts for a period of time
                            break;
                        //If this bonus is picked up it decreases the player bombs range
                        case BonusType.SmallExplosion:
                            Bonuses[bonus.Type].IncreaseTier();
                            Debug.Log("SmallExplosion effect started");
                            //Missing: This effect lasts for a period of time
                            break;
                        //If this bonus is picked up it is temporarily disables the bomb placement for the player
                        case BonusType.NoBomb:
                            Bonuses[bonus.Type].IncreaseTier();
                            Debug.Log("No bomb activated");
                            break;
                        //If this bonus is picked up the player will place down all its bombs
                        case BonusType.InstantBomb:
                            Bonuses[bonus.Type].IncreaseTier();
                            Debug.Log("InstantBomb effect started");
                            break;
                        //If this bonus is picked up the player will be able to detonate its bombs, however only after placing all of them and they dont
                        //blow up with time
                        case BonusType.Detonator:
                            Bonuses[bonus.Type].IncreaseTier();
                            Debug.Log("Detonator bonus picked up");
                            foreach (Bomb bomb in Bombs)
                            {
                                bomb.Detonable = true;
                            }
                            break;
                        //If this bonus is picked up the player speed will be increased
                        case BonusType.Skate:
                            Bonuses[bonus.Type].IncreaseTier();
                            Debug.Log("Skate bonus picked up");
                            if (!Bonuses.ContainsKey(BonusType.Slowness))
                                this.timeToMove = 1 / (float)(this.Speed * 1.3f);
                            break;
                        //If this bonus is picked up the player will be immune for damage for a short peroid of time
                        case BonusType.Immunity:
                            Bonuses[bonus.Type].IncreaseTier();
                            ImmunityMaxDuration = bonus.Duration;
                            spriteRenderer.color = new Color(1, 0, 0, spriteRenderer.color.a);
                            break;

                        case BonusType.Ghost:
                            Bonuses[bonus.Type].IncreaseTier();
                            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
                            GhostMaxDuration = bonus.Duration;
                            this.SetGhost(true);
                            break;

                        case BonusType.Obstacle:

                            if (Bonuses[bonus.Type].IncreaseTier())
                            {
                                AvailableObstacle += 3;
                            }
                            break;

                        default:
                            break;
                    }
                    if (bonus.Decaying)
                    {
                        this.GameBoard.Entites.Remove(bonus);

                        if (bonus != Bonuses[bonus.Type])
                        {
                            this.GameBoard.Entites.Remove(bonus);

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
                        bonus.Hide();
                        this.GameBoard.Entites.Remove(bonus);
                        Destroy(bonus.gameObject);
                    }

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Check for collision with the battle royale circle, if collided it kills the player instant
        /// </summary>
        public void OnCollisionExit2D(Collision2D collision)
        {
            Debug.Log(collision.gameObject.tag);
            if (collision.gameObject.tag == "Circle")
            {
                InstantKill();
            }
        }

        /// <summary>
        /// Changes the player's direction to left
        /// </summary>
        private void MoveLeft() => base.ChangeDir(Direction.Left);

        /// <summary>
        /// Changes the player's direction to up
        /// </summary>
        private void MoveUp() => base.ChangeDir(Direction.Up);

        /// <summary>
        /// Changes the player's direction to right
        /// </summary>
        private void MoveRight() => base.ChangeDir(Direction.Right);

        /// <summary>
        /// Changes the player's direction to down
        /// </summary>
        private void MoveDown() => base.ChangeDir(Direction.Down);

        /// <summary>
        /// The player places a bomb on the board if it has a bomb available
        /// </summary>
        private void PlaceBomb()
        {
            if (actionCooldown > 0)
            {
                return;
            }

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

            if (Bonuses.ContainsKey(BonusType.NoBomb) || GameBoard.Cells[this.CurrentBoardPos.Row, this.CurrentBoardPos.Col].Placed)
            {
                return;
            }

            actionCooldown = Config.PLAYERACTIONCOOLDOWN;

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

        /// <summary>
        /// The player places a bomb on the board if it has a wall available
        /// </summary>
        private void PlaceObstacle()
        {
            if (actionCooldown > 0 || !Bonuses.ContainsKey(BonusType.Obstacle) || AvailableObstacle <= 0)
            {
                return;
            }
            actionCooldown = Config.PLAYERACTIONCOOLDOWN;

            Obstacle obstacle = GameBoard.Cells[CurrentBoardPos.Row, CurrentBoardPos.Col];
            if (!obstacle.Placed)
            {
                AvailableObstacle--;
                obstacle.Place(false, playerId);
                obstacle.BlownUp = PlacedObstacleBlownUp;
            }
        }

        /// <summary>
        /// This methid is called when someone blows up one of the players obstacles
        /// </summary>
        private void PlacedObstacleBlownUp(object obj, EventArgs args)
        {
            ++AvailableObstacle;
        }

        /// <summary>
        /// Initialize the player object with base values
        /// </summary>
        public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
        {
            base.Init(entityType, gameBoard, CurrentPos);

            this.SetGhost(false);

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
        /// Loads in the player
        /// </summary>
        /// <param name="playerSave">The player's save to load</param>
        /// <param name="gameBoard">The parent gameBoard of the player</param>
        /// <param name="playerObstacles">The obstacles which was placed by this player</param>
        public void LoadPlayer(PlayerSave playerSave, GameBoard gameBoard, List<Obstacle> playerObstacles)
        {
            //Reset the player's components
            while (Bombs.Count != 0)
            {
                Destroy(Bombs[0].gameObject);
                Bombs.RemoveAt(0);
            }
            //Clear the player's bonuses if it has any
            foreach (BonusType bonus in Enum.GetValues(typeof(BonusType)))
            {
                if (Bonuses.ContainsKey(bonus))
                {
                    Bonuses.Remove(bonus);
                }
            }
            //Change every field to match the save's fields
            base.Init(MapEntityType.Player, gameBoard, playerSave.CurrentBoardPos);
            this.SetGhost(false);
            this.Score = playerSave.Score;
            this.Alive = playerSave.Alive;
            this.Hp = playerSave.Hp;
            this.ImmuneTime = playerSave.ImmuneTime;
            this.CurrentDirection = playerSave.CurrentDirection;
            this.playerName = playerSave.PlayerName;
            //Load the bonuses
            foreach (var item in playerSave.Bonuses)
            {
                switch (item.Type)
                {
                    case BonusType.Immunity:
                        break;

                    case BonusType.Ghost:
                        this.SetGhost(true);
                        break;

                    case BonusType.Skate:
                        if (!Bonuses.ContainsKey(BonusType.Slowness))
                        {
                            this.timeToMove = 1 / (float)(this.Speed * 1.3f);
                        }
                        break;

                    case BonusType.Slowness:
                        this.timeToMove = 1 / (float)(this.Speed * 0.6f);
                        break;

                    default:
                        break;
                }

                Bonus bonus = Instantiate(gameBoard.BonusPrefabs[item.Type], this.GameBoard.gameObject.transform).GetComponent<Bonus>();
                bonus.gameObject.transform.transform.localPosition = new Vector3(CurrentBoardPos.Col * Config.CELLSIZE, -2.5f - CurrentBoardPos.Row * Config.CELLSIZE, 1);
                bonus.Init(MapEntityType.Bonus, this.GameBoard, new Position(this.CurrentBoardPos.Row, this.CurrentBoardPos.Col));
                bonus.LoadBonus(item);
                Bonuses.Add(item.Type, bonus);
            }

            //Load the bombs
            foreach (var item in playerSave.Bombs)
            {
                Bomb bomb1 = Instantiate(bombPrefab, this.GameBoard.gameObject.transform).GetComponent<Bomb>();
                bomb1.Init(MapEntityType.Bomb, this.GameBoard, item.CurrentBoardPos);
                bomb1.LoadBomb(item);

                Bombs.Add(bomb1);
            }
            this.AvailableObstacle = playerSave.AvailableObstacle;
            //Set the display timers
            this.GhostMaxDuration = playerSave.GhostMaxDuration;
            this.ImmunityMaxDuration = playerSave.ImmunityMaxDuration;
            //Add the events to the player's owned obstacles
            foreach (var item in playerObstacles)
            {
                item.BlownUp = this.PlacedObstacleBlownUp;
            }
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
            else if (tookDamage)
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
            foreach (Bomb bomb in Bombs)
            {
                if (!bomb.Placed)
                {
                    return false;
                }
            }
            return true;
        }
    }
}