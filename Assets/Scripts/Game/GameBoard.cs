using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameBoard : MonoBehaviour
{
    public Obstacle[,] Cells { get; private set; }

    public List<MapEntity> Entites { get; private set; }
    public int RowCount { get; private set; }
    public int ColCount { get; private set; }

    [SerializeField]
    public List<Player> Players { get; private set; }

    [SerializeField]
    public List<Monster> Monsters { get; private set; }

    [SerializeField]
    public float CircleDecreaseRate { get; private set; }

    public EventHandler UpdateMenuFields;

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void DecreaseCircle()
    {
    }

    public void Reset()
    {
    }
}