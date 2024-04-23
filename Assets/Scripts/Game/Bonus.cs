using UnityEngine;
using DataTypes;
using Persistance;

namespace Bomberman
{
    /// <summary>
    /// Contains the methods and datas of the bonuses
    /// </summary>
    public class Bonus : MapEntity
    {
        [SerializeField]
        private int tier;

        [SerializeField]
        private BonusType type;

        [SerializeField]
        private float duration;

        [SerializeField]
        private bool decaying = false;

        /// <summary>
        /// The bonus's tier property
        /// </summary>
        public int Tier { get => tier; private set => tier = value; }

        /// <summary>
        /// The type of the bonus
        /// </summary>
        public BonusType Type { get => type; private set => type = value; }

        /// <summary>
        /// How much the bonus is active
        /// </summary>
        public float Duration { get => duration; private set => duration = value; }
        
        /// <summary>
        /// Is the bonus is decaying type or not
        /// </summary>
        public bool Decaying { get => decaying; private set => decaying = value; }
        
        /// <summary>
        /// If this is called, the bonus will be shown on the gameboard
        /// </summary>
        public void Show()
        {
            this.gameObject.SetActive(true);
        }
       
        /// <summary>
        /// If this is called, the bonus will be hidden on the gameboard
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Initialize the bonus
        /// </summary>
        public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
        {
            base.Init(entityType, gameBoard, CurrentPos);
            this.GameBoard.Entites.Add(this);
        }

        /// <summary>
        /// Loads the bonus's paramenters
        /// </summary>
        /// <param name="bonusSave">The bonus to load</param>
        public void LoadBonus(BonusSave bonusSave)
        {
            this.Tier = bonusSave.Tier;
            this.Duration = bonusSave.Duration;
        }

        /// <summary>
        /// Decrease it's duration if it's duration hit's 0 or less return true
        /// </summary>
        /// <param name="amount">How much to decrease</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public bool DecreaseDuration(float amount)
        {
            if (!Decaying)
            {
                throw new System.Exception("Not decaying");
            }
            this.Duration -= amount;
            return this.Duration <= 0;
        }
        /// <summary>
        /// Resets the decaying type bonus duration to base duration (helpfull when a decaying bonus picked up when its already active)
        /// </summary>
        public void ResetDecayingBonus(float baseDuration)
        {
            this.Duration = baseDuration;
        }
        /// <summary>
        /// Increases the tier of thee bonus
        /// </summary>
        public bool IncreaseTier()
        {
            int maxTier = 1;
            switch (Type)
            {
                case BonusType.BonusBomb:
                    maxTier = BonusConfigs.EXTRA_BOMB_MAX_TIER;
                    break;

                case BonusType.BombRange:
                    maxTier = BonusConfigs.EXTRA_RANGE_MAX_TIER;
                    break;

                case BonusType.Detonator:
                    break;

                case BonusType.Skate:
                    break;

                case BonusType.Immunity:
                    break;

                case BonusType.Ghost:
                    break;

                case BonusType.Obstacle:
                    maxTier = BonusConfigs.OBSTACLE_BONUS_MAX_TIER;
                    break;

                default:
                    break;
            }

            if (Tier < maxTier)
            {
                Tier++;
                return true;
            }
            return false;
        }
    }
}