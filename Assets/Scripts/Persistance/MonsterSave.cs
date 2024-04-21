using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    /// <summary>
    /// A container to save the monsters
    /// </summary>
    public class MonsterSave : MovingEntitySave
    {
        /// <summary>
        /// The type of the saved monster
        /// </summary>
        public MonsterType Type;

        /// <summary>
        /// Saves the monster
        /// </summary>
        /// <param name="monsterToSave">The monster to save</param>
        public void SaveMonster(Monster monsterToSave)
        {

            this.Type = monsterToSave.Type;
            //Save the movingEntity part
            base.SaveMovingEntity(monsterToSave);
        }
    }
}