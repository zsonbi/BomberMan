using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapEntity : MonoBehaviour
{
    public Position CurrentBoardPos { get; protected set; }
    public MapEntity EntityType { get; protected set; }
    public GameBoard GameBoard { get; protected set; }

    public abstract void Init(MapEntityType entityType, GameBoard gameBoard, Position CurrentPos);
}