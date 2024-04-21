using System.Collections.Generic;
using Bomberman;
using Menu;

namespace Persistance
{
    /// <summary>
    /// A container to save the players
    /// </summary>
    public class PlayerSave : MovingEntitySave
    {
        //The id of the player (set it in the editor)
        public int PlayerId;

        /// <summary>
        /// The player's name
        /// </summary>
        public string PlayerName;

        /// <summary>
        /// The bomb's of the player
        /// </summary>
        public List<BombSave> Bombs = new List<BombSave>();

        /// <summary>
        /// The bonuses active for the player
        /// </summary>
        public List<BonusSave> Bonuses = new List<BonusSave>();

        /// <summary>
        /// The current score of the player
        /// </summary>
        public int Score = 0;

        /// <summary>
        /// How many Obstacle can the player can still place
        /// </summary>
        public int AvailableObstacle = 0;

        /// <summary>
        /// How long does the ImmunityBonus lasts
        /// </summary>
        public float ImmunityMaxDuration;
        /// <summary>
        /// How long does the GhostDuration lasts
        /// </summary>
        public float GhostMaxDuration;

        /// <summary>
        /// The name of the player's skin
        /// </summary>
        public string SkinName { get; private set; }

        /// <summary>
        /// Saves the player
        /// </summary>
        /// <param name="playerToSave"></param>
        public void SavePlayer(Player playerToSave)
        {
            this.PlayerId = playerToSave.PlayerId;
            this.PlayerName = playerToSave.PlayerName;
            this.Score = playerToSave.Score;
            this.AvailableObstacle = playerToSave.AvailableObstacle;
            this.ImmunityMaxDuration = playerToSave.ImmunityMaxDuration;
            this.GhostMaxDuration = playerToSave.GhostMaxDuration;
            this.SkinName = MainMenuConfig.PlayerSkins[this.PlayerId];

            //Save the bombs
            foreach (var bomb in playerToSave.Bombs)
            {
                BombSave bombSave = new BombSave();
                bombSave.SaveBomb(bomb);
                Bombs.Add(bombSave);
            }

            //Save the bonuses
            foreach (var item in playerToSave.Bonuses)
            {
                BonusSave bombSave = new BonusSave();
                bombSave.SaveBonus(item.Value);
                this.Bonuses.Add(bombSave);
            }
            //Save the MovingEntityComponent
            base.SaveMovingEntity(playerToSave);
        }
    }
}