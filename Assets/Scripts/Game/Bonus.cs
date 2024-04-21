using UnityEngine;
using DataTypes;
using Persistance;

namespace Bomberman
{
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

        public int Tier { get => tier; private set => tier = value; }

        public BonusType Type { get => type; private set => type = value; }

        public float Duration { get => duration; private set => duration = value; }

        public bool Decaying { get => decaying; private set => decaying = value; }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

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

        public void ResetDecayingBonus(float baseDuration)
        {
            this.Duration = baseDuration;
        }

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
                    maxTier = BonusConfigs.EXTRA_WALL_MAX_TIER;
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