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
        public float Speed { get; protected set; }

        /// <summary>
        /// How many times can the entity be "killed"
        /// </summary>
        public int Hp { get; protected set; }

        /// <summary>
        /// The entity's immuneTime
        /// </summary>
        public float ImmuneTime { get; protected set; }

        /// <summary>
        /// Is the entity alive
        /// </summary>
        public bool Alive { get; protected set; }

        /// <summary>
        /// The current moving direction of the entity
        /// </summary>
        public Direction CurrentDirection { get; private set; }

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