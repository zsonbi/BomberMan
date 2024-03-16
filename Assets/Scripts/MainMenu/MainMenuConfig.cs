using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MainMenuConfig
{
    /// <summary>
    /// Player's name
    /// </summary>
    public static string[] PlayerNames = new string[3];

    /// <summary>
    /// How many players
    /// </summary>
    public static bool Player3;

    /// <summary>
    /// Choosen map index to play on
    /// </summary>
    public static int Map;

    /// <summary>
    /// Is Battle Royale Game mode choosen
    /// </summary>
    public static bool BattleRoyale;

    /// <summary>
    /// Required points to win
    /// </summary>
    public static int RequiredPoint;

}
