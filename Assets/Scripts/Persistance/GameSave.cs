using Bomberman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class GameSave
    {
        public ObstacleSave[] Cells { get; private set; }
        public float GameOverTimer { get; private set; }
        public float[] BattleRoyaleTimers { get; private set; }
        public float BattleRoyaleTimerIndex { get; private set; }
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
        public bool Paused { get; private set; }
        public bool StartGameOverCounter { get; private set; }
        public bool BattleRoyaleMode { get; private set; }
        public int RequiredPoints { get; private set; }
        public List<PlayerSave> Players { get; private set; } = new List<PlayerSave>();
        public List<MonsterSave> Monsters { get; private set; } = new List<MonsterSave>();

        public void SaveGameBoard(GameBoard gameBoardToSave)
        {
            this.GameOverTimer = gameBoardToSave.gameOverTimer;
            this.RowCount = gameBoardToSave.RowCount;
            this.ColCount = gameBoardToSave.ColCount;
            this.BattleRoyaleMode = MainMenuConfig.BattleRoyale;
            this.RequiredPoints = MainMenuConfig.RequiredPoint;
            this.Paused = gameBoardToSave.Paused;
            this.StartGameOverCounter = gameBoardToSave.StartGameOverCounter;
            this.BattleRoyaleTimerIndex = gameBoardToSave.BattleRoyaleTimerIndex;
            this.BattleRoyaleTimers = gameBoardToSave.BattleRoyaleTimers;

            foreach (var monster in gameBoardToSave.Monsters)
            {
                MonsterSave monsterSave = new MonsterSave();
                monsterSave.SaveMonster(monster);

                Monsters.Add(monsterSave);
            }

            foreach (var player in gameBoardToSave.Players)
            {
                PlayerSave playerSave = new PlayerSave();
                playerSave.SavePlayer(player);

                Players.Add(playerSave);
            }
            Cells = new ObstacleSave[RowCount * ColCount];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    ObstacleSave obstacleSave = new ObstacleSave();
                    obstacleSave.SaveObstacle(gameBoardToSave.Cells[i, j]);

                    Cells[i * ColCount + j] = obstacleSave;
                }
            }
        }
    }
}