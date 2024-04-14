﻿using System;
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
            GameObject.Destroy(this.mainMenu.gameObject);
        }

        [UnityTest]
        public void StartNewGameTest()
        {

        }

    }
}