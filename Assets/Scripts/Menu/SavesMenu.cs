using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SavesMenu : MonoBehaviour
{
    private string[] files;

    [SerializeField]
    private Button buttonPrefab;

    public EventHandler LoadFileEventHandler;
    private List<Button> buttons = new List<Button>();

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
            if (buttons[i] == gameObjectClicked)
            {
                index = i; break;
            }
        }

        LoadFileEventHandler.Invoke(index, EventArgs.Empty);
    }

    private void AddButtons()
    {
        ResetButtons();

        foreach (var file in files)
        {
            Button button = Instantiate(buttonPrefab, this.gameObject.transform).GetComponent<Button>();

            button.GetComponentInChildren<Text>().text = file;
            buttons.Add(button);
        }
    }
}