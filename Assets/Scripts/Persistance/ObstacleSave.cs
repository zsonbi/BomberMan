using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Persistance
{
    public class ObstacleSave : MapEntitySave
    {
        public bool Placed;

        public bool Destructible;

        public BonusType? ContainingBonusType = null;

        public bool NotPassable;

        public int OwnerId;

        public void SaveObstacle(Obstacle obstacleToSave)
        {
            this.Placed = obstacleToSave.Placed;
            this.Destructible = obstacleToSave.Destructible;
            if (obstacleToSave.ContainingBonus is not null)
            {
                this.ContainingBonusType = obstacleToSave.ContainingBonus.Type;
            }
            this.NotPassable = obstacleToSave.NotPassable;
            this.OwnerId = obstacleToSave.OwnerId;
            base.Save(obstacleToSave);
        }
    }
}