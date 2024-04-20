using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class MovingEntitySave : MapEntitySave
    {
        public MovingEntitySave(Position currentBoardPos) : base(currentBoardPos)
        {
        }
    }
}