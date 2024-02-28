using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameBoard : MonoBehaviour
{
    private const char CSVDELIMITER = ';';
    private const int HEIGHT = 100;
    private const int WIDTH = 100;



    public Obstacle[,] Cells { get; private set; }

    public List<MapEntity> Entites { get; private set; }
    public int RowCount { get; private set; }
    public int ColCount { get; private set; }

    public List<Player> Players { get; private set; }= new List<Player>();

    public List<Monster> Monsters { get; private set; }= new List<Monster>();

    [SerializeField]
    public float CircleDecreaseRate { get; private set; }

    [SerializeField]
    private GameObject indestructibleWallPrefab;

    [SerializeField]
    private GameObject destructibleWallPrefab;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject monsterPrefab;



    public EventHandler UpdateMenuFields;

    // Start is called before the first frame update
    private async void Start()
    {
        await CreateBoard("Assets/Maps/baseMap.csv");

    }

    private async Task CreateBoard(string mapLayoutFilename)
    {

        string mapsPath = Directory.GetCurrentDirectory() + "/";

        string[] fileLines = await File.ReadAllLinesAsync(mapsPath + mapLayoutFilename);

        this.Cells = new Obstacle[fileLines.Length, fileLines[0].Split(CSVDELIMITER).Length];

        List<Position> playerSpawns = new List<Position>();
        List<Position> monsterSpawns = new List<Position>();

        for (int i = 0; i < fileLines.Length; i++)
        {
            string[] splitted = fileLines[i].Split(CSVDELIMITER);
            for (int j = 0; j < splitted.Length; j++)
            {
                Obstacle obstacle;
                switch ((MapCell)Convert.ToByte(splitted[j]))
                {
                    case MapCell.Walkable:
                        obstacle = Instantiate(destructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();

                        break;

                    case MapCell.IndestructibleWall:
                        obstacle = Instantiate(indestructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                        obstacle.Place(false);
                        break;

                    case MapCell.DestructibleWall:
                        obstacle = Instantiate(destructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                        obstacle.Place(true);
                        break;

                    case MapCell.PlayerSpawn:
                        obstacle = Instantiate(destructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                        playerSpawns.Add(new Position(i, j));

                        break;

                    case MapCell.MonsterSpawn:
                        obstacle = Instantiate(destructibleWallPrefab, this.gameObject.transform).GetComponent<Obstacle>();
                        monsterSpawns.Add(new Position(i, j));
                        break;

                    default:
                        Debug.LogError("No such MapcellType which was in the file");
                        obstacle = null;
                        break;
                }

                obstacle.Init(MapEntityType.Obstacle, this, new Position(i, j));
                obstacle.gameObject.transform.localPosition = new Vector3( j * Config.CELLSIZE, -2.5f - i * Config.CELLSIZE, 1);

                Cells[i, j] = obstacle;

            }

        }

        int counter=0;
        while (playerSpawns.Count != 0 && counter<Config.PlayerCount)
        {
            int index = Config.RND.Next(0,playerSpawns.Count);
            if (Players.Count <= counter)
            {
                Players.Add(Instantiate(playerPrefab, this.transform).GetComponent<Player>());
            }
            Players[counter].Init(MapEntityType.Player, this, playerSpawns[index]);
            Players[counter].gameObject.transform.localPosition = new Vector3(playerSpawns[index].Col * Config.CELLSIZE, - 2.5f - playerSpawns[index].Row * Config.CELLSIZE, 2);
            playerSpawns.RemoveAt(index);
            ++counter;
            
        }

        if (counter < Config.PlayerCount)
        {
            Debug.LogError("Invalid map, no place to spawn the players");
        }

        counter=0;
        while (monsterSpawns.Count != 0 && counter < Config.MonsterCount)
        {
            int index = Config.RND.Next(0, monsterSpawns.Count);
            if (Monsters.Count <= counter)
            {
                Monsters.Add(Instantiate(monsterPrefab, this.transform).GetComponent<Monster>());
            }
            Monsters[counter].Init(MapEntityType.Player, this, monsterSpawns[index]);
            Monsters[counter].gameObject.transform.localPosition = new Vector3( monsterSpawns[index].Col * Config.CELLSIZE,  -2.5f - monsterSpawns[index].Row * Config.CELLSIZE, 2);
            monsterSpawns.RemoveAt(index);
            ++counter;

        }

        if (counter < Config.MonsterCount)
        {
            Debug.LogError("Invalid map, no place to spawn the monsters");
        }

    }



    private void DecreaseCircle()
    {
    }

    public void Reset()
    {
    }
}