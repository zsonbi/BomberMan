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
using UnityEngine.UI;
using Menu;
namespace Tests
{
    public class MainMenuTests
    {

        private GameObject mainMenuPrefab = Resources.Load<GameObject>("Prefabs/MainMenuPrefab");

        private MainMenu mainMenu;

        [SetUp]
        public void Init()
        {
            this.mainMenu = GameObject.Instantiate(mainMenuPrefab).GetComponent<MainMenu>();
        }

        [TearDown]
        public void Shutdown()
        {
            if (mainMenu.gameObject is not null)
                GameObject.Destroy(this.mainMenu.gameObject);
        }


        [UnityTest]
        public IEnumerator SkinSelectorTest()
        {
            Image imgComp = new GameObject("Player1Skin", typeof(Image)).GetComponent<Image>();


            string prevSkin = MainMenuConfig.PlayerSkins[0];

            mainMenu.NextSkinButton(imgComp);

            Assert.AreNotEqual(prevSkin, MainMenuConfig.PlayerSkins[0]);

            mainMenu.PrevSkinButton(imgComp);

            Assert.AreEqual(prevSkin, MainMenuConfig.PlayerSkins[0]);

            GameObject.Destroy(imgComp.gameObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Player3EnableTest()
        {
            mainMenu.ReadMorePlayerOption(true);
            Assert.IsTrue(MainMenuConfig.Player3);
            mainMenu.ReadMorePlayerOption(false);
            Assert.IsFalse(MainMenuConfig.Player3);
            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayerNamesTest()
        {

            mainMenu.ReadPlayer1Name("xd0");
            mainMenu.ReadPlayer2Name("xd1");
            mainMenu.ReadPlayer3Name("xd2");
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(MainMenuConfig.PlayerNames[i], "xd" + i);
            }
            yield return null;
        }

        [UnityTest]
        public IEnumerator RequiredScoreAndBattleRoyaleTest()
        {
            mainMenu.ReadRequiredPoints("5");
            Assert.AreEqual(5,MainMenuConfig.RequiredPoint);
            mainMenu.ReadRequiredPoints("1");
            Assert.AreEqual(1, MainMenuConfig.RequiredPoint);
            mainMenu.ReadRequiredPoints("");
            Assert.AreEqual(3, MainMenuConfig.RequiredPoint);
            mainMenu.ReadBattleRoyaleGameMode(true);

            Assert.IsTrue(MainMenuConfig.BattleRoyale);

            mainMenu.ReadBattleRoyaleGameMode(false);

            Assert.IsFalse(MainMenuConfig.BattleRoyale);

            yield return null;
        }

    }
}
