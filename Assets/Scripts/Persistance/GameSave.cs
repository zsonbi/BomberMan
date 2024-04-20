using Bomberman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class GameSave
    {
        public Obstacle[] Cells { get; private set; }
        public float gameOverTimer { get; private set; }
        public float battleRoyaleTimers { get; private set; }
        public float battleRoyaleTimerIndex { get; private set; }
        public List<MapEntity> Entities { get; private set; }
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
        public bool Paused { get; private set; }
        public bool StartGameOverCounter { get; private set; }
        public bool BattleRoyaleMode { get; private set; }
        public int RequiredPoints { get; private set; }
        public int timeConst { get; private set; }
    }
}