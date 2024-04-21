using Bomberman;
using DataTypes;
using Persistance;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Bomberman {

    /// <summary>
    /// This is the class which contains the oblstacle's functions and datas
    /// </summary>
    public class Obstacle : MapEntity
    {
        [SerializeField]
        private bool placed = false;

        /// <summary>
        /// Is the obstacle placed
        /// </summary>
        public bool Placed { get => placed; private set => placed = value; }

        /// <summary>
        /// Does it contains a bomb
        /// </summary>
        public bool HasBomb { get => placedBomb is not null; }

        [SerializeField]
        private bool destructible;

        [SerializeField]
        private bool notPassable;

        [SerializeField]
        private Sprite spriteWhenPlaced;

        [SerializeField]
        private Sprite spriteWhenBlownUp;

        [SerializeField]
        public List<GameObject> bonusPrefabs;

        private SpriteRenderer spriteRenderer;

        private Bomb placedBomb = null;

        /// <summary>
        /// Is it destructible
        /// </summary>
        public bool Destructible { get => destructible; private set => destructible = value; }

        /// <summary>
        /// What type of bonus does it contains
        /// </summary>
        public Bonus ContainingBonus { get; private set; }

        /// <summary>
        /// Is it notpassable
        /// </summary>
        public bool NotPassable { get => notPassable; private set => notPassable = value; }

        /// <summary>
        /// Who is the placed obstacle owner
        /// </summary>
        public int OwnerId { get; private set; } = -1;

        /// <summary>
        /// The blown up event handler
        /// </summary>
        public EventHandler BlownUp;

        private void Awake()
        {
            this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// This function is for testing purposes only!!!
        /// Spawn a specific bonus at the obstacle's position
        /// </summary>
        public void SpawnBonus(BonusType bonusToSpawn)
        {
   

            Bonus bonus = Instantiate(GameBoard.BonusPrefabs[bonusToSpawn], this.GameBoard.gameObject.transform).GetComponent<Bonus>();
            bonus.gameObject.transform.transform.localPosition = new Vector3(CurrentBoardPos.Col * Config.CELLSIZE, -2.5f - CurrentBoardPos.Row * Config.CELLSIZE, 1);
            bonus.Init(MapEntityType.Bonus, this.GameBoard, new Position(this.CurrentBoardPos.Row, this.CurrentBoardPos.Col));
            bonus.Show();
        }

        private void DropBonus()
        {
            if (ContainingBonus is not null)
            {
                ContainingBonus.Show();
                this.ContainingBonus = null;
            }
        }

        /// <summary>
        /// Initializes the obstacle
        /// </summary>
        public override void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
        {
            base.Init(entityType, gameBoard, CurrentPos);
        }

        /// <summary>
        /// Loads in the obstacle
        /// </summary>
        /// <param name="obstacleSave"></param>
        public void ObstacleLoad(ObstacleSave obstacleSave)
        {
            if (obstacleSave.Placed)
            {
                Place(false);
            }
            this.placed = obstacleSave.Placed;
            this.Destructible = obstacleSave.Destructible;
            this.notPassable = obstacleSave.NotPassable;
            this.OwnerId = obstacleSave.OwnerId;
            //Create a new bonus which it contains
            if (obstacleSave.ContainingBonusType != null)
            {
                Bonus bonus = Instantiate(GameBoard.BonusPrefabs[obstacleSave.ContainingBonusType.Value], this.GameBoard.gameObject.transform).GetComponent<Bonus>();
                bonus.gameObject.transform.transform.localPosition = new Vector3(CurrentBoardPos.Col * Config.CELLSIZE, -2.5f - CurrentBoardPos.Row * Config.CELLSIZE, 1);
                bonus.Init(MapEntityType.Bonus, this.GameBoard, new Position(this.CurrentBoardPos.Row, this.CurrentBoardPos.Col));
                this.ContainingBonus = bonus;
            }
        }

        /// <summary>
        /// This method controls the placing of the obstacle
        /// </summary>
        public bool Place(bool containBonus, int placerId = -1)
        {
            if (this.Placed)
            {
                return false;
            }
            this.Placed = true;
            this.OwnerId = placerId;
            if (spriteWhenPlaced is not null)
            {
                spriteRenderer.sprite = spriteWhenPlaced;
            }
            else
            {
                Debug.LogError("Sprite when placed is not set!");
            }

            if (containBonus)
            {
                this.ContainingBonus = Instantiate(bonusPrefabs[Config.RND.Next(0, bonusPrefabs.Count)], this.GameBoard.gameObject.transform).GetComponent<Bonus>();
                this.ContainingBonus.gameObject.transform.transform.localPosition = new Vector3(CurrentBoardPos.Col * Config.CELLSIZE, -2.5f - CurrentBoardPos.Row * Config.CELLSIZE, 1);
                this.ContainingBonus.Init(MapEntityType.Bonus, this.GameBoard, new Position(this.CurrentBoardPos.Row, this.CurrentBoardPos.Col));
            }

            return true;
        }

        /// <summary>
        /// This method controls the blowing up of the obstacle
        /// </summary>
        public bool BlowUp(bool dropBonus = true)
        {
            if (!this.Placed)
            {
                return false;
            }

            if (HasBomb)
            {
                placedBomb.BlowUp();
                placedBomb = null;
            }
            else if (dropBonus)
            {
                DropBonus();
            }

            if (BlownUp is not null)
            {
                BlownUp.Invoke(this, EventArgs.Empty);
                BlownUp = null;
                this.OwnerId = -1;
            }

            this.Placed = false;
            if (spriteWhenBlownUp is not null)
            {
                spriteRenderer.sprite = spriteWhenBlownUp;
            }
            else
            {
                Debug.LogError("Sprite when blown up is not set!");
            }

            return true;
        }

        /// <summary>
        /// Places a bomb
        /// </summary>
        public void PlaceBomb(Bomb bombToPlace)
        {
            //bandaid
            spriteRenderer.sprite = spriteWhenBlownUp;

            this.placedBomb = bombToPlace;
            this.Placed = true;
        }

        /// <summary>
        /// Erases a bomb
        /// </summary>
        public void EraseBomb()
        {
            //bandaid
            spriteRenderer.sprite = spriteWhenBlownUp;
            this.placedBomb = null;
            this.Placed = false;
        }
    }
}