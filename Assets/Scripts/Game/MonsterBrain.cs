using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBrain : MonoBehaviour
{
    private Monster body;
    public Player Target { get; private set; }

    public float Accuracy { get; private set; }

    public void InitBrain(Monster body, float accuracy = 0.9f)
    {
        this.body = body;
        this.Accuracy = accuracy;
    }

    public void NextTargetDir()
    {
        throw new System.NotImplementedException();
    }
}