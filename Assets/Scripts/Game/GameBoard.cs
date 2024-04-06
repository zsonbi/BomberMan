using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using DataTypes;

namespace Bomberman
{
    /// <summary>
    /// Controls the game
    /// </summary>
    public class GameBoard : MonoBehaviour
    {
        //What will be used as a delimiter in the maps file if it is a csv

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

        /// <summary>
        /// The cells of the board
        /// </summary>
        public Obstacle[,] Cells { get; private set; }

        /// <summary>
        /// List of the entities such as Bonuses, Bombs etc.
        /// </summary>
        public List<MapEntity> Entites { get; private set; }

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
        /// Event is called when the menu needs to be refreshed
        /// </summary>
        public EventHandler UpdateMenuFields;

        // Start is called before the first frame update
        private void Start()
        {
            //await CreateBoard("Assets/Maps/testMap.csv");
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
            }
        }

        public void MakeMapLoadManual()
        {
            this.loadMapOnStartUp = false;
        }

        //Creates a new board specified by the given file's layout
        public void CreateBoard(string mapLayoutResourcePath)
        {
            //lines
            string[] fileLines = Resources.Load<TextAsset>(mapLayoutResourcePath).text.Trim('\n').Replace("\r","").Split('\n');

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
                Destroy(Monsters[0]);
                Monsters.RemoveAt(0);
            }
            //Spawns the monsters in
            counter = 0;
            while (monsterSpawns.Count != 0 && counter < Config.MonsterCount)
            {
                int index = Config.RND.Next(0, monsterSpawns.Count);
                if (Monsters.Count <= counter)
                {
                    Monsters.Add(Instantiate(monsterPrefabs[Config.RND.Next(0, monsterPrefabs.Count)], this.transform).GetComponent<Monster>());
                }
                Monsters[counter].Init(MapEntityType.Monster, this, monsterSpawns[index]);
                Monsters[counter].gameObject.transform.localPosition = new Vector3(monsterSpawns[index].Col * Config.CELLSIZE, -2.5f - monsterSpawns[index].Row * Config.CELLSIZE, 2);
                monsterSpawns.RemoveAt(index);
                ++counter;
            }
            //Error detection
            if (counter < Config.MonsterCount)
            {
                Debug.LogError("Invalid map, no place to spawn the monsters");
            }
        }

        //Decreases the battle royale circle
        private void DecreaseCircle()
        {
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}