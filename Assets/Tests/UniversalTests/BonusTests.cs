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

        [UnityTest]
        public IEnumerator TestMultipleBonusesPickUp()
        {
            BonusType[] bonusTypesToTest = new BonusType[] { BonusType.BombRange, BonusType.BonusBomb };

            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;

            foreach (var item in bonusTypesToTest)
            {
                gameBoard.SpawnBonus(item, gameBoard.Players.First().CurrentBoardPos);
            }

            yield return new WaitForFixedUpdate();

            foreach (var item in bonusTypesToTest)
            {
                Assert.IsTrue(gameBoard.Players.First().Bonuses.ContainsKey(item));
                Assert.AreEqual(1, gameBoard.Players.First().Bonuses[BonusType.BombRange].Tier);
            }
        }

        [UnityTest]
        public IEnumerator TestNoBombBonus()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            gameBoard.SpawnBonus(BonusType.NoBomb, gameBoard.Players.First().CurrentBoardPos);
            float noBombLength = gameBoard.Entites[0].GetComponent<Bonus>().Duration;
            yield return new WaitForFixedUpdate();

            foreach (var control in gameBoard.Players[0].Controls)
            {
                if (control.Value.Method.Name == "PlaceBomb")
                {
                    control.Value.Method.Invoke(gameBoard.Players[0], null);
                    Assert.AreEqual(gameBoard.Players[0].Bombs.Count, gameBoard.Players[0].Bombs.Count(x => !x.Placed));
                }
            }
            yield return new WaitForSeconds(noBombLength + 0.1f);
            foreach (var control in gameBoard.Players[0].Controls)
            {
                if (control.Value.Method.Name == "PlaceBomb")
                {
                    control.Value.Method.Invoke(gameBoard.Players[0], null);
                    Assert.AreNotEqual(gameBoard.Players[0].Bombs.Count, gameBoard.Players[0].Bombs.Count(x => !x.Placed));
                }
            }
        }

        [UnityTest]
        public IEnumerator TestInstantBombBonus()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            gameBoard.SpawnBonus(BonusType.InstantBomb, gameBoard.Players.First().CurrentBoardPos);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(gameBoard.Players[0].Bombs.Count, gameBoard.Players[0].Bombs.Count(x => !x.Placed));
        }
    }
}