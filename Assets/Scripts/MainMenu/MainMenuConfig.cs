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
    /// How many players (default: unchecked)
    /// </summary>
    public static bool Player3;

    /// <summary>
    /// Choosen map index to play on (default: -1)
    /// </summary>
    public static int Map = -1;

    /// <summary>
    /// Is Battle Royale Game mode choosen (default: unchecked)
    /// </summary>
    public static bool BattleRoyale;

    /// <summary>
    /// Required points to win (default: 3)
    /// </summary>
    public static int RequiredPoint = 3;

}
