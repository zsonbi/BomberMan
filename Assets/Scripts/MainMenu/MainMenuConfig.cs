using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MainMenuConfig
{
    /// <summary>
    /// Player's name (default: null)
    /// </summary>
    public static string[] PlayerNames = new string[3];

    /// <summary>
    /// How many players (default: checked)
    /// </summary>
    public static bool Player3 = true;

    /// <summary>
    /// Choosen map index to play on (default: 0)
    /// </summary>
    public static int Map;

    /// <summary>
    /// Is Battle Royale Game mode choosen (default: checked)
    /// </summary>
    public static bool BattleRoyale;

    /// <summary>
    /// Required points to win (default: 3)
    /// </summary>
    public static int RequiredPoint;


    public static void ResetEveryValue()
    {
        PlayerNames = new string[3];
        Player3 = true;
        Map = 0;
        BattleRoyale = true;
        RequiredPoint = 3;
    }

}
