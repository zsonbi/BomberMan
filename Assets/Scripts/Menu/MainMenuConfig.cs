namespace Menu
{
    /// <summary>
    /// The settings which was changed by the main menu
    /// </summary>
    public static class MainMenuConfig
    {
        /// <summary>
        /// Player's name (default: empty strings)
        /// </summary>
        public static string[] PlayerNames = new string[3] { "player1", "player2", "player3" };
        /// <summary>
        /// The player skins (only the name)
        /// </summary>
        public static string[] PlayerSkins = new string[3] { "ChenSpray", "Sylvanas_Spray", "Illidan_Spray" };

        /// <summary>
        /// How many players (default: unchecked)
        /// </summary>
        public static bool Player3 = false;

        /// <summary>
        /// Is Battle Royale Game mode choosen (default: unchecked)
        /// </summary>
        public static bool BattleRoyale = false;

        /// <summary>
        /// Required points to win (default: 3)
        /// </summary>
        public static int RequiredPoint = 3;

        /// <summary>
        /// Is the sound muted (default: unmuted)
        /// </summary>
        public static bool SoundMuted = false;

        /// <summary>
        /// The path to the save to load
        /// if the string is empty the gameBoard won't load it
        /// </summary>
        public static string saveFilePath = "";
    }
}