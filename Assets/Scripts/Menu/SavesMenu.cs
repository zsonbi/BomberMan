using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Menu { 
public class SavesMenu : MonoBehaviour
{
    private string[] files;

    [SerializeField]
    private UnityEngine.UI.Button buttonPrefab;

    public EventHandler LoadFileEventHandler;
    private List<UnityEngine.UI.Button> buttons = new List<UnityEngine.UI.Button>();

    private void ResetButtons()
    {
        while (buttons.Count != 0)
        {
            GameObject.Destroy(buttons[0].gameObject);
            buttons.RemoveAt(0);
        }
    }

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

    public void Hide()
    {
        this.gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
    }

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

        MainMenuConfig.mapPathToLoad = buttons[index].GetComponentInChildren<TMPro.TMP_Text>().text;

        SceneManager.LoadSceneAsync("BombermanScene");
    }

    private void AddButtons()
    {
        ResetButtons();

        foreach (var file in files.Skip(files.Length-7))
        {
            UnityEngine.UI.Button button = Instantiate(buttonPrefab, this.gameObject.transform).GetComponent<UnityEngine.UI.Button>();

            button.GetComponentInChildren<TMPro.TMP_Text>().text = file;
            button.gameObject.SetActive(true);
            buttons.Add(button);
        }
    }
}
    }
