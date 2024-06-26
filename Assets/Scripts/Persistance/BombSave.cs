using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    /// <summary>
    /// A save container for the Bomb
    /// </summary>
    public class BombSave : MapEntitySave
    {
        /// <summary>
        /// How far does the bomb is blasting
        /// </summary>
        public int BlastRadius;

        /// <summary>
        /// Is the bomb is currently placed and active
        /// </summary>
        public bool Placed;

        /// <summary>
        /// The bomb's internal timer
        /// </summary>
        public float BombTimer;

        /// <summary>
        /// How long does the bomb take to blow up
        /// </summary>
        public float TimeTillBlow = Config.BOMBBLOWTIME;

        /// <summary>
        /// Is the bomb detonable
        /// </summary>
        public bool Detonable = false;

        /// <summary>
        /// Saves the bomb's values
        /// </summary>
        /// <param name="bombToSave">The bomb object to save</param>
        public void SaveBomb(Bomb bombToSave)
        {
            this.BlastRadius = bombToSave.BlastRadius;
            this.Placed = bombToSave.Placed;
            this.BombTimer = bombToSave.BombTimer;
            this.TimeTillBlow = bombToSave.TimeTillBlow;
            this.Detonable = bombToSave.Detonable;
            //Call the MapEntitySave
            base.Save(bombToSave);
        }
    }
}