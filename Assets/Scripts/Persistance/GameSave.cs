using Bomberman;
using DataTypes;
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
        public int BattleRoyaleTimerIndex;
        public int RowCount;
        public int ColCount;
        public bool Paused;
        public bool StartGameOverCounter;
        public bool BattleRoyaleMode;
        public int RequiredPoints;
        public List<PlayerSave> Players = new List<PlayerSave>();
        public List<MonsterSave> Monsters = new List<MonsterSave>();

        public List<BonusSave> droppedBonusSaves = new List<BonusSave>();

        public Position BattleRoyaleCircle;

        public GameSave()
        {
        }

        public GameSave(GameBoard gameBoardToSave, Vector3 battleRoyaleCircle)
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

            foreach (var item in gameBoardToSave.Entites)
            {
                if (item != null && item.gameObject.activeSelf && item is Bonus)
                {
                    BonusSave bonusSave = new BonusSave();
                    bonusSave.SaveBonus((Bonus)item);
                    droppedBonusSaves.Add(bonusSave);
                }
            }
            BattleRoyaleCircle = new Position((int)battleRoyaleCircle.y, (int)battleRoyaleCircle.x);
        }
    }
}