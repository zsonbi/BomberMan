using System.Threading.Tasks;
using UnityEngine;

public abstract class MonsterBrain
{
    protected Monster body;
    public Player Target { get; private set; }

    public float Accuracy { get; private set; }

    public virtual void InitBrain(Monster body, float accuracy = 0.9f)
    {
        this.body = body;
        this.Accuracy = accuracy;
    }

    public abstract Direction NextTargetDir();

    public abstract Direction ChangedCell();
}