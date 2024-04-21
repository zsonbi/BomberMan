using DataTypes;
using Bomberman;

namespace Persistance
{
    /// <summary>
    /// A container to save the Obstacles
    /// </summary>
    public class ObstacleSave : MapEntitySave
    {
        /// <summary>
        /// Is the obstacle placed
        /// </summary>
        public bool Placed;

        /// <summary>
        /// Is the obstacle destructible
        /// </summary>
        public bool Destructible;

        /// <summary>
        /// Does the obstacle contain some kind of bonus
        /// </summary>
        public BonusType? ContainingBonusType = null;

        /// <summary>
        /// Is the obstacle indestructibleWall
        /// </summary>
        public bool NotPassable;

        /// <summary>
        /// The owner's id (to give back the use when it blown up)
        /// </summary>
        public int OwnerId;

        /// <summary>
        /// Saves the Obstacle
        /// </summary>
        /// <param name="obstacleToSave">The obstacle to save int the container</param>
        public void SaveObstacle(Obstacle obstacleToSave)
        {
            this.Placed = obstacleToSave.Placed;
            this.Destructible = obstacleToSave.Destructible;
            //If it contains a bonus get it's type
            if (obstacleToSave.ContainingBonus is not null)
            {
                this.ContainingBonusType = obstacleToSave.ContainingBonus.Type;
            }
            this.NotPassable = obstacleToSave.NotPassable;
            this.OwnerId = obstacleToSave.OwnerId;
            //Save the Mapentity
            base.Save(obstacleToSave);
        }
    }
}