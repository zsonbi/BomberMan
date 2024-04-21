using System;
using System.Collections;
using System.Linq;
using Bomberman;
using DataTypes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Menu;

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
            if(gameBoard is not null)
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
                    break;
                }
            }
            yield return new WaitForSeconds(noBombLength + 0.1f);
            foreach (var control in gameBoard.Players[0].Controls)
            {
                if (control.Value.Method.Name == "PlaceBomb")
                {
                    control.Value.Method.Invoke(gameBoard.Players[0], null);
                    Assert.AreNotEqual(gameBoard.Players[0].Bombs.Count, gameBoard.Players[0].Bombs.Count(x => !x.Placed));
                    break;
                }
            }
        }

        [EdgeCase]
        [UnityTest]
        public IEnumerator TestNoBombAndInstantBombBonus()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            gameBoard.SpawnBonus(BonusType.NoBomb, gameBoard.Players.First().CurrentBoardPos);
            gameBoard.SpawnBonus(BonusType.InstantBomb, gameBoard.Players.First().CurrentBoardPos);
            float noBombLength = gameBoard.Entites[0].GetComponent<Bonus>().Duration;
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(1f);
       
            Assert.IsTrue(gameBoard.Players.First().Bombs.All(x=>!x.Placed));
        }


        [UnityTest]
        public IEnumerator TestSmallExplosion()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            float originalTimeToMove = gameBoard.Players.First().timeToMove;
            gameBoard.SpawnBonus(BonusType.SmallExplosion, gameBoard.Players.First().CurrentBoardPos);
            float smallExplosionDuration = gameBoard.Entites[0].GetComponent<Bonus>().Duration;
            gameBoard.SpawnBonus(BonusType.BombRange, gameBoard.Players.First().CurrentBoardPos);
            gameBoard.SpawnBonus(BonusType.BombRange, gameBoard.Players.First().CurrentBoardPos);
            gameBoard.SpawnBonus(BonusType.BombRange, gameBoard.Players.First().CurrentBoardPos);
            yield return new WaitForFixedUpdate();

            foreach (var control in gameBoard.Players[0].Controls)
            {
                if (control.Value.Method.Name == "PlaceBomb")
                {
                    control.Value.Method.Invoke(gameBoard.Players[0], null);
                    break;
                }
            }

            Assert.AreEqual(1,gameBoard.Players.First().Bombs[0].BlastRadius);

            yield return new WaitForSeconds(Mathf.Max(new float[]{ smallExplosionDuration, Config.BOMBBLOWTIME }) + 0.1f);
            foreach (var control in gameBoard.Players[0].Controls)
            {
                if (control.Value.Method.Name == "PlaceBomb")
                {
                    control.Value.Method.Invoke(gameBoard.Players[0], null);
                    break;
                }
            }

            Assert.AreEqual(4, gameBoard.Players.First().Bombs[0].BlastRadius);

        }

        [UnityTest]
        public IEnumerator TestSlowness()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            float originalTimeToMove = gameBoard.Players.First().timeToMove;
            gameBoard.SpawnBonus(BonusType.Slowness, gameBoard.Players.First().CurrentBoardPos);
            float slownessDuration = gameBoard.Entites[0].GetComponent<Bonus>().Duration;
            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(gameBoard.Players.First().timeToMove, originalTimeToMove);

            yield return new WaitForSeconds(slownessDuration + 0.1f);

            Assert.AreEqual(gameBoard.Players.First().timeToMove, originalTimeToMove);

        }

        [UnityTest]
        public IEnumerator TestDecayingStack()
        {
            MainMenuConfig.Player3 = false;
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);

            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            yield return null;
            float originalTimeToMove = gameBoard.Players.First().timeToMove;
            gameBoard.SpawnBonus(BonusType.Slowness, gameBoard.Players.First().CurrentBoardPos);
            float slownessDuration = gameBoard.Entites[0].GetComponent<Bonus>().Duration;
            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(gameBoard.Players.First().timeToMove, originalTimeToMove);

            yield return new WaitForSeconds(slownessDuration/2 + 0.1f);

            Assert.AreNotEqual(gameBoard.Players.First().timeToMove, originalTimeToMove);
            gameBoard.SpawnBonus(BonusType.Slowness, gameBoard.Players.First().CurrentBoardPos);
           yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(slownessDuration/2 + 0.1f);

            Assert.AreNotEqual(gameBoard.Players.First().timeToMove, originalTimeToMove);


            yield return null;
        }

        [UnityTest]
        public IEnumerator TestGhostBonus()
        {
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            Vector3 playerPos = gameBoard.Players.First().gameObject.transform.position;

            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(playerPos,gameBoard.Players.First().transform.position);

            gameBoard.SpawnBonus(BonusType.Ghost,gameBoard.Players.First().CurrentBoardPos);
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.1f);
            Assert.AreNotEqual(playerPos, gameBoard.Players.First().transform.position);
        }


        [UnityTest]
        public IEnumerator TestImmunityBonus()
        {
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            int playerHp = gameBoard.Players.First().Hp;
            gameBoard.SpawnBonus(BonusType.Immunity, gameBoard.Players.First().CurrentBoardPos);
            yield return new WaitForFixedUpdate();
            gameBoard.SpawnBomb(gameBoard.Players.First().CurrentBoardPos);

            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(playerHp, gameBoard.Players.First().Hp);

        }

        [UnityTest]
        public IEnumerator TestSkateBonus()
        {
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testEveryOneCanMove");
            float prevTimeToMove= gameBoard.Players.First().timeToMove;
            gameBoard.SpawnBonus(BonusType.Skate,gameBoard.Players.First().CurrentBoardPos);
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.2f);
            
            Assert.Less(gameBoard.Players.First().timeToMove, prevTimeToMove);
        }

        [EdgeCase]
        [UnityTest]
        public IEnumerator TestSkateAndSlownessBonus()
        {
            gameBoard.ForceSpecificMobTypeOnLoad(MonsterType.Basic);
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testMapEveryOneStuck2");
            float prevTimeToMove = gameBoard.Players.First().timeToMove;
            gameBoard.SpawnBonus(BonusType.Skate, gameBoard.Players.First().CurrentBoardPos);
            yield return new WaitForFixedUpdate();
            Assert.Less(gameBoard.Players.First().timeToMove, prevTimeToMove);
            float skateMoveTime = gameBoard.Players.First().timeToMove;
            gameBoard.SpawnBonus(BonusType.Slowness, gameBoard.Players.First().CurrentBoardPos);
            gameBoard.SpawnBonus(BonusType.Immunity, gameBoard.Players.First().CurrentBoardPos);
            gameBoard.SpawnBonus(BonusType.Immunity, gameBoard.Players[1].CurrentBoardPos);
            yield return new WaitForFixedUpdate();
            Assert.Less( prevTimeToMove, gameBoard.Players.First().timeToMove);
            yield return new WaitForSeconds(gameBoard.Players.First().Bonuses[BonusType.Slowness].Duration/2+0.1f);
            gameBoard.SpawnBonus(BonusType.Immunity, gameBoard.Players.First().CurrentBoardPos);
            gameBoard.SpawnBonus(BonusType.Immunity, gameBoard.Players[1].CurrentBoardPos);
            yield return new WaitForSeconds(gameBoard.Players.First().Bonuses[BonusType.Slowness].Duration+0.1f );
            Assert.AreEqual(gameBoard.Players.First().timeToMove, skateMoveTime);
        }

        [EdgeCase]
        [UnityTest]
        public IEnumerator TestObstacleBonusAndPlaceBomb()
        {
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testEveryOneCanMove");
            Player firstPlayer = gameBoard.Players.First();
            Action placeObstacleAction = null;
            Action placeBomb=null;
            foreach (var control in firstPlayer.Controls)
            {
                if (control.Value.Method.Name == "PlaceObstacle")
                {
                    placeObstacleAction = control.Value;
                }
                else if(control.Value.Method.Name == "PlaceBomb")
                {
                    placeBomb = control.Value;
                }
            }
            if (placeObstacleAction is null || placeBomb is null)
            {
                Assert.Fail();
            }

            placeObstacleAction.Method.Invoke(firstPlayer, null);

            Assert.IsFalse(gameBoard.Cells[firstPlayer.CurrentBoardPos.Row, firstPlayer.CurrentBoardPos.Col].Placed);

            Assert.AreEqual(0, firstPlayer.AvailableObstacle);
            gameBoard.SpawnBonus(BonusType.Obstacle, firstPlayer.CurrentBoardPos);

            yield return new WaitForFixedUpdate();
            Assert.AreEqual(3, firstPlayer.AvailableObstacle);

            placeObstacleAction.Method.Invoke(firstPlayer, null);
            Assert.AreEqual(2, firstPlayer.AvailableObstacle);
            placeBomb.Method.Invoke(firstPlayer, null);

            Assert.IsTrue(firstPlayer.Bombs.All(x=>!x.Placed));


        }

        [UnityTest]
        public IEnumerator TestObstacleBonus()
        {
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testEveryOneCanMove");
            Player firstPlayer = gameBoard.Players.First();
            Action placeObstacleAction=null;
            foreach (var control in firstPlayer.Controls)
            {
                if (control.Value.Method.Name == "PlaceObstacle")
                {
                   placeObstacleAction= control.Value;
                    break;
                }
            }
            if(placeObstacleAction is null)
            {
                Assert.Fail();
            }

            placeObstacleAction.Method.Invoke(firstPlayer,null);

            Assert.IsFalse(gameBoard.Cells[firstPlayer.CurrentBoardPos.Row,firstPlayer.CurrentBoardPos.Col].Placed);
           
            Assert.AreEqual(0, firstPlayer.AvailableObstacle);
            gameBoard.SpawnBonus(BonusType.Obstacle,firstPlayer.CurrentBoardPos);

            yield return new WaitForFixedUpdate();
            Assert.AreEqual(3, firstPlayer.AvailableObstacle);

            placeObstacleAction.Method.Invoke(firstPlayer, null);
            Assert.AreEqual(2, firstPlayer.AvailableObstacle);

            Assert.IsTrue(gameBoard.Cells[firstPlayer.CurrentBoardPos.Row, firstPlayer.CurrentBoardPos.Col].Placed);

            gameBoard.SpawnBonus(BonusType.Obstacle, gameBoard.Players.First().CurrentBoardPos);
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(5, firstPlayer.AvailableObstacle);


            gameBoard.SpawnBomb(new Position(firstPlayer.CurrentBoardPos.Row, firstPlayer.CurrentBoardPos.Col-1));
            Assert.AreEqual(6, firstPlayer.AvailableObstacle);
            Assert.AreEqual(1, gameBoard.Entites.Count);

        }


        [UnityTest]
        public IEnumerator TestDetonator()
        {
            gameBoard.StartNextGame();
            gameBoard.CreateBoard("Maps/TestMaps/testEveryOneCanMove");
            Player firstPlayer = gameBoard.Players.First();
            Action placeBombAction = null;
            foreach (var control in firstPlayer.Controls)
            {
                if (control.Value.Method.Name == "PlaceBomb")
                {
                    placeBombAction = control.Value;
                    break;
                }
            }
            if (placeBombAction is null)
            {
                Assert.Fail();
            }
            gameBoard.SpawnBonus(BonusType.Detonator, firstPlayer.CurrentBoardPos);
            yield return new WaitForFixedUpdate();
            
            
            placeBombAction.Method.Invoke(firstPlayer, null);
            
            yield return new WaitForSeconds(firstPlayer.Bombs.First().TimeTillBlow+0.1f+Config.BOMBEXPLOSIONSPREADSPEED);

            Assert.IsTrue(firstPlayer.Bombs.First().Placed);
            placeBombAction.Method.Invoke(firstPlayer, null);

            yield return new WaitForSeconds( 0.1f + Config.BOMBEXPLOSIONSPREADSPEED*(firstPlayer.Bombs.First().BlastRadius+1));

            Assert.IsFalse(firstPlayer.Bombs.First().Placed);


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