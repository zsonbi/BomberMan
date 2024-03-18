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
    /// Choosen map index to play on (default: null)
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

}
