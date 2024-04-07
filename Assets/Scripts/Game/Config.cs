using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    public const char CSVDELIMITER = ';';

    /// <summary>
    /// The size of a single tile
    /// </summary>
    public const float CELLSIZE = 5;

    /// <summary>
    /// How long should the entities be immune
    /// </summary>
    public const float IMMUNETIME = 1f;

    /// <summary>
    /// How long should the player action cooldown last
    /// </summary>
    public const float PLAYERACTIONCOOLDOWN = 0.5f;

    /// <summary>
    /// What is the chance for ghost no clipping
    /// </summary>
    public const float GHOSTPASSTHROUGHCHANCE = 0.3f;

    /// <summary>
    /// What is the chance for stalker missing the path
    /// </summary>
    public const float STALKERMISSCHANCE = 0.1f;

    /// <summary>
    /// How far should the bomb range be default
    /// </summary>
    public const int BOMBDEFAULTEXPLOSIONRANGE = 1;

    /// <summary>
    /// How long does it take for the bomb to blow up
    /// </summary>
    public const float BOMBBLOWTIME = 3f;

    public const float GAME_OVER_TIMER=3f;

    /// <summary>
    /// How quickly should the explosion spread
    /// </summary>
    public const float BOMBEXPLOSIONSPREADSPEED = 1f;

    /// <summary>
    /// The number of players to spawn on start
    /// </summary>
    public static byte PlayerCount = 1;

    /// <summary>
    /// How many monsters does the game has
    /// </summary>
    public static byte MonsterCount = 3;




    public static readonly KeyCode[,] PLAYERDEFAULTKEYS=new KeyCode[3,7]{{ KeyCode.W,KeyCode.S,KeyCode.A,KeyCode.D,KeyCode.Space,KeyCode.LeftAlt,KeyCode.X}, { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.RightShift, KeyCode.RightControl, KeyCode.Return }, { KeyCode.Keypad8, KeyCode.Keypad5, KeyCode.Keypad4, KeyCode.Keypad6, KeyCode.KeypadPlus, KeyCode.KeypadEnter, KeyCode.KeypadMinus } };

    /// <summary>
    /// Global random for the program
    /// </summary>
    public static System.Random RND = new System.Random();
}