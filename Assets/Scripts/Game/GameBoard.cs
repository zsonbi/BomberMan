using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DataTypes;
using System.Linq;
using UnityEngine.SceneManagement;
using Menu;
using UnityEngine.UI;
using UnityEditor;
using Persistance;
using Newtonsoft.Json;

namespace Bomberman
{
    /// <summary>
    /// Controls the game
    /// </summary>
    public class GameBoard : MonoBehaviour
    {
        [SerializeField]
        public MenuController MenuController;

        [SerializeField]
        [Header("Path to the asset you want to force load (Can be left empty)")]
        private string mapAssetPath = "";

        [SerializeField]
        private bool loadMapOnStartUp = true;

        //Prefabs link them in editor!
        [SerializeField]
        private GameObject indestructibleWallPrefab;

        [SerializeField]
        private GameObject destructibleWallPrefab;

        [SerializeField]
        private List<GameObject> playerPrefabs;

        [SerializeField]
        private List<GameObject> monsterPrefabs;

        [SerializeField]
        private ModalWindow modalWindow;

        [SerializeField]
        private GameObject CircleGameObject;

        [SerializeField]
        private Text BattleRoyaleTimerText;

        [SerializeField]
        private SavesMenu SavesMenu;

        public float gameOverTimer { get; private set; } = Config.GAME_OVER_TIMER;

        //What monster type to force load only works when it is none
        private MonsterType forceMonsterType = MonsterType.None;

        //The timers for the battle royale circle (even index stay, odd index decrease)
        public float[] BattleRoyaleTimers { get; private set; }

        //What index to use in the timers
        public int BattleRoyaleTimerIndex { get; private set; } = 0;

        private float circleDecreaseRate = Config.CIRCLE_DECREASE_RATE;

        /// <summary>
        /// The cells of the board
        /// </summary>
        public Obstacle[,] Cells { get; private set; }

        /// <summary>
        /// List of the entities such as Bonuses, Bombs etc.
        /// </summary>
        public List<MapEntity> Entites { get; private set; } = new List<MapEntity>();

        /// <summary>
        /// The number of rows of the board
        /// </summary>
        public int RowCount { get; private set; }

        /// <summary>
        /// The number of cols of the board
        /// </summary>
        public int ColCount { get; private set; }

        /// <summary>
        /// The players on the board (check for alive)
        /// </summary>
        public List<Player> Players { get; private set; } = new List<Player>();

        /// <summary>
        /// The players on the board (check for alive)
        /// </summary>
        public List<Monster> Monsters { get; private set; } = new List<Monster>();

        /// <summary>
        /// The game is paused
        /// </summary>
        public bool Paused { get; private set; } = false;

        /// <summary>
        /// The game over counter is started
        /// </summary>
        public bool StartGameOverCounter { get; private set; } = false;

        internal Dictionary<BonusType, GameObject> BonusPrefabs { get; private set; }

