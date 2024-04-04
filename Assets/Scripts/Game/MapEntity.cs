using UnityEngine;
using DataTypes;

namespace Bomberman
{
    /// <summary>
    /// Everything which will be placed on the board should be derived from this class
    /// </summary>
    public abstract class MapEntity : MonoBehaviour
    {
        /// <summary>
        /// The entity's current position on the Board's grid
        /// </summary>
        public Position CurrentBoardPos { get; protected set; }

        /// <summary>
        /// The type of the entity
        /// </summary>
        public MapEntityType EntityType { get; protected set; }

        /// <summary>
        /// Reference to the parent board
        /// </summary>
        public GameBoard GameBoard { get; protected set; }

        /// <summary>
        /// Inits the entity should be called right after creating the entity
        /// </summary>
        /// <param name="entityType">The type of the entity</param>
        /// <param name="gameBoard">The parent board</param>
        /// <param name="CurrentPos">The current position of the entity</param>
        public virtual void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
        {
            this.EntityType = entityType;
            this.GameBoard = gameBoard;
            this.CurrentBoardPos = CurrentPos;
        }
    }
}