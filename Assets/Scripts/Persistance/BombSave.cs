using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public abstract class BombSave : MapEntitySave
    {
 

        public BombSave(Position currentBoardPos, MapEntityType mapEntityType) : base(currentBoardPos, mapEntityType)
        {
            this.CurrentBoardPos = currentBoardPos;
        }
    }
}