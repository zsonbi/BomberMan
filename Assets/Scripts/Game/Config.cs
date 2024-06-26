using System.Collections.Generic;
using UnityEngine;

namespace Bomberman
{
    /// <summary>
    /// This is where the game's settings is stored
    /// </summary>
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

        /// <summary>
        /// How long the game waits before announcing the winner
        /// </summary>
        public const float GAME_OVER_TIMER = 3f;

        /// <summary>
        /// How quickly should the explosion spread
        /// </summary>
        public const float BOMBEXPLOSIONSPREADSPEED = 0.3f;

        /// <summary>
        /// The number of players to spawn on start
        /// </summary>
        public static byte PlayerCount = 1;

        /// <summary>
        /// How many monsters does the game has
        /// </summary>
        public static byte MonsterCount = 3;

        /// <summary>
        /// How quick should the battle royale circle decrease
        /// </summary>
        public const float CIRCLE_DECREASE_RATE = 5f;

        public static readonly KeyCode[,] PLAYERDEFAULTKEYS = new KeyCode[3, 6] { { KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A, KeyCode.Space, KeyCode.LeftAlt }, { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightShift, KeyCode.Return }, { KeyCode.Keypad8, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad4, KeyCode.KeypadPlus, KeyCode.KeypadEnter } };

        /// <summary>
        /// Global random for the program
        /// </summary>
        public static readonly System.Random RND = new System.Random();

        /// <summary>
        /// The even indices how long the circle should stop shrinking
        /// Odd indices how long the circle should shrink
        /// </summary>
        public static readonly float[] BATTLE_ROYALE_TIMERS = new float[] { 10, 20, 20, 20 };

        /// <summary>
        /// The player's color when it picks up a ghost
        /// </summary>
        public readonly static Color GHOST_COLOR = new Color(1, 1, 1, 0.5f);


    }

}