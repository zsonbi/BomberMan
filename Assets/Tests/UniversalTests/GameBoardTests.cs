using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bomberman;
using DataTypes;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameBoardTests
    {
        [SerializeField]
        private GameObject gameBoardPrefab = Resources.Load<GameObject>("Prefabs/GameBoardPrefab");

        private GameBoard gameBoard;

        [SetUp]
        public void Init()
        {
            this.gameBoard = GameObject.Instantiate(gameBoardPrefab).GetComponent<GameBoard>();
            this.gameBoard.MakeMapLoadManual();
        }

        [TearDown]
        public void Shutdown()
        {
            if(gameBoard is not null)
            GameObject.Destroy(this.gameBoard.gameObject);
        }

        // Test if private the load private on startup is disabled as private it should be
        [UnityTest]
        public IEnumerator ManualLoadPasses()
        {
            //  GameBoard tmp = GameObject.Instantiate(gameBoardPrefab).GetComponent<GameBoard>();
            gameBoard.MakeMapLoadManual();
            yield return null;
            Assert.IsNull(gameBoard.Cells);
        }

        // A simple load test
        [Test]
        public void GameBoardLoadTest()
        {
            gameBoard.CreateBoard("Maps/TestMaps/testMap1");

            Assert.IsNotNull(gameBoard.Cells);
            Assert.AreEqual(20, gameBoard.Cells.GetLength(0));
            Assert.AreEqual(20, gameBoard.Cells.GetLength(1));
        }

        [Test]
        public void MultipleMapLoadTest()
        {
            gameBoard.CreateBoard("Maps/TestMaps/testMap1");

            Assert.IsNotNull(gameBoard.Cells);
            Assert.AreEqual(20, gameBoard.Cells.GetLength(0));
            Assert.AreEqual(20, gameBoard.Cells.GetLength(1));
            Assert.IsFalse(gameBoard.Cells[0, 0].Destructible);

            gameBoard.CreateBoard("Maps/TestMaps/testMap2");
            Assert.IsNotNull(gameBoard.Cells);
            Assert.AreEqual(20, gameBoard.Cells.GetLength(0));
            Assert.AreEqual(20, gameBoard.Cells.GetLength(1));
            Assert.IsTrue(gameBoard.Cells[0, 0].Destructible);
        }

        //Validate the given map true-it is good
        private bool validateMap(string[] lines)
        {
            const int minPlayerSpawnCount = 3;
            const int matrixSize = 20;

            int playerSpawns = minPlayerSpawnCount;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] splitted = lines[i].Split(Config.CSVDELIMITER);
                if (splitted.Length != matrixSize)
                {
                    return false;
                }
                foreach (string item in splitted)
                    switch ((MapCell)Convert.ToByte(item))
                    {
                        case MapCell.PlayerSpawn:
                            --playerSpawns;
                            break;

                        default:
                            break;
                    }
            }

            return playerSpawns <= 0 && lines.Length == matrixSize;
        }

        //Test the maps which will be loaded
        [UnityTest]
        public IEnumerator TestMaps()
        {
            TextAsset[] maps = Resources.LoadAll<TextAsset>("Maps/GameMaps/");

            foreach (var item in maps)
            {
                Assert.IsTrue(validateMap(item.text.Trim('\n').Replace("\r", "").Split('\n')));
            }
            yield return null;
        }

        //Test the pause for the game
        [UnityTest]
        public IEnumerator TestPause()
        {
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testEveryOneCanMove");
            List<Vector3> playerStartVector3 = new List<Vector3>();
            List<Vector3> monsterStartVector3 = new List<Vector3>();
            foreach (var item in gameBoard.Players)
            {
                playerStartVector3.Add(new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z));
            }

            foreach (var item in gameBoard.Monsters)
            {
                monsterStartVector3.Add(new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z));
            }

            yield return new WaitForSeconds(0.2f);

            for (int i = 0; i < gameBoard.Players.Count; i++)
            {
                Assert.IsFalse(playerStartVector3[i].Equals(gameBoard.Players[i].transform.position));
                playerStartVector3[i] = new Vector3(gameBoard.Players[i].transform.position.x, gameBoard.Players[i].transform.position.y, gameBoard.Players[i].transform.position.z);
            }
            for (int i = 0; i < gameBoard.Monsters.Count; i++)
            {
                Assert.IsFalse(monsterStartVector3[i].Equals(gameBoard.Monsters[i].transform.position));
                monsterStartVector3[i] = new Vector3(gameBoard.Monsters[i].transform.position.x, gameBoard.Monsters[i].transform.position.y, gameBoard.Monsters[i].transform.position.z);
            }

            gameBoard.Pause();
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < gameBoard.Players.Count; i++)
            {
                Assert.IsTrue(playerStartVector3[i].Equals(gameBoard.Players[i].transform.position));
            }
            for (int i = 0; i < gameBoard.Monsters.Count; i++)
            {
                Assert.IsTrue(monsterStartVector3[i].Equals(gameBoard.Monsters[i].transform.position));
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator WinTest2Players()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            int healthCount = this.gameBoard.Players.First().Hp;

            for (int i = 0; i <= healthCount; i++)
            {
                gameBoard.SpawnBomb(this.gameBoard.Players.First().CurrentBoardPos, 2);
                yield return new WaitForSeconds(Config.IMMUNETIME + 0.01f);
            }
            yield return new WaitForSeconds(Config.GAME_OVER_TIMER + 0.01f);
            Assert.AreEqual(1, this.gameBoard.Players[1].Score);
        }

        [UnityTest]
        public IEnumerator WinTest3Players()
        {
            MainMenuConfig.Player3 = true;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            int healthCount = this.gameBoard.Players.First().Hp;

            for (int i = 0; i <= healthCount; i++)
            {
                foreach (var item in gameBoard.Players.Skip(1))
                {
                    gameBoard.SpawnBomb(item.CurrentBoardPos, 2);
                };
                yield return new WaitForSeconds(Config.IMMUNETIME + 0.01f);
            }
            yield return new WaitForSeconds(Config.GAME_OVER_TIMER - Config.IMMUNETIME + 0.01f);
            Assert.AreEqual(1, this.gameBoard.Players.First().Score);

            MainMenuConfig.Player3 = false;
        }

        [EdgeCase]
        [UnityTest]
        public IEnumerator DrawTest3Players()
        {
            MainMenuConfig.Player3 = true;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            int healthCount = this.gameBoard.Players.First().Hp;

            for (int i = 0; i <= healthCount; i++)
            {
                foreach (var item in gameBoard.Players)
                {
                    gameBoard.SpawnBomb(item.CurrentBoardPos, 2);
                };
                yield return new WaitForSeconds(Config.IMMUNETIME + 0.01f);
            }
            yield return new WaitForSeconds(Config.GAME_OVER_TIMER - Config.IMMUNETIME + 0.01f);

            foreach (var player in this.gameBoard.Players)
            {
                Assert.AreEqual(0, player.Score);
            }

            MainMenuConfig.Player3 = false;
        }

        [UnityTest]
        public IEnumerator Player1WinGameTest()
        {
            MainMenuConfig.Player3 = false;
            
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");

            for (int i = 0; i < MainMenuConfig.RequiredPoint; i++)
            {
                gameBoard.Players[1].InstantKill();

                yield return new WaitForSeconds(Config.GAME_OVER_TIMER + 0.01f);
                Assert.AreEqual(i+1, gameBoard.Players.First().Score);
                gameBoard.StartNextGame();
                gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
                gameBoard.Resume();
                yield return null;
            }

            Assert.AreEqual(MainMenuConfig.RequiredPoint, gameBoard.Players.First().Score);


            yield return null;
        }


    }
}