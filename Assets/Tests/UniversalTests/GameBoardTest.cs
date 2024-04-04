using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bomberman;
using DataTypes;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameBoardTest
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

        // Test if private the load private on startup is disabled as private it should be
        [UnityTest]
        public IEnumerator ManualLoadPasses()
        {
            GameBoard tmp = GameObject.Instantiate(gameBoardPrefab).GetComponent<GameBoard>();
            tmp.MakeMapLoadManual();
            yield return null;
            Assert.IsNull(tmp.Cells);
            // GameObject.Destroy(tmp);
        }

        // A simple load test
        [Test]
        public void GameBoardLoadPasses()
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
            //    string mapsPath = Directory.GetCurrentDirectory() + "/Assets/Maps/GameMaps/";
            //    string[] filePaths = Directory.GetFiles(mapsPath, "*.csv");
            TextAsset[] maps = Resources.LoadAll<TextAsset>("Maps/GameMaps/");

            foreach (var item in maps)
            {
                //Check so it doesn't contain \r
                Assert.IsFalse(item.text.Contains('\r'));

                Assert.IsTrue(validateMap(item.text.Trim('\n').Split('\n')));
            }
            yield return null;
        }
    }
}