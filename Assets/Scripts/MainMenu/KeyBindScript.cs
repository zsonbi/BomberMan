using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Bomberman;

namespace Menu
{
    public class KeyBindScript : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private Dictionary<string, TextMeshProUGUI> buttons;

        private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

        [Header("Player 1 buttons")]
        [SerializeField]
        private TextMeshProUGUI up, down, right, left, place, placeingObstacle;

        [Header("Player 2 buttons")]
        [SerializeField]
        private TextMeshProUGUI up2, down2, right2, left2, place2, placeingObstacle2;

        [Header("Player 3 buttons")]
        [SerializeField]
        private TextMeshProUGUI up3, down3, right3, left3, place3, placeingObstacle3;


        private GameObject currentKey;

        private Color32 normal = new Color32(255, 255, 255, 255);
        private Color32 selected = new Color32(114, 112, 112, 255);

        private void Start()
        {

            for (int i = 0; i < 3; i++)
            {
                keys.Add("UpButton" + i, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpButton" + i, Config.PLAYERDEFAULTKEYS[i, 0].ToString())));
                keys.Add("DownButton" + i, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownButton" + i, Config.PLAYERDEFAULTKEYS[i, 1].ToString())));
                keys.Add("RightButton" + i, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButton" + i, Config.PLAYERDEFAULTKEYS[i, 2].ToString())));
                keys.Add("LeftButton" + i, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButton" + i, Config.PLAYERDEFAULTKEYS[i, 3].ToString())));
                keys.Add("PlacingBombButton" + i, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton" + i, Config.PLAYERDEFAULTKEYS[i, 4].ToString())));
                keys.Add("PlacingObstacleButton" + i, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingObstacleButton" + i, Config.PLAYERDEFAULTKEYS[i, 5].ToString())));

            }
            UpdateLabels();


        }



        private void OnGUI()
        {
            if (currentKey != null)
            {
                Event e = Event.current;
                if (e.isKey)
                {
                    keys[currentKey.name] = e.keyCode;
                    currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = e.keyCode.ToString();
                    currentKey.GetComponent<Image>().color = normal;
                    currentKey = null;
                }
            }
        }

        private void UpdateLabels()
        {
            up.text = keys["UpButton0"].ToString();
            down.text = keys["DownButton0"].ToString();
            right.text = keys["RightButton0"].ToString();
            left.text = keys["LeftButton0"].ToString();
            place.text = keys["PlacingBombButton0"].ToString();
            placeingObstacle.text = keys["PlacingObstacleButton0"].ToString();

            up2.text = keys["UpButton1"].ToString();
            down2.text = keys["DownButton1"].ToString();
            right2.text = keys["RightButton1"].ToString();
            left2.text = keys["LeftButton1"].ToString();
            place2.text = keys["PlacingBombButton1"].ToString();
            placeingObstacle2.text = keys["PlacingObstacleButton1"].ToString();

            up3.text = keys["UpButton2"].ToString();
            down3.text = keys["DownButton2"].ToString();
            right3.text = keys["RightButton2"].ToString();
            left3.text = keys["LeftButton2"].ToString();
            place3.text = keys["PlacingBombButton2"].ToString();
            placeingObstacle3.text = keys["PlacingObstacleButton2"].ToString();
        }

        public void ChangeKey(GameObject clicked)
        {
            if (currentKey != null)
            {
                currentKey.GetComponent<Image>().color = normal;
            }

            currentKey = clicked;
            currentKey.GetComponent<Image>().color = selected;
        }

        public void SaveKeys()
        {
            foreach (var key in keys)
            {
                PlayerPrefs.SetString(key.Key, key.Value.ToString());
            }
            PlayerPrefs.Save();
        }

        public void ResetSettings()
        {
            for (int i = 0; i < 3; i++)
            {
                keys["UpButton" + i] = Config.PLAYERDEFAULTKEYS[i, 0];
                keys["DownButton" + i] = Config.PLAYERDEFAULTKEYS[i, 1];
                keys["RightButton" + i] = Config.PLAYERDEFAULTKEYS[i, 2];
                keys["LeftButton" + i] = Config.PLAYERDEFAULTKEYS[i, 3];
                keys["PlacingBombButton" + i] = Config.PLAYERDEFAULTKEYS[i, 4];
                keys["PlacingObstacleButton" + i] = Config.PLAYERDEFAULTKEYS[i, 5];
            }

            UpdateLabels();
        }
    }
}