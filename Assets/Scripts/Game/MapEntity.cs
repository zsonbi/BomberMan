using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapEntity : MonoBehaviour
{
    public Position CurrentBoardPos { get; protected set; }
    public MapEntityType EntityType { get; protected set; }
    public GameBoard GameBoard { get; protected set; }

    public virtual void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos)
    {
       
        this.EntityType = MapEntityType.Obstacle;
        this.GameBoard = gameBoard;
        this.CurrentBoardPos = CurrentPos;

    
}
}