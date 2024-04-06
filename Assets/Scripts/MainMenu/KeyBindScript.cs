using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] 
    private Dictionary<string, TextMeshProUGUI> buttons;

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    [Header("Player 1 buttons")]
    [SerializeField]
    private TextMeshProUGUI up, down, right, left, place, detonate, placeingObstacle;
    
    [Header("Player 2 buttons")]
    [SerializeField]
    private TextMeshProUGUI up2, down2, right2, left2, place2, detonate2, placeingObstacle2;
    
    [Header("Player 3 buttons")]
    [SerializeField]
    private TextMeshProUGUI up3, down3, right3, left3, place3, detonate3, placeingObstacle3;


    private GameObject currentKey;

    private Color32 normal = new Color32(255, 255, 255, 255);
    private Color32 selected = new Color32(114, 112, 112, 255);

    private void Start()
    {
        bool saveStuff = !PlayerPrefs.HasKey("UpButton0");

        keys.Add("UpButton0", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpButton0", "W")));
        keys.Add("DownButton0", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownButton0", "S")));
        keys.Add("RightButton0", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButton0", "D")));
        keys.Add("LeftButton0", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButton0", "A")));
        keys.Add("PlacingBombButton0", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton0", "Space")));
        keys.Add("DetonateButton0",  (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DetonateButton0", "LeftAlt")));
        keys.Add("PlacingObstacleButton0", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingObstacleButton0", KeyCode.X.ToString())));

        keys.Add("UpButton1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpButton1", "UpArrow")));
        keys.Add("DownButton1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownButton1", "DownArrow")));
        keys.Add("RightButton1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButton1", "RightArrow")));
        keys.Add("LeftButton1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButton1", "LeftArrow")));
        keys.Add("PlacingBombButton1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton1", "RightShift")));
        keys.Add("DetonateButton1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton1", KeyCode.RightControl.ToString())));
        keys.Add("PlacingObstacleButton1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingObstacleButton1", KeyCode.Return.ToString())));

        keys.Add("UpButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpButton2", KeyCode.Keypad8.ToString())));
        keys.Add("DownButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownButton2", KeyCode.Keypad5.ToString())));
        keys.Add("RightButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButton2", KeyCode.Keypad6.ToString())));
        keys.Add("LeftButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButton2", KeyCode.Keypad4.ToString())));
        keys.Add("PlacingBombButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton2", KeyCode.KeypadPlus.ToString())));
        keys.Add("DetonateButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton2", KeyCode.KeypadEnter.ToString())));
        keys.Add("PlacingObstacleButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingObstacleButton2", KeyCode.KeypadMinus.ToString())));

        UpdateLabels();

        if (saveStuff)
        {
            SaveKeys();
        }
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
        detonate.text = keys["DetonateButton0"].ToString();
        placeingObstacle.text = keys["PlacingObstacleButton0"].ToString();

        up2.text = keys["UpButton1"].ToString();
        down2.text = keys["DownButton1"].ToString();
        right2.text = keys["RightButton1"].ToString();
        left2.text = keys["LeftButton1"].ToString();
        place2.text = keys["PlacingBombButton1"].ToString();
        detonate2.text = keys["DetonateButton1"].ToString();
        placeingObstacle2.text = keys["PlacingObstacleButton1"].ToString();

        up3.text = keys["UpButton2"].ToString();
        down3.text = keys["DownButton2"].ToString();
        right3.text = keys["RightButton2"].ToString();
        left3.text = keys["LeftButton2"].ToString();
        place3.text = keys["PlacingBombButton2"].ToString();
        detonate3.text = keys["DetonateButton2"].ToString();
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
        keys["UpButton0"] = KeyCode.W;
        keys["DownButton0"] = KeyCode.S;
        keys["LeftButton0"] = KeyCode.A;
        keys["RightButton0"] = KeyCode.D;
        keys["PlacingBombButton0"] = KeyCode.Space;
        keys["DetonateButton0"]=KeyCode.LeftAlt;
        keys["PlacingObstacleButton0"]=KeyCode.X;

        keys["UpButton1"] = KeyCode.UpArrow;
        keys["DownButton1"] = KeyCode.DownArrow;
        keys["LeftButton1"] = KeyCode.LeftArrow;
        keys["RightButton1"] = KeyCode.RightArrow;
        keys["PlacingBombButton1"] = KeyCode.RightShift;
        keys["DetonateButton1"] = KeyCode.RightControl;
        keys["PlacingObstacleButton1"] = KeyCode.Return;

        keys["UpButton2"] = KeyCode.Keypad8;
        keys["DownButton2"] = KeyCode.Keypad5;
        keys["LeftButton2"] = KeyCode.Keypad4;
        keys["RightButton2"] = KeyCode.Keypad6;
        keys["PlacingBombButton2"] = KeyCode.KeypadPlus;
        keys["DetonateButton2"] = KeyCode.KeypadEnter;
        keys["PlacingObstacleButton2"] = KeyCode.KeypadMinus;


        UpdateLabels();
    }
}