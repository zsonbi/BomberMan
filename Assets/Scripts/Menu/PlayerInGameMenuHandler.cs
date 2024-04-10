using DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameMenuHandler : MonoBehaviour
{

    [SerializeField]
    Sprite healthSprite;

    [SerializeField]
    GameObject healthBarContainer;

    [SerializeField]
    GameObject bonusesContainer;

    [SerializeField]
    UnityEngine.UI.Image playerIconImage;

    [SerializeField]
    TMP_Text playerName;

    List<Image> playerHealths = new List<Image>();

    private int currentHealth = 0;
    List<BonusType> bonuses = new List<BonusType>();

    public void SetUpPanel(Player player)
    {
        playerName.text = player.PlayerName;
        playerIconImage.sprite = player.gameObject.GetComponent<SpriteRenderer>().sprite;
        while (bonuses.Count > 0)
        {
            RemoveBonus(bonuses[0]);
            bonuses.RemoveAt(0);
        }
        while (currentHealth > player.Hp)
        {
            RemoveHealth();
        }

        for (int i = currentHealth; i < player.Hp; i++)
        {
            AddHealth();
        }


    }

    public void UpdatePlayer(Player playerToDisplay)
    {
        if (playerToDisplay.Hp < currentHealth)
        {
            AddHealth();
        }
        else if (playerToDisplay.Hp > currentHealth)
        {
            RemoveHealth();
        }

        int count = 0;
        foreach (var bonusType in playerToDisplay.Bonuses.Keys)
        {
            if (!bonuses.Contains(bonusType))
            {
                AddBonus(bonusType, playerToDisplay.Bonuses[bonusType].gameObject.GetComponent<SpriteRenderer>().sprite);
            }
            count++;
        }
        if (count != bonuses.Count)
        {
            for (int i = 0; i < bonuses.Count; i++)
            {
                if (!playerToDisplay.Bonuses.ContainsKey(bonuses[i]))
                {
                    RemoveBonus(bonuses[i]);
                    bonuses.RemoveAt(i);
                    i--;
                }
            }

        }

    }

    private void AddHealth()
    {
        currentHealth++;
        GameObject healthGameObject = new GameObject("health" + currentHealth, typeof(UnityEngine.UI.Image));
        healthGameObject.transform.SetParent( healthBarContainer.transform);
        Image healthImage = healthGameObject.GetComponent<Image>();
        RectTransform rectTransform = healthImage.GetComponent<RectTransform>();
        healthImage.sprite = healthSprite;
        rectTransform.anchorMin = new Vector2(0,0.5f);
        rectTransform.anchorMax = new Vector2(0,0.5f);
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition= new Vector2(-30+(currentHealth-1)*30,0);
        rectTransform.sizeDelta=new Vector2(30,30);
        
    }

    private void RemoveHealth()
    {
        currentHealth--;
    }

    private void AddBonus(BonusType type, Sprite bonusImage)
    {
        bonuses.Add(type);
    }

    private void RemoveBonus(BonusType type)
    {

    }



}