        //Called every frame
        private void Update()
        {
            if (Paused)
            {
                return;
            }
            if (StartGameOverCounter)
            {
                if (!Paused)
                {
                    gameOverTimer -= Time.deltaTime;

                    if (gameOverTimer <= 0)
                    {
                        Pause();

                        Player winner = Players.Find(x => x.Alive);
                        string modalContent;
                        if (winner is not null)
                        {
                            winner.AddScore();
                            modalContent = $"{winner.PlayerName} won this round!";
                        }
                        else
                        {
                            modalContent = "The round was a draw";
                        }

                        if (winner?.Score >= MainMenuConfig.RequiredPoint)
                        {
                            modalWindow.Show("The game is over", $"{winner.PlayerName} won the game!", BackToMainMenu, "Back to main menu");
                        }
                        else
                        {
                            modalWindow.Show("Round is over!", modalContent, StartNextGame);
                        }
                    }
                }
            }

            if (MainMenuConfig.BattleRoyale)
            {
                if (BattleRoyaleTimerIndex < BattleRoyaleTimers.Length)
                {
                    BattleRoyaleTimers[BattleRoyaleTimerIndex] -= Time.deltaTime;
                    if (BattleRoyaleTimers[BattleRoyaleTimerIndex] < 0)
                    {
                        BattleRoyaleTimerIndex++;
                    }
                    CountDown();
                    if (BattleRoyaleTimerIndex % 2 == 1)
                    {
                        DecreaseCircle();
                    }
                }
                else
                {
                    DecreaseCircle();
                }
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (CircleGameObject is null)
            {
                throw new Exception("The battle royale circle object is not set");
            }

            if (BattleRoyaleTimerText is null)
            {
                throw new Exception("The battle royale timer container object is not set");
            }
            BonusPrefabs = new Dictionary<BonusType, GameObject>();
            foreach (var item in destructibleWallPrefab.GetComponent<Obstacle>().bonusPrefabs)
            {
                Bonus bonus = item.GetComponent<Bonus>();
                BonusPrefabs.Add(bonus.Type, item);
            }

            StartNextGame();
        }

        public void MakeMapLoadManual()
        {
            this.loadMapOnStartUp = false;
        }

        //Creates a new board specified by the given file's layout
        public void CreateBoard(string mapLayoutResourcePath)
        {
            //lines
            string[] fileLines = Resources.Load<TextAsset>(mapLayoutResourcePath).text.Trim('\n').Replace("\r", "").Split('\n');

            this.Cells = new Obstacle[fileLines.Length, fileLines[0].Split(Config.CSVDELIMITER).Length];
            this.RowCount = this.Cells.GetLength(0);
            this.ColCount = this.Cells.GetLength(1);
            List<Position> playerSpawns = new List<Position>();
            List<Position> monsterSpawns = new List<Position>();

            for (int i = 0; i < fileLines.Length; i++)
            {
                string[] splitted = fileLines[i].Split(Config.CSVDELIMITER);
                for (int j = 0; j < splitted.Length; j++)
                {
                    Obstacle obstacle;
                    //Determine by the type what to spawn
                    switch ((MapCell)Convert.ToByte(splitted[j]))
                    {
                        case MapCell.Walkable:
                            obstacle = Instantiate(destructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                            obstacle.Init(MapEntityType.Obstacle, this, new Position(i, j));
                            break;

                        case MapCell.IndestructibleWall:
                            obstacle = Instantiate(indestructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                            obstacle.Init(MapEntityType.Obstacle, this, new Position(i, j));
                            obstacle.Place(false);
                            break;

                        case MapCell.DestructibleWall:
                            obstacle = Instantiate(destructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                            obstacle.Init(MapEntityType.Obstacle, this, new Position(i, j));
                            obstacle.Place(true);
                            break;

                        case MapCell.PlayerSpawn:
                            obstacle = Instantiate(destructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                            obstacle.Init(MapEntityType.Obstacle, this, new Position(i, j));
                            playerSpawns.Add(new Position(i, j));

                            break;

                        case MapCell.MonsterSpawn:
                            obstacle = Instantiate(destructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                            obstacle.Init(MapEntityType.Obstacle, this, new Position(i, j));
                            monsterSpawns.Add(new Position(i, j));
                            break;

                        default:
                            Debug.LogError("No such MapcellType which was in the file");
                            obstacle = null;
                            break;
                    }

                    //Set the object position's on the board
                    obstacle.gameObject.transform.localPosition = new Vector3(j * Config.CELLSIZE, -2.5f - i * Config.CELLSIZE, 1);

                    Cells[i, j] = obstacle;
                }
            }
            //Spawns the player's in
            int counter = 0;
            while (playerSpawns.Count != 0 && counter < (MainMenuConfig.Player3 ? 3 : 2))
            {
                int index = Config.RND.Next(0, playerSpawns.Count);
                if (Players.Count <= counter)
                {
                    Players.Add(Instantiate(playerPrefabs[counter], this.transform).GetComponent<Player>());
                }
                Players[counter].Init(MapEntityType.Player, this, playerSpawns[index]);
                Players[counter].gameObject.transform.localPosition = new Vector3(playerSpawns[index].Col * Config.CELLSIZE, -2.5f - playerSpawns[index].Row * Config.CELLSIZE, 2);
                Players[counter].ChangeName(MainMenuConfig.PlayerNames[counter]);
                Players[counter].PlayerDiedEventHandler = CheckGameOverEvent;
                Players[counter].gameObject.SetActive(true);
                playerSpawns.RemoveAt(index);
                ++counter;
            }
            //Error detection
            if (counter < (MainMenuConfig.Player3 ? 3 : 2))
            {
                Debug.LogError("Invalid map, no place to spawn the players");
            }

            //Delete the monsters so they are still random
            while (Monsters.Count != 0)
            {
                Destroy(Monsters[0].gameObject);
                Monsters.RemoveAt(0);
            }
            //Spawns the monsters in
            counter = 0;
            while (monsterSpawns.Count != 0)
            {
                int index = Config.RND.Next(0, monsterSpawns.Count);
                if (Monsters.Count <= counter)
                {
                    if (forceMonsterType == MonsterType.None)
                    {
                        Monsters.Add(Instantiate(monsterPrefabs[Config.RND.Next(0, monsterPrefabs.Count)], this.transform).GetComponent<Monster>());
                    }
                    else
                    {
                        Monsters.Add(Instantiate(monsterPrefabs[(int)forceMonsterType], this.transform).GetComponent<Monster>());
                    }
                }
                Monsters[counter].Init(MapEntityType.Monster, this, monsterSpawns[index]);
                Monsters[counter].gameObject.transform.localPosition = new Vector3(monsterSpawns[index].Col * Config.CELLSIZE, -2.5f - monsterSpawns[index].Row * Config.CELLSIZE, 2);
                monsterSpawns.RemoveAt(index);
                ++counter;
            }

            this.MenuController.NewGame(Players);
        }

        /// <summary>
        ///Decreases the battle royale circle
        /// </summary>
        private void DecreaseCircle()
        {
            if (CircleGameObject.transform.localScale.x < 0)
            {
                return;
            }

            Vector3 size = CircleGameObject.transform.localScale - Vector3.one * circleDecreaseRate * Time.deltaTime;
            size.z = 0;
            CircleGameObject.transform.localScale = size;
        }

        /// <summary>
        /// Count down waiting and shringing time
        /// </summary>
        private void CountDown()
        {
            Debug.Log("MainMenuConfig.BattleRoyale:" + MainMenuConfig.BattleRoyale);
            if (MainMenuConfig.BattleRoyale && BattleRoyaleTimerIndex < BattleRoyaleTimers.Length)
            {
                DateTime datetime = new DateTime((int)(TimeSpan.TicksPerSecond * BattleRoyaleTimers[BattleRoyaleTimerIndex]));
                BattleRoyaleTimerText.text = datetime.ToString("mm:ss");
            }
            else
            {
                BattleRoyaleTimerText.text = "00:00";
            }
        }

        /// <summary>
        /// Make the game paused
        /// </summary>
        public void Pause()
        {
            this.Paused = true;
        }

        /// <summary>
        /// Make the game unpaused
        /// </summary>
        public void Resume()
        {
            if (gameOverTimer >= 0)
            {
                this.Paused = false;
            }
        }

        /// <summary>
        /// Check if the game over timer should be started
        /// </summary>
        public void CheckGameOverEvent(object obj, EventArgs args)
        {
            if (Players.Count(x => x.Alive) == 1)
            {
                StartGameOverCounter = true;
            }
        }

        /// <summary>
        /// Change to the main menu scene
        /// </summary>
        public void BackToMainMenu()
        {
            SceneManager.LoadSceneAsync("MainMenuScene");
        }

        /// <summary>
        /// Closes the game (the whole application)
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Overrides the battle royale timers for testing only
        /// </summary>
        /// <param name="newTimers">The new timers to use</param>
        public void OverrideBattleRoyaleTimers(float[] newTimers, float newRate = Config.CIRCLE_DECREASE_RATE)
        {
            this.BattleRoyaleTimers = newTimers;
            circleDecreaseRate = newRate;
        }

        /// <summary>
        /// Make a monster type force loaded
        /// </summary>
        /// <param name="monsterType">The type to force use (MonsterType.None) to reset it back to random</param>
        public void ForceSpecificMobTypeOnLoad(MonsterType monsterType)
        {
            this.forceMonsterType = monsterType;
        }

        /// <summary>
        ///Spawn a bomb at the given position
        /// </summary>
        /// <param name="whereToSpawn">Where to spawn the bomb</param>
        public void SpawnBomb(Position whereToSpawn, int radius = Config.BOMBDEFAULTEXPLOSIONRANGE, bool permament = false)
        {
            Bomb bombToSpawn = Instantiate(Players[0].Bombs[0], this.transform).GetComponent<Bomb>();
            bombToSpawn.Init(MapEntityType.Bomb, this, whereToSpawn);
            this.Cells[whereToSpawn.Row, whereToSpawn.Col].PlaceBomb(bombToSpawn);

            bombToSpawn.PlaceByGameBoard(whereToSpawn, radius, permament);
        }

        /// <summary>
        /// Spawn a bonus at a specific coordinate
        /// </summary>
        /// <param name="bonusType">The type of the bonus</param>
        /// <param name="pos">The position to spawn the bonus at</param>

        public void SpawnBonus(BonusType bonusType, Position pos)
        {
            if (pos.Row >= 0 && pos.Col >= 0 && pos.Row < this.Cells.GetLength(0) && pos.Col < Cells.GetLength(1))
            {
                this.Cells[pos.Row, pos.Col].SpawnBonus(bonusType);
            }
            else
            {
                throw new IndexOutOfRangeException("The cells array is not big enough to spawn a bonus there!");
            }
        }

        /// <summary>
        /// Start the next game
        /// </summary>
        public void StartNextGame()
        {
            if (MainMenuConfig.mapPathToLoad != "")
            {
                LoadState(MainMenuConfig.mapPathToLoad);
            }
            else
            {
                StartGameOverCounter = false;
                gameOverTimer = Config.GAME_OVER_TIMER;
                //Reset the battle royale components
                CircleGameObject.SetActive(MainMenuConfig.BattleRoyale);
                BattleRoyaleTimerText.transform.parent.gameObject.SetActive(MainMenuConfig.BattleRoyale);
                BattleRoyaleTimers = Config.BATTLE_ROYALE_TIMERS.Select(x => x).ToArray();
                BattleRoyaleTimerIndex = 0;
                //Reset the battle royale circle
                if (CircleGameObject is not null)
                {
                    CircleGameObject.transform.localScale = new Vector3(1000, 1000);
                }
                if (Cells is not null)
                {
                    //Cleare out the previous game's entities
                    for (int i = 0; i < Cells.GetLength(0); i++)
                    {
                        for (int j = 0; j < Cells.GetLength(1); j++)
                        {
                            Destroy(Cells[i, j].gameObject);
                        }
                    }
                    while (Entites.Count > 0)
                    {
                        if (Entites[0] != null)
                        {
                            Destroy(Entites[0].gameObject);
                        }
                        Entites.RemoveAt(0);
                    }
                }
                if (loadMapOnStartUp)
                {
                    if (mapAssetPath != "")
                    {
                        CreateBoard(mapAssetPath);
                    }
                    else
                    {
                        //Not efficient, but can't do it other way
                        TextAsset[] maps = Resources.LoadAll<TextAsset>("Maps/GameMaps/");

                        CreateBoard("Maps/GameMaps/" + maps[Config.RND.Next(0, maps.Length)].name);
                    }
                    Resume();
                }
            }
        }

        /// <summary>
        /// Saves the gameBoards state
        /// </summary>
        public void SaveState(string saveId = "gameSaves\\save1.json")
        {
            if (!Directory.Exists("gameSaves"))
            {
                Directory.CreateDirectory("gameSaves");
            }
            string[] files = Directory.GetFiles("gameSaves");
            if (saveId == "")
            {
                saveId = "gameSaves/" + DateTime.Now.ToString("yyyy_MM_DD_HH_mm_ss") + ".json";
            }

            GameSave gameSave = new GameSave(this, CircleGameObject.transform.localScale);
            string jsonString = JsonConvert.SerializeObject(gameSave, formatting: Formatting.Indented);

            File.WriteAllText(saveId, jsonString);
        }

        /// <summary>
        /// Loads the game according to the file
        /// </summary>
        /// <param name="savePath">The path to the saveFile</param>
        public void LoadState(string savePath)
        {
            string json = File.ReadAllText(savePath);
            //Load it
            GameSave gameSave = JsonConvert.DeserializeObject<GameSave>(json);

            //Change the gameboard's field and MenuConfig to match the save's fields
            this.gameOverTimer = gameSave.GameOverTimer;
            this.BattleRoyaleTimers = gameSave.BattleRoyaleTimers;
            this.BattleRoyaleTimerIndex = gameSave.BattleRoyaleTimerIndex;
            this.RowCount = gameSave.RowCount;
            this.ColCount = gameSave.ColCount;
            this.Paused = gameSave.Paused;
            this.StartGameOverCounter = gameSave.StartGameOverCounter;
            MainMenuConfig.BattleRoyale = gameSave.BattleRoyaleMode;
            MainMenuConfig.RequiredPoint = gameSave.RequiredPoints;
            CircleGameObject.SetActive(MainMenuConfig.BattleRoyale);
            BattleRoyaleTimerText.transform.parent.gameObject.SetActive(MainMenuConfig.BattleRoyale);
            //Change the battle royale circle's size
            if (CircleGameObject is not null)
            {
                CircleGameObject.transform.localScale = new Vector3(gameSave.BattleRoyaleCircle.Col, gameSave.BattleRoyaleCircle.Row);
            }
            //Delete the old gameObjects
            if (Cells is not null)
            {
                //Cleare out the previous game's entities
                for (int i = 0; i < Cells.GetLength(0); i++)
                {
                    for (int j = 0; j < Cells.GetLength(1); j++)
                    {
                        Destroy(Cells[i, j].gameObject);
                    }
                }
                while (Entites.Count > 0)
                {
                    if (Entites[0] != null)
                    {
                        Destroy(Entites[0].gameObject);
                    }
                    Entites.RemoveAt(0);
                }
            }

            //Load the game's state
            this.Cells = new Obstacle[RowCount, ColCount];
            //This is for the obstacle bonus
            Dictionary<int, List<Obstacle>> playerObstacles = new Dictionary<int, List<Obstacle>>();
            for (int i = 0; i < 3; i++)
            {
                playerObstacles.Add(i, new List<Obstacle>());
            }
            //Create the cells
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    ObstacleSave obstacleSave = gameSave.Cells[i * RowCount + j];
                    Obstacle obstacle;
                    if (obstacleSave.NotPassable)
                    {
                        obstacle = Instantiate(indestructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                    }
                    else
                    {
                        obstacle = Instantiate(destructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                    }
                    obstacle.Init(MapEntityType.Obstacle, this, obstacleSave.CurrentBoardPos);
                    obstacle.gameObject.transform.localPosition = new Vector3(j * Config.CELLSIZE, -2.5f - i * Config.CELLSIZE, 1);
                    obstacle.ObstacleLoad(obstacleSave);

                    if (obstacleSave.OwnerId != -1)
                    {
                        playerObstacles[obstacleSave.OwnerId].Add(obstacle);
                    }

                    Cells[i, j] = obstacle;
                }
            }
            //Load the players
            for (int i = 0; i < gameSave.Players.Count; i++)
            {
                MainMenuConfig.PlayerNames[i] = gameSave.Players[i].SkinName;

                MainMenuConfig.PlayerNames[i] = gameSave.Players[i].PlayerName;
                if (Players.Count <= i)
                {
                    Players.Add(Instantiate(playerPrefabs[i], this.transform).GetComponent<Player>());
                }

                Players[i].LoadPlayer(gameSave.Players[i], this, playerObstacles[i]);
                Players[i].gameObject.transform.localPosition = new Vector3(gameSave.Players[i].CurrentBoardPos.Col * Config.CELLSIZE, -2.5f - gameSave.Players[i].CurrentBoardPos.Row * Config.CELLSIZE, 2);

                Players[i].PlayerDiedEventHandler = CheckGameOverEvent;
                Players[i].gameObject.SetActive(true);
            }

            //Delete the monsters so they are still random
            while (Monsters.Count != 0)
            {
                Destroy(Monsters[0].gameObject);
                Monsters.RemoveAt(0);
            }
            //Load the monsters
            for (int i = 0; i < gameSave.Monsters.Count; i++)
            {
                Monsters.Add(Instantiate(monsterPrefabs[(int)gameSave.Monsters[i].Type], this.transform).GetComponent<Monster>());
                Monsters[i].gameObject.transform.localPosition = new Vector3(gameSave.Monsters[i].CurrentBoardPos.Col * Config.CELLSIZE, -2.5f - gameSave.Monsters[i].CurrentBoardPos.Row * Config.CELLSIZE, 2);

                Monsters[i].LoadMonster(gameSave.Monsters[i], this);
            }
            //And spawn the currently visible bonuses
            foreach (var item in gameSave.droppedBonusSaves)
            {
                this.SpawnBonus(item.Type, item.CurrentBoardPos);
            }

            this.MenuController.NewGame(Players);
            //Update the in game menu to display the new gamestate
            foreach (var item in this.Players)
            {
                foreach (var bonus in item.Bonuses)
                {
                    MenuController.AddBonus(bonus.Key, item);
                }
            }

            this.Resume();
            //Clear this load request
            MainMenuConfig.mapPathToLoad = "";
            //Check if it is a 3 player game
            MainMenuConfig.Player3 = gameSave.Players.Count == 3;
        }
    }
}