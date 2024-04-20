using Bomberman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistance
{
    public class GameSave
    {
        public ObstacleSave[] Cells;
        public float GameOverTimer;

        public float[] BattleRoyaleTimers;
        public float BattleRoyaleTimerIndex;
        public int RowCount;
        public int ColCount;
        public bool Paused;
        public bool StartGameOverCounter;
        public bool BattleRoyaleMode;
        public int RequiredPoints;
        public List<PlayerSave> Players = new List<PlayerSave>();
        public List<MonsterSave> Monsters = new List<MonsterSave>();

        public GameSave()
        {
        }

        public GameSave(GameBoard gameBoardToSave)
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