using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Config
{
    public const float CELLSIZE = 5;

    public const float IMMUNETIME = 2f;

    public const float PLAYERACTIONCOOLDOWN = 0.5f;

    public const float GHOSTPASSTHROUGHCHANCE = 0.3f;

    public static byte PlayerCount = 1;

    public static byte MonsterCount = 3;

    public static System.Random RND = new System.Random();
}