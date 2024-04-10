using DataTypes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bomberman
{
    namespace Menu
    {
        public class MenuController : MonoBehaviour
        {


            [SerializeField]
            PlayerInGameMenuHandler[] PlayerInGameMenuHandlers;

            public void Update()
            {
            }

            public void NewGame(List<Player> players)
            {

                for (int i = 0; i < players.Count; i++)
                {
                    if (PlayerInGameMenuHandlers.Length < players.Count)
                    {
                        Debug.LogError("Not enough PlayerInGameMenuHandlers for the players");
                        continue;
                    }
                    PlayerInGameMenuHandlers[i].SetUpPanel(players[i]);
                    PlayerInGameMenuHandlers[i].gameObject.SetActive(true);

                }
                for (int i = players.Count;i < PlayerInGameMenuHandlers.Length;i++)
                {
                    PlayerInGameMenuHandlers[i].gameObject.SetActive(false);
                }

            }

            public void RemoveHealth(Player player)
            {
                PlayerInGameMenuHandlers[player.PlayerId].RemoveHealth();
            }
           

            public void AddBonus(BonusType bonusType, Player player)
            {

            }

            public void RemoveBonus(BonusType bonusType, Player player)
            {

            }

        }
    }
}