using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using DataTypes;
using System.Linq;
using UnityEngine.SceneManagement;
using Bomberman.Menu;

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

        [SerializeField]
        private float circleDecreaseRate;

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

        //How long it will take after one player is alive to the game over to happen
        private float gameOverTimer = Config.GAME_OVER_TIMER;

        //What monster type to force load only works when it is none
        private MonsterType forceMonsterType = MonsterType.None;


        [SerializeField]
        private GameObject CircleGameObject;


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
        /// How quick the circle will decrease
        /// </summary>
        public float CircleDecreaseRate { get => circleDecreaseRate; private set => circleDecreaseRate = value; }

        /// <summary>
        /// The game is paused
        /// </summary>
        public bool Paused { get; private set; } = false;

        /// <summary>
        /// The game over counter is started
        /// </summary>
        public bool StartGameOverCounter { get; private set; } = false;


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

            //Rakd bele majd az if-be
            DecreaseCircle(CircleGameObject.transform.localScale - Vector3.one * circleDecreaseRate * Time.deltaTime);

            if (MainMenuConfig.BattleRoyale)
            {
                DecreaseCircle(CircleGameObject.transform.localScale - circleDecreaseRate * Time.deltaTime * CircleGameObject.transform.localScale);

                //Vector3 sizeChange = (targetCircleSize - circleSize).normalized;
                //Vector3 newCircleSize = circleSize + sizeChange * Time.deltaTime * circleDecreaseRate;
                //DecreaseCircle(circlePosition, newCircleSize);
            }

        }

        // Start is called before the first frame update
        private void Start()
        {
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
        private void DecreaseCircle(Vector3 size)
        {

            CircleGameObject.transform.localScale = size;

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
            StartGameOverCounter = false;
            gameOverTimer = Config.GAME_OVER_TIMER;


            //Cleare out the previous game's entities
            for (int i = 0; i < Cells.GetLength(0); i++)

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
                    Destroy(Entites[0].gameObject);
                    Entites.RemoveAt(0);
                }
            }

            while (Entites.Count > 0)
            {
                Destroy(Entites[0].gameObject);
                Entites.RemoveAt(0);
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

                    //string mapsPath = Directory.GetCurrentDirectory() + "/Assets/Maps/GameMaps/";

                    CreateBoard("Maps/GameMaps/" + maps[Config.RND.Next(0, maps.Length)].name);
                    // CreateBoard("Maps/GameMaps/baseMap");
                }
                Resume();
            }


            Resume();

        }
    }
}