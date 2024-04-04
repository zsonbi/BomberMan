using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    [SerializeField]
    private TextMeshProUGUI up, down, right, left, place;

    [SerializeField]
    private TextMeshProUGUI up2, down2, right2, left2, place2;

    private GameObject currentKey;

    private Color32 normal = new Color32(255, 255, 255, 255);
    private Color32 selected = new Color32(114, 112, 112, 255);

    private void Start()
    {
        keys.Add("UpButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpButton", "W")));
        keys.Add("DownButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownButton", "S")));
        keys.Add("RightButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButton", "D")));
        keys.Add("LeftButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButton", "A")));
        keys.Add("PlacingBombButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton", "Space")));

        keys.Add("UpButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpButton2", "UpArrow")));
        keys.Add("DownButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownButton2", "DownArrow")));
        keys.Add("RightButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButton2", "RightArrow")));
        keys.Add("LeftButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButton2", "LeftArrow")));
        keys.Add("PlacingBombButton2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton2", "RightShift")));

        up.text = keys["UpButton"].ToString();
        down.text = keys["DownButton"].ToString();
        right.text = keys["RightButton"].ToString();
        left.text = keys["LeftButton"].ToString();
        place.text = keys["PlacingBombButton"].ToString();

        up2.text = keys["UpButton2"].ToString();
        down2.text = keys["DownButton2"].ToString();
        right2.text = keys["RightButton2"].ToString();
        left2.text = keys["LeftButton2"].ToString();
        place2.text = keys["PlacingBombButton2"].ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(keys["UpButton"]))
        {
            // Do a move action
            Debug.Log("UpButton");
        }
        if (Input.GetKeyDown(keys["DownButton"]))
        {
            // Do a move action
            Debug.Log("DownButton");
        }
        if (Input.GetKeyDown(keys["PlacingBombButton"]))
        {
            // Do a move action
            Debug.Log("PlacingBombButton");
        }
        if (Input.GetKeyDown(keys["LeftButton"]))
        {
            // Do a move action
            Debug.Log("LeftButton");
        }
        if (Input.GetKeyDown(keys["RightButton"]))
        {
            // Do a move action
            Debug.Log("RightButton");
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
        keys["UpButton"] = KeyCode.W;
        keys["DownButton"] = KeyCode.S;
        keys["LeftButton"] = KeyCode.A;
        keys["RightButton"] = KeyCode.D;
        keys["PlacingBombButton"] = KeyCode.Space;

        keys["UpButton2"] = KeyCode.UpArrow;
        keys["DownButton2"] = KeyCode.DownArrow;
        keys["LeftButton2"] = KeyCode.LeftArrow;
        keys["RightButton2"] = KeyCode.RightArrow;
        keys["PlacingBombButton2"] = KeyCode.RightShift;

        up.text = keys["UpButton"].ToString();
        down.text = keys["DownButton"].ToString();
        right.text = keys["RightButton"].ToString();
        left.text = keys["LeftButton"].ToString();
        place.text = keys["PlacingBombButton"].ToString();

        up2.text = keys["UpButton2"].ToString();
        down2.text = keys["DownButton2"].ToString();
        right2.text = keys["RightButton2"].ToString();
        left2.text = keys["LeftButton2"].ToString();
        place2.text = keys["PlacingBombButton2"].ToString();
    }
}