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
    /// <summary>
    /// Handles the player ingame menus
    /// </summary>
    public class PlayerInGameMenuHandler : MonoBehaviour
    {
        /// <summary>
        /// The sprite of the health icons
        /// </summary>
        [SerializeField]
        Sprite healthSprite;

        /// <summary>
        /// Where to put the health icons
        /// </summary>
        [SerializeField]
        GameObject healthBarContainer;

        /// <summary>
        /// Where to put the bonus icons
        /// </summary>
        [SerializeField]
        GameObject bonusesContainer;

        /// <summary>
        /// Player skin display
        /// </summary>
        [SerializeField]
        UnityEngine.UI.Image playerIconImage;

        /// <summary>
        /// Player name display
        /// </summary>
        [SerializeField]
        TMP_Text playerName;

        /// <summary>
        /// Player score display
        /// </summary>
        [SerializeField]
        TMP_Text ScoreText;

        /// <summary>
        /// The current health which are displayed
        /// </summary>
        private int currentHealth = 0;

        /// <summary>
        /// The bonuses which are displayed
        /// </summary>
        private Dictionary<BonusType, RectTransform> bonuses = new Dictionary<BonusType, RectTransform>();

        /// <summary>
        /// The health icons which are displayed right now
        /// </summary>
        private List<GameObject> healthIcons = new List<GameObject>();

        /// <summary>
        /// Displays the player with it's current health
        /// Should be used for reseting only!
        /// </summary>
        /// <param name="player">The player to display</param>
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

        /// <summary>
        /// Add a new health icon to the display
        /// </summary>
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

        /// <summary>
        /// Remove a health from the display
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
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

        /// <summary>
        /// Adds a new bonus to the display
        /// </summary>
        /// <param name="type">The type of the bonus to display</param>
        /// <param name="bonusImage">The bonus's image to display</param>
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

        /// <summary>
        /// Removes a bonus from the display
        /// </summary>
        /// <param name="type">The bonus type to remove</param>
        public void RemoveBonus(BonusType type)
        {

            RemoveBonusWithoutReorganize(type);
            ReorganizeBonuses();
        }

        /// <summary>
        /// Reorganizes the bonuses positions
        /// </summary>
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

        /// <summary>
        /// Removes a bonus without reorganizing the displays
        /// </summary>
        /// <param name="type">The bonus to remove</param>
        private void RemoveBonusWithoutReorganize(BonusType type)
        {
            Destroy(bonuses[type].gameObject);
            bonuses.Remove(type);
        }

    }
}