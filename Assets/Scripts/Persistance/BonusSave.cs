using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class BonusSave : MapEntitySave
    {
        public int Tier;

        public BonusType Type;

        public float Duration;

        public bool Decaying;

        public void SaveBonus(Bonus bonusToSave)
        {
            base.Save(bonusToSave);
            this.Tier = bonusToSave.Tier;
            this.Type = bonusToSave.Type;
            this.Duration = bonusToSave.Duration;
            this.Decaying = bonusToSave.Decaying;
        }
    }
}