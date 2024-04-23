using DataTypes;
using System.Collections.Generic;
using UnityEngine;
using Bomberman;
namespace Menu
{
    /// <summary>
    /// Manages the in game UI
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        /// <summary>
        /// The game's UI left menu panel
        /// </summary>
        [SerializeField]
        private RectTransform leftMenu;

        /// <summary>
        /// The game's UI right menu panel
        /// </summary>
        [SerializeField]
        private RectTransform rightMenu;

        /// <summary>
        /// The gameboard's transform
        /// </summary>
        [SerializeField]
        private RectTransform GameBoard;

        /// <summary>
        /// Reference to the ingame menu controller
        /// </summary>
        [SerializeField]
        private PlayerInGameMenuHandler[] PlayerInGameMenuHandlers;

        /// <summary>
        /// Called on first frame
        /// </summary>
        private void Start()
        {
            //Cap the fps to 60
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        }

        /// <summary>
        /// Call it when new game is started
        /// Resets the game UI
        /// </summary>
        /// <param name="players">The players of the game</param>
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
            for (int i = players.Count; i < PlayerInGameMenuHandlers.Length; i++)
            {
                PlayerInGameMenuHandlers[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Remove a health icon from the player
        /// </summary>
        /// <param name="player">The player whos health we want to remove</param>
        public void RemoveHealth(Player player)
        {
            PlayerInGameMenuHandlers[player.PlayerId].RemoveHealth();
        }

        /// <summary>
        /// Adds a bonus icon to the player's ui display
        /// </summary>
        /// <param name="bonusType">The type of the bonus</param>
        /// <param name="player">Which player we want to add the bonus</param>
        public void AddBonus(BonusType bonusType, Player player)
        {
            PlayerInGameMenuHandlers[player.PlayerId].AddBonus(bonusType, player.Bonuses[bonusType].GetComponent<SpriteRenderer>().sprite);
        }
        /// <summary>
        /// Removes a bonus icon to the player's ui display
        /// </summary>
        /// <param name="bonusType">The type of the bonus</param>
        /// <param name="player">Which player we want to remove the bonus from</param>
        public void RemoveBonus(BonusType bonusType, Player player)
        {
            PlayerInGameMenuHandlers[player.PlayerId].RemoveBonus(bonusType);
        }
    }
}
