using DataTypes;

namespace Bomberman
{
    /// <summary>
    /// The basic monster's brain
    /// </summary>
    public class BasicBrain : MonsterBrain
    {
        /// <summary>
        /// What direction to move towards when changed cells
        /// </summary>
        /// <returns>A new direction to move towards</returns>
        public override Direction ChangedCell()
        {
            if (Accuracy < Config.RND.NextDouble())
            {
                return NextTargetDir();
            }
            else
            {
                if (!body.DirectionPassable(body.CurrentDirection))
                {
                    return NextTargetDir();
                }
                else
                {
                    return body.CurrentDirection;
                }
            }
        }
    }
}