using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class MonsterSave : MovingEntitySave
    {
        public MonsterType Type;

        public void SaveMonster(Monster monsterToSave)
        {
            this.Type = monsterToSave.Type;

            base.SaveMovingEntity(monsterToSave);
            base.Save(monsterToSave);
        }
    }
}