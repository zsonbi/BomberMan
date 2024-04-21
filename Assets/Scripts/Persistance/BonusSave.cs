using Bomberman;
using DataTypes;

namespace Persistance
{
    /// <summary>
    /// A save container for the bonus
    /// </summary>
    public class BonusSave : MapEntitySave
    {
        /// <summary>
        /// The tier of the saved bonus
        /// </summary>
        public int Tier;

        /// <summary>
        /// The type of the saved bonus
        /// </summary>
        public BonusType Type;

        /// <summary>
        /// How long the bonus lasts
        /// </summary>
        public float Duration;

        /// <summary>
        /// Is the bomb currently decaying
        /// </summary>
        public bool Decaying;

        /// <summary>
        /// Saves Bonus's values
        /// </summary>
        /// <param name="bonusToSave">The bonus to save in the container</param>
        public void SaveBonus(Bonus bonusToSave)
        {
            //Call the MapEntitySave
            base.Save(bonusToSave);
            this.Tier = bonusToSave.Tier;
            this.Type = bonusToSave.Type;
            this.Duration = bonusToSave.Duration;
            this.Decaying = bonusToSave.Decaying;
        }
    }
}