using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public TextMeshProUGUI up, down, right, left, place;

    private GameObject currentKey;

    private Color32 normal = new Color32(255,255,255,255);
    private Color32 selected = new Color32(114,112,112,255);

    void Start()
    {
        keys.Add("UpButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpButton","W")));
        keys.Add("DownButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownButton", "S")));
        keys.Add("RightButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightButton", "D")));
        keys.Add("LeftButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftButton", "A")));
        keys.Add("PlacingBombButton", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PlacingBombButton", "Space")));

        up.text = keys["UpButton"].ToString();
        down.text = keys["DownButton"].ToString();
        right.text = keys["RightButton"].ToString();
        left.text = keys["LeftButton"].ToString();
        place.text = keys["PlacingBombButton"].ToString();

    }

    // Update is called once per frame
    void Update()
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

    void OnGUI()
    {
        if(currentKey != null)
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
        if(currentKey != null)
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

		up.text = keys["UpButton"].ToString();
		down.text = keys["DownButton"].ToString();
		right.text = keys["RightButton"].ToString();
		left.text = keys["LeftButton"].ToString();
		place.text = keys["PlacingBombButton"].ToString();
	}
}
