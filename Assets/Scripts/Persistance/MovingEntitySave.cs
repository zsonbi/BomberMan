using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    /// <summary>
    /// An abstract container to save the MovingEntities
    /// </summary>
    public class MovingEntitySave : MapEntitySave
    {
        /// <summary>
        /// The spped of the saved entity
        /// </summary>
        public float Speed;

        /// <summary>
        /// How many extra health does the saved entity has
        /// </summary>
        public int Hp;

        /// <summary>
        /// The entity's immuneTime
        /// </summary>
        public float ImmuneTime;

        /// <summary>
        /// Is the entity alive
        /// </summary>
        public bool Alive;

        /// <summary>
        /// The current moving direction of the entity
        /// </summary>
        public Direction CurrentDirection;

        /// <summary>
        /// Saves the entity's MovingEntity compontents
        /// </summary>
        /// <param name="entity">The entity to save</param>
        protected void SaveMovingEntity(MovingEntity entity)
        {
            this.Speed = entity.Speed;
            this.Hp = entity.Hp;
            this.ImmuneTime = entity.ImmuneTime;
            this.CurrentDirection = entity.CurrentDirection;
            this.Alive = entity.Alive;
            //Save the MapEntity
            Save(entity);
        }
    }
}