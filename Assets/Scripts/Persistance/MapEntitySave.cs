using Bomberman;
using DataTypes;

namespace Persistance
{
    /// <summary>
    /// An abstract container to save the MapEntities
    /// </summary>
    public abstract class MapEntitySave
    {
        /// <summary>
        /// The entity's current position on the Board's grid
        /// </summary>
        public Position CurrentBoardPos;

        /// <summary>
        /// The type of the entity
        /// </summary>
        public MapEntityType EntityType;

        /// <summary>
        /// Saves the Mapentity
        /// </summary>
        /// <param name="mapEntity">The MapentityToSave</param>
        protected void Save(MapEntity mapEntity)
        {
            this.CurrentBoardPos = mapEntity.CurrentBoardPos;
            this.EntityType = mapEntity.EntityType;
        }
    }
}