using Bomberman;
using DataTypes;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Menu;

namespace Persistance
{
    /// <summary>
    /// A container to save the gameboard
    /// </summary>
    public class GameSave
    {
        /// <summary>
        /// The cells of the gameboard
        /// </summary>
        public ObstacleSave[] Cells;
        /// <summary>
        /// How long till the game is over
        /// </summary>
        public float GameOverTimer;
        /// <summary>
        /// The timer for the battle royales
        /// </summary>
        public float[] BattleRoyaleTimers;
        /// <summary>
        /// What battleRoyale index are we using
        /// </summary>
        public int BattleRoyaleTimerIndex;
        /// <summary>
        /// How many rows does the gameboard has
        /// </summary>
        public int RowCount;
        /// <summary>
        /// How many cols does the gameboard has
        /// </summary>
        public int ColCount;
        /// <summary>
        /// Is the gameboard paused
        /// </summary>
        public bool Paused;
        /// <summary>
        /// Is the countback started
        /// </summary>
        public bool StartGameOverCounter;
        /// <summary>
        /// Is the battle royale enabled
        /// </summary>
        public bool BattleRoyaleMode;
        /// <summary>
        /// How many points are required for victory
        /// </summary>
        public int RequiredPoints;
        /// <summary>
        /// The player in the game
        /// </summary>
        public List<PlayerSave> Players = new List<PlayerSave>();
        /// <summary>
        /// The monsters in the game
        /// </summary>
        public List<MonsterSave> Monsters = new List<MonsterSave>();
        /// <summary>
        /// The displaying bonuses
        /// </summary>
        public List<BonusSave> droppedBonusSaves = new List<BonusSave>();

        /// <summary>
        /// How big is the battle royale circle currently
        /// </summary>
        public Position BattleRoyaleCircle;

        /// <summary>
        /// Constructor for NewtonSoft
        /// </summary>
        [JsonConstructor]
        public GameSave()
        {
        }

        /// <summary>
        /// Creates a new GameSave
        /// </summary>
        /// <param name="gameBoardToSave">The board to save</param>
        /// <param name="battleRoyaleCircle">The current size of the battle royale circle</param>
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

            //Save the monsters
            foreach (var monster in gameBoardToSave.Monsters)
            {
                MonsterSave monsterSave = new MonsterSave();
                monsterSave.SaveMonster(monster);

                Monsters.Add(monsterSave);
            }
            //Save the players
            foreach (var player in gameBoardToSave.Players)
            {
                PlayerSave playerSave = new PlayerSave();
                playerSave.SavePlayer(player);

                Players.Add(playerSave);
            }
            //Save the cells
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

            //Save the droped bonuses
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