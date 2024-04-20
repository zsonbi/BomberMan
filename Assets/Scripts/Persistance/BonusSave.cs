using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class BonusSave : MapEntitySave
    {
        public int Tier { get; private set; }

        public BonusType Type { get; private set; }

        public float Duration { get; private set; }

        public bool Decaying { get; private set; }

        public Vector3 Position { get; private set; }

        public void SaveBonus(Bonus bonusToSave)
        {
            this.Tier = bonusToSave.Tier;
            this.Type = bonusToSave.Type;
            this.Duration = bonusToSave.Duration;
            this.Decaying = bonusToSave.Decaying;
            this.Position = bonusToSave.gameObject.transform.localPosition;
        }
    }
}