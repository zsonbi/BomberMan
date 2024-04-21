using Bomberman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
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

        public float ImmunityMaxDuration;
        public float GhostMaxDuration;

        public string SkinName { get; private set; }

        public void SavePlayer(Player playerToSave)
        {
            this.PlayerId = playerToSave.PlayerId;
            this.PlayerName = playerToSave.PlayerName;
            this.Score = playerToSave.Score;
            this.AvailableObstacle = playerToSave.AvailableObstacle;
            this.ImmunityMaxDuration = playerToSave.ImmunityMaxDuration;
            this.GhostMaxDuration = playerToSave.GhostMaxDuration;
            this.SkinName = MainMenuConfig.PlayerSkins[this.PlayerId];

            foreach (var bomb in playerToSave.Bombs)
            {
                BombSave bombSave = new BombSave();
                bombSave.SaveBomb(bomb);
                Bombs.Add(bombSave);
            }

            foreach (var item in playerToSave.Bonuses)
            {
                BonusSave bombSave = new BonusSave();
                bombSave.SaveBonus(item.Value);
                this.Bonuses.Add(bombSave);
            }

            base.SaveMovingEntity(playerToSave);
            base.Save(playerToSave);
        }
    }
}