using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Config
{
    /// <summary>
    /// The size of a single tile
    /// </summary>
    public const float CELLSIZE = 5;

    /// <summary>
    /// How long should the entities be immune
    /// </summary>
    public const float IMMUNETIME = 2f;

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

    public const float BOMBBLOWTIME = 3f;

    /// <summary>
    /// How many monsters does the game has
    /// </summary>
    public static byte MonsterCount = 3;

    /// <summary>
    /// Global random for the program
    /// </summary>
    public static System.Random RND = new System.Random();
}