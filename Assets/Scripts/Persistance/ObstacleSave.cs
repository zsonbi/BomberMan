using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Persistance
{
    public abstract class ObstacleSave : MapEntitySave
    {
        public bool Placed { get; private set; }

        public bool Destructible { get; private set; }

        public BonusType ContainingBonusType { get; private set; }

        public bool NotPassable { get; private set; }

        public void SaveObstacle(Obstacle obstacleToSave)
        {
            this.Placed = obstacleToSave.Placed;
            this.Destructible = obstacleToSave.Destructible;
            this.ContainingBonusType = obstacleToSave.ContainingBonus.Type;
            this.NotPassable = obstacleToSave.NotPassable;

            base.Save(obstacleToSave);
        }
    }
}