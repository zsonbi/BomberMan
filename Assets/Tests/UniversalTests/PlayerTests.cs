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
    public class PlayerTests
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
            GameObject.Destroy(this.gameBoard.gameObject);
        }

        // Test if private the load private on startup is disabled as private it should be
        [UnityTest]
        public IEnumerator MonsterDeathAndPlayerDamageTest()
        {
            MainMenuConfig.Player3 = true;
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/playerNextToMonsterMap");
            gameBoard.Resume();
            List<int> healths = gameBoard.Players.Select(x => x.Hp).ToList();
            List<Position> playerSpawns = new List<Position>();

            foreach (var item in gameBoard.Players)
            {
                gameBoard.SpawnBomb(item.CurrentBoardPos, 2);
            }

            yield return null;

            for (int i = 0; i < healths.Count; i++)
            {
                Assert.AreNotEqual(healths[i], gameBoard.Players[i].Hp);
            }
            yield return null;
            Assert.AreNotEqual(gameBoard.Monsters.Count, gameBoard.Monsters.Count(x => x.Alive));
            yield return null;
            MainMenuConfig.Player3 = false;
        }

        //Test if every movingentity respects the walls
        [UnityTest]
        public IEnumerator TestPlayerAndMonsterWallDetection()
        {
            MainMenuConfig.Player3 = true;
            gameBoard.StartNextGame();
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck");
            List<Position> playerSpawns = new List<Position>();
            List<Position> monsterSpawns = new List<Position>();

            List<Vector3> playerStartVector3 = new List<Vector3>();
            List<Vector3> monsterStartVector3 = new List<Vector3>();

            foreach (var item in gameBoard.Players)
            {
                playerSpawns.Add(new Position(item.CurrentBoardPos.Row, item.CurrentBoardPos.Col));
                playerStartVector3.Add(new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z));
            }

            foreach (var item in gameBoard.Monsters)
            {
                monsterSpawns.Add(new Position(item.CurrentBoardPos.Row, item.CurrentBoardPos.Col));
                monsterStartVector3.Add(new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z));
            }

            yield return new WaitForSeconds(1);

            for (int i = 0; i < playerSpawns.Count; i++)
            {
                Assert.IsTrue(playerSpawns[i].Equals(gameBoard.Players[i].CurrentBoardPos));
                Assert.IsTrue(playerStartVector3[i].Equals(gameBoard.Players[i].transform.position));
            }

            for (int i = 0; i < monsterSpawns.Count; i++)
            {
                Assert.IsTrue(monsterSpawns[i].Equals(gameBoard.Monsters[i].CurrentBoardPos));
                Assert.IsTrue(monsterStartVector3[i].Equals(gameBoard.Monsters[i].transform.position));
            }
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.None);
        }

        /// <summary>
        /// Test it if the bomb also despawns on new game
        /// </summary>
        [EdgeCase]
        [UnityTest]
        public IEnumerator TestBombDespawnOnNewGame()
        {
            MainMenuConfig.Player3 = true;
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck");
            List<int> healths = gameBoard.Players.Select(x => x.Hp).ToList();

            foreach (var item in gameBoard.Players)
            {
                foreach (var control in item.Controls)
                {
                    if (control.Value.Method.Name == "PlaceBomb")
                    {
                        control.Value.Method.Invoke(item, null);
                        Assert.AreNotEqual(item.Bombs.Count, item.Bombs.Count(x => !x.Placed));
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck");

            yield return new WaitForSeconds(Config.BOMBBLOWTIME);

            for (int i = 0; i < healths.Count; i++)
            {
                Assert.AreEqual(healths[i], gameBoard.Players[i].Hp);
            }

            MainMenuConfig.Player3 = false;

            yield return null;
        }
    }
}