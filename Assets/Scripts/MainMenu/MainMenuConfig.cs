public static class MainMenuConfig
{
    /// <summary>
    /// Player's name (default: empty strings)
    /// </summary>
    public static string[] PlayerNames = new string[3] { "player1", "player2", "player3" };

    public static string[] PlayerSkins = new string[3] {"ChenSpray", "Sylvanas_Spray", "Illidan_Spray" };

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
}