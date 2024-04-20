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

        /// <summary>
        /// How is the move progressing (0.0-1.0)
        /// </summary>
        public float moveProgress { get; private set; }

        /// <summary>
        /// How long does the movement take
        /// </summary>
        public float timeToMove { get; protected set; } = 1f;

        /// <summary>
        /// The current position's of the entity
        /// </summary>
        public Vector3 CurrentLocalPosition { get; private set; }

        public MovingEntitySave(Position currentBoardPos, MapEntityType mapEntityType) : base(currentBoardPos, mapEntityType)
        {
        }
    }
}