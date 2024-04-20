using Bomberman;
using DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class MonsterSave : MovingEntitySave
    {
        public MonsterType Type { get; private set; }

        //The monster's brain for the pathfinding
        private MonsterBrain Brain;

        public void SaveMonster(Monster monsterToSave)
        {
            this.Type = monsterToSave.Type;

            base.SaveMovingEntity(monsterToSave);
            base.Save(monsterToSave);
        }
    }
}