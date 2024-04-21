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
using Menu;

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

        [UnityTest]
        public IEnumerator MonsterDeath()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapMonstersFree");
            foreach (var monster in gameBoard.Monsters)
            {

                for (Direction i = 0; i <= Direction.Down; i++)
                {

                    gameBoard.SpawnBomb(Position.CreateCopyAndMoveDir(monster.CurrentBoardPos, i), 1);

                }

            }

            yield return new WaitForFixedUpdate();
            for (int i = 0; i < gameBoard.Monsters.Count; i++)
            {
                Assert.IsFalse(gameBoard.Monsters[i].Alive);
            }
        }

        [UnityTest]
        public IEnumerator PlayerAttackByStalker()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.StartNextGame();
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Stalker);
            gameBoard.CreateBoard("Maps/TestMaps/LabirintForMonsters");
            int[] originalHealths=gameBoard.Players.Select(x=>x.Hp).ToArray();
            yield return new WaitForSeconds(15);
            bool allEqual=true;
            for (int i = 0; i < originalHealths.Length; i++)
            {
                allEqual=allEqual && originalHealths[i] == gameBoard.Players[i].Hp;
            }
            Assert.IsFalse(allEqual);
        }

        [UnityTest]
        public IEnumerator PlayerAttackBySmarty()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.StartNextGame();
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Smarty);
            gameBoard.CreateBoard("Maps/TestMaps/LabirintForMonsters");
            int[] originalHealths = gameBoard.Players.Select(x => x.Hp).ToArray();
            yield return new WaitForSeconds(15);
            bool allEqual = true;
            for (int i = 0; i < originalHealths.Length; i++)
            {
                allEqual = allEqual && originalHealths[i] == gameBoard.Players[i].Hp;
            }
            Assert.IsFalse(allEqual);
        }

    }
}