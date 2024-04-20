using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public abstract class BombSave : MapEntitySave
    {
        /// <summary>
        /// The entity's current position on the Board's grid
        /// </summary>
        public Position CurrentBoardPos { get; protected set; }

        /// <summary>
        /// The type of the entity
        /// </summary>
        public MapEntityType EntityType { get; protected set; }

        public BombSave(Position currentBoardPos, MapEntityType mapEntityType) : base(currentBoardPos, mapEntityType)
        {
            this.CurrentBoardPos = currentBoardPos;
        }
    }
}