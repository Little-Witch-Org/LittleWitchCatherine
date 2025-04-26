using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class PlayerStatsUI:MonoBehaviour,IMenu
    {
        [SerializeField] private GameObject contentParent;
        public GameObject ContentParent => contentParent;
        
        [SerializeField] private Image healthBarFill;
        [SerializeField] private Image saturationBarFill;
        [SerializeField] private Image moodBarFill;
        [SerializeField] private Image energyBarFill;
        [SerializeField] private Image motherReputationFill;
        
        [SerializeField] private Image healthBarFillMenu;
        [SerializeField] private Image saturationBarFillMenu;
        [SerializeField] private Image moodBarFillMenu;
        [SerializeField] private Image energyBarFillMenu;
        [SerializeField] private Image motherReputationFillMenu;
        
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text saturationText;
        [SerializeField] private TMP_Text moodText;
        [SerializeField] private TMP_Text energyText;
        [SerializeField] private TMP_Text motherReputationText;

        private void OnEnable()
        {
            EventManager.Instance.PlayerStatsEvents.OnHealthChanged += UpdateHealthBar;
            EventManager.Instance.PlayerStatsEvents.OnSaturationChanged += UpdateSaturationBar;
            EventManager.Instance.PlayerStatsEvents.OnMoodChanged += UpdateMoodBar;
            EventManager.Instance.PlayerStatsEvents.OnEnergyChanged += UpdateEnergyBar;
            
            EventManager.Instance.ReputationEvents.OnReputationChanged += UpdateReputation;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.PlayerStatsEvents.OnHealthChanged -= UpdateHealthBar;
            EventManager.Instance.PlayerStatsEvents.OnSaturationChanged -= UpdateSaturationBar;
            EventManager.Instance.PlayerStatsEvents.OnMoodChanged -= UpdateMoodBar;
            EventManager.Instance.PlayerStatsEvents.OnEnergyChanged -= UpdateEnergyBar;
            
            EventManager.Instance.ReputationEvents.OnReputationChanged -= UpdateReputation;
        }

        public void HideMenu()
        {
            contentParent.SetActive(false);
        }


        public void ShowMenu()
        {
            contentParent.SetActive(true);
        }
        
        //stats
        private void UpdateHealthBar(int health)
        {
            var maxHealth = 100;
            healthBarFillMenu.fillAmount = healthBarFill.fillAmount = (float)health / maxHealth;
            healthText.text = "Здоровье: " + health + "/" + maxHealth;
        }
        
        private void UpdateSaturationBar(int saturation)
        {
            var maxSaturation = 100;
            saturationBarFillMenu.fillAmount = saturationBarFill.fillAmount = (float)saturation / maxSaturation;
            saturationText.text = "Насыщение: " + saturation + "/" + maxSaturation;
        }
        
        private void UpdateMoodBar(int mood)
        {
            var maxMood = 100;
            moodBarFillMenu.fillAmount = moodBarFill.fillAmount = (float)mood / maxMood;
            moodText.text = "Настроение: " + mood + "/" + maxMood;
        }
        
        private void UpdateEnergyBar(int energy)
        {
            var maxEnergy = 100;
            energyBarFillMenu.fillAmount =  energyBarFill.fillAmount = (float)energy / maxEnergy;
            energyText.text = "Энергия: " + energy + "/" + maxEnergy;
        }
        
        //reputation
        private void UpdateReputation(string npcName, int reputation)
        {
            switch (npcName)
            {
                case "Mother":
                {
                    UpdateMotherReputationBar(reputation);
                    break;
                }
            }
        }
        
        private void UpdateMotherReputationBar(int reputation)
        {
            var maxRep = 100;
            motherReputationFillMenu.fillAmount =  motherReputationFill.fillAmount = (float)reputation / maxRep;
            motherReputationText.text = "Мама: " + reputation + "/" + maxRep;
        }
    }
    
}