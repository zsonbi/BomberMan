using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Menu
{
    /// <summary>
    /// Handles the saves display
    /// </summary>
    public class SavesMenu : MonoBehaviour
    {
        /// <summary>
        /// The saves which are currently on the device
        /// </summary>
        private string[] files;

        /// <summary>
        /// A button prefab
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button buttonPrefab;
 
        /// <summary>
        /// The save file buttons
        /// </summary>
        private List<UnityEngine.UI.Button> buttons = new List<UnityEngine.UI.Button>();

        /// <summary>
        /// Reset the buttons
        /// </summary>
        private void ResetButtons()
        {
            while (buttons.Count != 0)
            {
                GameObject.Destroy(buttons[0].gameObject);
                buttons.RemoveAt(0);
            }
        }

        /// <summary>
        /// Show the save menu
        /// </summary>
        public void Show()
        {
            if (!Directory.Exists("gameSaves"))
            {
                Directory.CreateDirectory("gameSaves");
            }
            files = Directory.GetFiles("gameSaves");

            this.gameObject.transform.parent.transform.parent.gameObject.SetActive(true);
            AddButtons();
        }

        /// <summary>
        /// Hides the save menu
        /// </summary>
        public void Hide()
        {
            this.gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
        }

        /// <summary>
        /// When the button is pressed
        /// </summary>
        /// <param name="gameObjectClicked">Which button was pressed</param>
        public void ButtonEvent(GameObject gameObjectClicked)
        {
            int index = -1;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].gameObject == gameObjectClicked)
                {
                    index = i; break;
                }
            }

            MainMenuConfig.saveFilePath = buttons[index].GetComponentInChildren<TMPro.TMP_Text>().text;

            SceneManager.LoadSceneAsync("BombermanScene");
        }

        /// <summary>
        /// Adds the buttons
        /// Only the latest 7 saves will be displayed
        /// </summary>
        private void AddButtons()
        {
            ResetButtons();

            foreach (var file in files.Skip(files.Length - 7))
            {
                UnityEngine.UI.Button button = Instantiate(buttonPrefab, this.gameObject.transform).GetComponent<UnityEngine.UI.Button>();

                button.GetComponentInChildren<TMPro.TMP_Text>().text = file;
                button.gameObject.SetActive(true);
                buttons.Add(button);
            }
        }
    }
}
