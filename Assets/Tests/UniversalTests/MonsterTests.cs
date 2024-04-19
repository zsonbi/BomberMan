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
    public class MonsterTests
    {

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
            if (gameBoard is not null)
                GameObject.Destroy(this.gameBoard.gameObject);
        }

        [EdgeCase]
        [UnityTest]
        public IEnumerator GhostWithBombs()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Ghost);
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapMonstersFree");

            List<Vector3> monsterPositions = new List<Vector3>();
            foreach (var monster in gameBoard.Monsters)
            {

                for (Direction i = 0; i <= Direction.Down; i++)
                {

                    gameBoard.SpawnBomb(Position.CreateCopyAndMoveDir(monster.CurrentBoardPos, i), 1, true);

                }
                monsterPositions.Add(new Vector3(monster.gameObject.transform.localPosition.x, monster.gameObject.transform.localPosition.y, monster.gameObject.transform.localPosition.z));

                monster.Init(MapEntityType.Monster,gameBoard,monster.CurrentBoardPos);
                }

            yield return new WaitForSeconds(5f);
            for (int i = 0; i < gameBoard.Monsters.Count; i++)
            {
                Assert.AreEqual(gameBoard.Monsters[i].gameObject.transform.localPosition, monsterPositions[i]);
            }

        }


    }
}