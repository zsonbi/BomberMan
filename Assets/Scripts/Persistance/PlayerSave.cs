using Bomberman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class PlayerSave : MovingEntitySave
    {
        public void SavePlayer(Player playerToSave)
        {
            base.SaveMovingEntity(playerToSave);
            base.Save(playerToSave);
        }
    }
}