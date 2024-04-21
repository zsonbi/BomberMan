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
    public class PersistanceTest
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
            if (File.Exists("./testSave.json"))
                File.Delete("./testSave.json");

            GameObject.Destroy(this.gameBoard.gameObject);
        }

        // Test if private the load private on startup is disabled as private it should be
        [UnityTest]
        public IEnumerator SaveAndLoadTest()
        {
            MainMenuConfig.Player3 = true;
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testEveryOneCanMove");
            gameBoard.Resume();
            yield return new WaitForSeconds(0.5f);
            gameBoard.Pause();
            List<Position> playerPositions = gameBoard.Players.Select(x => x.CurrentBoardPos).ToList();
            List<Position> monsterPositions = gameBoard.Monsters.Select(x => x.CurrentBoardPos).ToList();

            gameBoard.SaveState("testSave.json");

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testEveryOneCanMove");
            yield return new WaitForSeconds(0.1f);
            MainMenuConfig.Player3 = false;

            gameBoard.LoadState("./testSave.json");

            Assert.IsTrue(MainMenuConfig.Player3);

            for (int i = 0; i < playerPositions.Count; i++)
            {
                Assert.AreEqual(playerPositions[i], gameBoard.Players[i].CurrentBoardPos);
            }
            for (int i = 0; i < gameBoard.Monsters.Count; i++)
            {
                Assert.AreEqual(monsterPositions[i], gameBoard.Monsters[i].CurrentBoardPos);
            }
            MainMenuConfig.Player3 = false;
        }
    }
}