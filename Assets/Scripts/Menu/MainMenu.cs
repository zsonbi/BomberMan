using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

namespace Menu
{
    /// <summary>
    /// Handles the game's main menu
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        private Sprite[] playerSkins;
        private int[] skinIds;

        [SerializeField]
        private UnityEngine.UI.Image[] skinRenderers;

        [SerializeField]
        private TMP_InputField requiredPointsText;

        [SerializeField]
        private UnityEngine.UI.Toggle isPlayer3;

        [SerializeField]
        private UnityEngine.UI.Toggle isBattleRoyale;

        [SerializeField]
        private TMP_InputField[] playerNames = new TMP_InputField[0];

        /// <summary>
        /// Runs on first frame to load the necessary things
        /// </summary>
        private void Start()
        {
            playerSkins = Resources.LoadAll<Sprite>("PlayerSkins");
            skinIds = new int[3];

            if (requiredPointsText is not null)
                requiredPointsText.text = (MainMenuConfig.RequiredPoint.ToString());

            if (isPlayer3 is not null)
                isPlayer3.isOn = MainMenuConfig.Player3;

            if (isBattleRoyale is not null)
                isBattleRoyale.isOn = MainMenuConfig.BattleRoyale;

            for (int i = 0; i < playerNames.Length; i++)
            {
                if (playerNames[i] is not null)
                {
                    playerNames[i].text = (MainMenuConfig.PlayerNames[i]);
                }
            }

            //Get which skin is which Id
            for (int i = 0; i < playerSkins.Length; i++)
            {
                for (int j = 0; j < MainMenuConfig.PlayerSkins.Length; j++)
                {
                    if (playerSkins[i].name == MainMenuConfig.PlayerSkins[j])
                    {
                        skinIds[j] = i;
                        skinRenderers[j].sprite = playerSkins[i];
                    }
                }
            }
        }

        /// <summary>
        /// Select the next skin
        /// </summary>
        /// <param name="parent">The image object to set the new skin for display</param>
        public void NextSkinButton(UnityEngine.UI.Image parent)
        {
            int id = -1;
            switch (parent.name)
            {
                case "Player1Skin":
                    id = 0;
                    break;

                case "Player2Skin":
                    id = 1;
                    break;

                case "Player3Skin":
                    id = 2;
                    break;

                default:
                    Debug.LogError("No such thing as: " + parent.name + " in the switch");
                    return;
            }

            skinIds[id] = (skinIds[id] + 1) % playerSkins.Length;

            parent.sprite = playerSkins[skinIds[id]];
            MainMenuConfig.PlayerSkins[id] = playerSkins[skinIds[id]].name;
        }

        /// <summary>
        /// Select the previous skin
        /// </summary>
        /// <param name="parent">The image object to set the new skin for display</param>
        public void PrevSkinButton(UnityEngine.UI.Image parent)
        {
            int id = -1;
            switch (parent.name)
            {
                case "Player1Skin":
                    id = 0;
                    break;

                case "Player2Skin":
                    id = 1;
                    break;

                case "Player3Skin":
                    id = 2;
                    break;

                default:
                    Debug.LogError("No such thing as: " + parent.name + " in the switch");
                    return;
            }

            skinIds[id] = (skinIds[id] - 1);
            if (skinIds[id] < 0)
            {
                skinIds[id] = playerSkins.Length - 1;
            }
            parent.sprite = playerSkins[skinIds[id]];
            MainMenuConfig.PlayerSkins[id] = playerSkins[skinIds[id]].name;
        }

        /// <summary>
        /// Starting the game by switching between the two scene
        /// </summary>
        public void PlayGame()
        {
            //Every field is filled with valid informations
            if (MainMenuConfig.RequiredPoint > 0)
            {
                SceneManager.LoadSceneAsync("BombermanScene");
            }
            else
            {
                //Error message: Less input or not good syntax
                Debug.Log("Not enough input or bad syntax");
            }
        }

        /// <summary>
        /// Quitting the game
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Read if 3 player game mode is choosen or not
        /// </summary>
        /// <param name="selected"></param>
        public void ReadMorePlayerOption(bool selected)
        {
            if (selected)
            {
                MainMenuConfig.Player3 = true;
                Debug.Log("3 player mode is choosen");
            }
            else
            {
                MainMenuConfig.Player3 = false;
                Debug.Log("3 player mode is not choosen");
            }
        }

        /// <summary>
        /// Reads in the player1's name
        /// </summary>
        /// <param name="name">The name of the player</param>
        public void ReadPlayer1Name(string name)
        {
            MainMenuConfig.PlayerNames[0] = name;
            Debug.Log("Player 1 name: " + MainMenuConfig.PlayerNames[0]);
        }

        /// <summary>
        /// Reads in the player2's name
        /// </summary>
        /// <param name="name">The name of the player</param>
        public void ReadPlayer2Name(string name)
        {
            MainMenuConfig.PlayerNames[1] = name;
            Debug.Log("Player 2 name: " + MainMenuConfig.PlayerNames[1]);
        }

        /// <summary>
        /// Reads in the player3's name
        /// </summary>
        /// <param name="name">The name of the player</param>
        public void ReadPlayer3Name(string name)
        {
            MainMenuConfig.PlayerNames[2] = name;
            Debug.Log("Player 3 name: " + MainMenuConfig.PlayerNames[2]);
        }

        /// <summary>
        /// Read if Battle Royale game mode is choosen or not
        /// </summary>
        /// <param name="selected"></param>
        public void ReadBattleRoyaleGameMode(bool selected)
        {
            if (selected)
            {
                MainMenuConfig.BattleRoyale = true;
                Debug.Log("Battle Royale Game mode is choosen");
            }
            else
            {
                MainMenuConfig.BattleRoyale = false;
                Debug.Log("Battle Royale Game mode is not choosen");
            }
        }

        /// <summary>
        /// Reading the required points to win the game
        /// </summary>
        /// <param name="points"></param>
        public void ReadRequiredPoints(string points)
        {
            if (points == "")
            {
                MainMenuConfig.RequiredPoint = 3;
                return;
            }
            MainMenuConfig.RequiredPoint = int.Parse(points);
            Debug.Log("Required points to win the game: " + MainMenuConfig.RequiredPoint);
        }
    }
}