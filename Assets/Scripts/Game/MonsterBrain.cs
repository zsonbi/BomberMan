using DataTypes;
using System.Collections.Generic;

namespace Bomberman
{
    public abstract class MonsterBrain
    {
        /// <summary>
        /// The monster which the brain is controlling
        /// </summary>
        protected Monster body;

        /// <summary>
        /// The accuracy of the monster (how frequently should it be full random)
        /// </summary>
        public float Accuracy { get; private set; }

        /// <summary>
        /// Inits the brain
        /// </summary>
        /// <param name="body">The parent monster</param>
        /// <param name="accuracy">The accuracy of the brain</param>
        public virtual void InitBrain(Monster body, float accuracy = 0.9f)
        {
            this.body = body;
            this.Accuracy = accuracy;
            if (body.Type == MonsterType.Ghost)
            {
                body.SetGhost(true);
            }
        }

        /// <summary>
        /// Gets the next target direction
        /// </summary>
        /// <returns>A new direction to move towards</returns>
        protected Direction NextTargetDir()
        {
            List<Direction> possDir = new List<Direction>();

            for (int i = 0; i < 4; i++)
            {
                if (body.DirectionPassable((Direction)i))
                {
                    possDir.Add((Direction)i);
                }
            }

            if (possDir.Count == 0)
            {
                return (Direction)((byte)(body.CurrentDirection + 2) % 4);
            }

            return possDir[Config.RND.Next(0, possDir.Count)];
        }

        /// <summary>
        /// What direction to move towards when changed cells
        /// </summary>
        /// <returns>A new direction to move towards</returns>
        public abstract Direction ChangedCell();
    }
}