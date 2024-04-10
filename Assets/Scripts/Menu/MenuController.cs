using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberman
{
    namespace Menu
    {
        public class MenuController : MonoBehaviour
        {
            [SerializeField]
            private GameBoard Game;

            [SerializeField]
            PlayerInGameMenuHandler[] PlayerInGameMenuHandlers;

            public void Update()
            {
                PlayerInGameMenuHandlers[0].SetUpPanel(Game.Players[0]);   
            }

            public void NewGame()
            {

                throw new System.NotImplementedException();
            }

            public void NewGame(int playerCount)
            {
                throw new System.NotImplementedException();
            }

            public bool AlterPlayerControls(int playerId)
            {
                throw new System.NotImplementedException();
            }

        }
    }
}