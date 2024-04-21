using DataTypes;
using System;
using Bomberman;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{

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

        [SerializeField]
        TMP_Text ScoreText;


        private int currentHealth = 0;
        Dictionary<BonusType, RectTransform> bonuses = new Dictionary<BonusType, RectTransform>();

        List<GameObject> healthIcons = new List<GameObject>();

        public void SetUpPanel(Player player)
        {
            playerName.text = player.PlayerName;
            playerIconImage.sprite = player.gameObject.GetComponent<SpriteRenderer>().sprite;
            while (bonuses.Count > 0)
            {
                RemoveBonusWithoutReorganize(bonuses.ElementAt(0).Key);
            }
            while (currentHealth > player.Hp)
            {
                RemoveHealth();
            }

            for (int i = currentHealth; i < player.Hp; i++)
            {
                AddHealth();
            }

            ScoreText.text = player.Score.ToString();

        }


        private void AddHealth()
        {
            currentHealth++;
            GameObject healthGameObject = new GameObject("health" + currentHealth, typeof(UnityEngine.UI.Image));
            healthGameObject.transform.SetParent(healthBarContainer.transform);
            Image healthImage = healthGameObject.GetComponent<Image>();
            RectTransform rectTransform = healthImage.GetComponent<RectTransform>();
            healthImage.sprite = healthSprite;
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = new Vector2(-30 + (currentHealth - 1) * 30, 0);
            rectTransform.sizeDelta = new Vector2(30, 30);
            healthIcons.Add(healthGameObject);
        }

        public void RemoveHealth()
        {
            if (currentHealth <= 0)
            {
                throw new NullReferenceException("No health to remove");
            }

            currentHealth--;
            Destroy(healthIcons[currentHealth]);
            healthIcons.RemoveAt(currentHealth);

        }


        public void AddBonus(BonusType type, Sprite bonusImage)
        {
            GameObject bonusGameObject = new GameObject("bonusIcon" + type.ToString(), typeof(UnityEngine.UI.Image));
            bonusGameObject.transform.SetParent(bonusesContainer.transform);
            Image bonusImageComp = bonusGameObject.GetComponent<Image>();
            RectTransform rectTransform = bonusImageComp.GetComponent<RectTransform>();
            bonusImageComp.sprite = bonusImage;
            rectTransform.anchorMin = new Vector2(0, 1f);
            rectTransform.anchorMax = new Vector2(0, 1f);
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = new Vector2(-40 + (bonuses.Count % 4) * 30, 20 - (bonuses.Count / 4) * 30);
            rectTransform.sizeDelta = new Vector2(30, 30);
            bonuses.Add(type, rectTransform);
            if (bonuses.Count > 8)
            {
                bonusGameObject.SetActive(false);
            }

            //bonuses.Add(type);
        }

        public void RemoveBonus(BonusType type)
        {

            RemoveBonusWithoutReorganize(type);
            ReorganizeBonuses();
        }

        private void ReorganizeBonuses()
        {
            int counter = 0;
            foreach (var bonus in bonuses)
            {
                if (counter >= 8)
                {
                    bonus.Value.gameObject.SetActive(false);

                }
                else
                {
                    bonus.Value.gameObject.SetActive(true);
                }
                bonus.Value.transform.localPosition = new Vector2(-40 + (counter % 4) * 30, 20 - (counter / 4) * 30);
                counter++;

            }
        }

        private void RemoveBonusWithoutReorganize(BonusType type)
        {
            Destroy(bonuses[type].gameObject);
            bonuses.Remove(type);
        }

    }
}