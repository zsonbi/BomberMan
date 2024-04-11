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
    public class BonusTests
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

        [UnityTest]
        public IEnumerator PickUpBonusTest()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            gameBoard.SpawnBonus(BonusType.BombRange, gameBoard.Players.First().CurrentBoardPos);
            yield return new WaitForFixedUpdate();

            Assert.IsTrue(gameBoard.Players.First().Bonuses.ContainsKey(BonusType.BombRange));
        }

        [UnityTest]
        public IEnumerator BonusStackTest()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            gameBoard.SpawnBonus(BonusType.BombRange, gameBoard.Players.First().CurrentBoardPos);
            yield return new WaitForFixedUpdate();

            Assert.IsTrue(gameBoard.Players.First().Bonuses.ContainsKey(BonusType.BombRange));
            Assert.AreEqual(1, gameBoard.Players.First().Bonuses[BonusType.BombRange].Tier);

            for (int i = 2; i < 10; i++)
            {
                gameBoard.SpawnBonus(BonusType.BombRange, gameBoard.Players.First().CurrentBoardPos);
                yield return new WaitForFixedUpdate();

                if (BonusConfigs.EXTRA_BOMB_MAX_TIER <= i)
                {
                    Assert.AreEqual(BonusConfigs.EXTRA_BOMB_MAX_TIER, gameBoard.Players.First().Bonuses[BonusType.BombRange].Tier);
                }
                else
                {
                    Assert.AreEqual(i, gameBoard.Players.First().Bonuses[BonusType.BombRange].Tier);
                }
            }
        }
    }
}