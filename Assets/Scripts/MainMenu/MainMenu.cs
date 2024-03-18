using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Starting the game by switching between the two scene
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("BombermanScene");
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
    /// Readning the player's name
    /// </summary>
    /// <param name = "name" ></ param >
    public void ReadPlayer1Name(string name)
    {
        MainMenuConfig.PlayerNames[0] = name;
        Debug.Log("Player 1 name: " + MainMenuConfig.PlayerNames[0]);
    }

    public void ReadPlayer2Name(string name)
    {
        MainMenuConfig.PlayerNames[1] = name;
        Debug.Log("Player 2 name: " + MainMenuConfig.PlayerNames[1]);
    }

    public void ReadPlayer3Name(string name)
    {
        MainMenuConfig.PlayerNames[2] = name;
        Debug.Log("Player 3 name: " + MainMenuConfig.PlayerNames[2]);
    }


    /// <summary>
    /// Read the choosen index of the map
    /// </summary>
    /// <param name="index"></param>
    public void ReadMapIndex(int index = 0)
    {
        MainMenuConfig.Map = index;
        Debug.Log("Map index: " + MainMenuConfig.Map);
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
        MainMenuConfig.RequiredPoint = int.Parse(points);
        Debug.Log("Required points to win the game: " + MainMenuConfig.RequiredPoint);
    }
}
