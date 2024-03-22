using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MapEntity
{
    [SerializeField]
    private int tier;

    [SerializeField]
    private BonusType type;

    [SerializeField]
    private float duration;

    [SerializeField]
    private bool decaying = false;

    public int Tier { get => tier; private set => tier = value; }

    public BonusType Type { get => type; private set => type = value; }

    public float Duration { get => duration; private set => duration = value; }

    public bool Decaying { get => decaying; private set => decaying = value; }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public bool IncreaseTier()
    {
        int maxTier = -1;
        switch (Type)
        {
            case BonusType.BonusBomb:
                maxTier = BonusConfigs.EXTRA_BOMB_MAX_TIER;
                break;

            case BonusType.BombRange:
                break;

            case BonusType.Detonator:
                break;

            case BonusType.Skate:
                break;

            case BonusType.Immunity:
                break;

            case BonusType.Ghost:
                break;

            case BonusType.Obstacle:
                break;

            case BonusType.Slowness:
                break;

            case BonusType.SmallExplosion:
                break;

            case BonusType.NoBomb:
                break;

            case BonusType.InstantBomb:
                break;

            default:
                break;
        }

        if (Tier < maxTier)
        {
            Tier++;
            return true;
        }
        return false;
    }
}