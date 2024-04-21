using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class MovingEntitySave : MapEntitySave
    {
        /// <summary>
        /// Speed of the entity (how fast it will move on the board)
        /// </summary>
        public float Speed;

        /// <summary>
        /// How many times can the entity be "killed"
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

        protected void SaveMovingEntity(MovingEntity entity)
        {
            this.Speed = entity.Speed;
            this.Hp = entity.Hp;
            this.ImmuneTime = entity.ImmuneTime;
            this.CurrentDirection = entity.CurrentDirection;
            this.Alive = entity.Alive;

            Save(entity);
        }
    }
}