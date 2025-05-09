using System;
using System.Globalization;
using _Scripts.Managers;
using DG.Tweening;
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
        
        [Header("Stats UI")] 
        
        [Header("Health")]
        [SerializeField] private Image healthBarFill;
        [SerializeField] private Image healthBarPositiveFill;
        [SerializeField] private Image healthBarNegativeFill;
        
        [Header("Satiety")]
        [SerializeField] private Image satietyBarFill;
        [SerializeField] private Image satietyBarPositiveFill;
        [SerializeField] private Image satietyBarNegativeFill;
        
        [Header("Mood")]
        [SerializeField] private Image moodBarFill;
        [SerializeField] private Image moodBarPositiveFill;
        [SerializeField] private Image moodBarNegativeFill;
        
        [Header("Energy")]
        [SerializeField] private Image energyBarFill;
        [SerializeField] private Image energyBarPositiveFill;
        [SerializeField] private Image energyBarNegativeFill;
        
        [Header("Mother reputation")]
        [SerializeField] private Image motherReputationBarFill;
        [SerializeField] private Image motherReputationBarPositiveFill;
        [SerializeField] private Image motherReputationBarNegativeFill;
        
        [Header("Stats Menu")] 
        
        [SerializeField] private Image healthBarFillMenu;
        [SerializeField] private Image satietyBarFillMenu;
        [SerializeField] private Image moodBarFillMenu;
        [SerializeField] private Image energyBarFillMenu;
        [SerializeField] private Image motherReputationFillMenu;
        
        [Header("Stats Text")] 
        
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text satietyText;
        [SerializeField] private TMP_Text moodText;
        [SerializeField] private TMP_Text energyText;
        [SerializeField] private TMP_Text motherReputationText;
        
        [Header("Stats Change Rate")] 
        
        [SerializeField] private TMP_Text healthChangeRateText;
        [SerializeField] private TMP_Text satietyChangeRateText;
        [SerializeField] private TMP_Text moodChangeRateText;
        [SerializeField] private TMP_Text energyChangeRateText;



        private void Start()
        {
            //Initialization();
        }

        private void Initialization()
        {
            //healthBarPositiveFill.fillAmount = 0;
            //healthBarNegativeFill.fillAmount = 0;
            
            //_lastHealth = 100f;
            //_lastSatiety = 100f;
            //_lastMood = 100f;
            //_lastEnergy = 100f;
            //_lastMotherRep = 100f;
        }

        private void KillAllSequences()
        {
            _activeHealthSequence?.Kill();
            _activeSatietySequence?.Kill();
            _activeMoodSequence?.Kill();
            _activeEnergySequence?.Kill();
            
            _activeMotherRepSequence?.Kill();
        }
        
        private void OnEnable()
        {
            EventManager.Instance.PlayerStatsEvents.OnHealthChanged += UpdateHealthBar;
            EventManager.Instance.PlayerStatsEvents.OnSatietyChanged += UpdateSatietyBar;
            EventManager.Instance.PlayerStatsEvents.OnMoodChanged += UpdateMoodBar;
            EventManager.Instance.PlayerStatsEvents.OnEnergyChanged += UpdateEnergyBar;
            
            EventManager.Instance.ReputationEvents.OnReputationChanged += UpdateReputation;
            
            EventManager.Instance.PlayerStatsEvents.OnStatsChangeRateChanged += UpdateStatsChangeRate;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.PlayerStatsEvents.OnHealthChanged -= UpdateHealthBar;
            EventManager.Instance.PlayerStatsEvents.OnSatietyChanged -= UpdateSatietyBar;
            EventManager.Instance.PlayerStatsEvents.OnMoodChanged -= UpdateMoodBar;
            EventManager.Instance.PlayerStatsEvents.OnEnergyChanged -= UpdateEnergyBar;
            
            EventManager.Instance.ReputationEvents.OnReputationChanged -= UpdateReputation;
            
            EventManager.Instance.PlayerStatsEvents.OnStatsChangeRateChanged -= UpdateStatsChangeRate;
            
            
            KillAllSequences();
        }

        public void HideMenu()
        {
            contentParent.SetActive(false);
        }


        public void ShowMenu()
        {
            contentParent.SetActive(true);
        }
        
        //---stats
        //stats
        
        private float _lastHealth = 100f;
        private Sequence _activeHealthSequence;

        private void UpdateHealthBar(float newHealth)
        {
            const float maxHealth = 100f;
            float newFill = newHealth / maxHealth;
            float difference = Mathf.Round((_lastHealth - newHealth) / maxHealth * 10000f) / 10000f;

            _activeHealthSequence?.Kill();
            _activeHealthSequence = DOTween.Sequence();

            //immediately change for menu
            healthBarFillMenu.fillAmount = newFill;
            
            
            if (difference > 0.001f) // negative
            {
                // Negative to animate
                healthBarNegativeFill.fillAmount = _lastHealth / maxHealth;
                _activeHealthSequence
                    .AppendInterval(1.5f)
                    .Append(healthBarNegativeFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutSine)
                    );

                //positive and original immediately to target amount
                healthBarFill.fillAmount = newFill;
                healthBarPositiveFill.fillAmount = newFill;
            }
            else if (difference < -0.001f) // positive
            {
                // original to animate
                healthBarFill.fillAmount = _lastHealth / maxHealth;
                _activeHealthSequence
                    .AppendInterval(1.5f)
                    .Append(
                        healthBarFill.DOFillAmount(newFill, 1.5f)
                            .SetEase(Ease.OutSine)
                    );
                // positive and negative immediately to target amount (positive above negative in hierarchy)
                healthBarPositiveFill.fillAmount = newFill;
                healthBarNegativeFill.fillAmount = newFill;
            }
            else // very little amounts
            {
                //synchronize
                healthBarFill.fillAmount = newFill;
                healthBarNegativeFill.fillAmount = newFill;
                healthBarPositiveFill.fillAmount = newFill;
            }

            // update text
            healthText.text = $"{newHealth}/{maxHealth}";
            _lastHealth = newHealth;

            // launch animation
            _activeHealthSequence.Play();
        }


        /*private void UpdateHealthBar(float health)
        {
            var maxHealth = 100;
            healthBarFillMenu.fillAmount = healthBarFill.fillAmount = (float)health / maxHealth;
            healthText.text = "Здоровье: " + health + "/" + maxHealth;
        }*/
        
        private float _lastSatiety = 100f;
        private Sequence _activeSatietySequence;

        private void UpdateSatietyBar(float newSatiety)
        {
            const float maxSatiety = 100f;
            float newFill = newSatiety / maxSatiety;
            float difference = Mathf.Round((_lastSatiety - newSatiety) / maxSatiety * 10000f) / 10000f;

            _activeSatietySequence?.Kill();
            _activeSatietySequence = DOTween.Sequence();

            // Мгновенное обновление для меню
            satietyBarFillMenu.fillAmount = newFill;

            if (difference > 0.001f) // Уменьшение
            {
                satietyBarNegativeFill.fillAmount = _lastSatiety / maxSatiety;
                _activeSatietySequence
                    .AppendInterval(1.5f)
                    .Append(satietyBarNegativeFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutSine));
        
                satietyBarFill.fillAmount = newFill;
                satietyBarPositiveFill.fillAmount = newFill;
            }
            else if (difference < -0.001f) // Увеличение
            {
                satietyBarFill.fillAmount = _lastSatiety / maxSatiety;
                _activeSatietySequence
                    .AppendInterval(1.5f)
                    .Append(satietyBarFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutSine));
            
                satietyBarPositiveFill.fillAmount = newFill;
                satietyBarNegativeFill.fillAmount = newFill;
            }
            else
            {
                satietyBarFill.fillAmount = newFill;
                satietyBarNegativeFill.fillAmount = newFill;
                satietyBarPositiveFill.fillAmount = newFill;
            }

            satietyText.text = $"{newSatiety}/{maxSatiety}";
            _lastSatiety = newSatiety;
            _activeSatietySequence.Play();
        }
        
        /*private void UpdateSatietyBar(float satiety)
        {
            var maxSatiety = 100;
            satietyBarFillMenu.fillAmount = satietyBarFill.fillAmount = (float)satiety / maxSatiety;
            satietyText.text = "Сытость: " + satiety + "/" + maxSatiety;
        }*/
        
        private float _lastMood = 100f;
        private Sequence _activeMoodSequence;

        private void UpdateMoodBar(float newMood)
        {
            const float maxMood = 100f;
            float newFill = newMood / maxMood;
            float difference = Mathf.Round((_lastMood - newMood) / maxMood * 10000f) / 10000f;

            _activeMoodSequence?.Kill();
            _activeMoodSequence = DOTween.Sequence();

            moodBarFillMenu.fillAmount = newFill;

            if (difference > 0.001f)
            {
                moodBarNegativeFill.fillAmount = _lastMood / maxMood;
                _activeMoodSequence
                    .AppendInterval(1.5f)
                    .Append(moodBarNegativeFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutSine));
        
                moodBarFill.fillAmount = newFill;
                moodBarPositiveFill.fillAmount = newFill;
            }
            else if (difference < -0.001f)
            {
                moodBarFill.fillAmount = _lastMood / maxMood;
                _activeMoodSequence
                    .AppendInterval(1.5f)
                    .Append(moodBarFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutSine));
            
                moodBarPositiveFill.fillAmount = newFill;
                moodBarNegativeFill.fillAmount = newFill;
            }
            else
            {
                moodBarFill.fillAmount = newFill;
                moodBarNegativeFill.fillAmount = newFill;
                moodBarPositiveFill.fillAmount = newFill;
            }

            moodText.text = $"{newMood}/{maxMood}";
            _lastMood = newMood;
            _activeMoodSequence.Play();
        }
        
        /*private void UpdateMoodBar(float mood)
        {
            var maxMood = 100;
            moodBarFillMenu.fillAmount = moodBarFill.fillAmount = (float)mood / maxMood;
            moodText.text = "Настроение: " + mood + "/" + maxMood;
        }*/
        
        private float _lastEnergy = 100f;
        private Sequence _activeEnergySequence;

        private void UpdateEnergyBar(float newEnergy)
        {
            const float maxEnergy = 100f;
            float newFill = newEnergy / maxEnergy;
            float difference = Mathf.Round((_lastEnergy - newEnergy) / maxEnergy * 10000f) / 10000f;

            _activeEnergySequence?.Kill();
            _activeEnergySequence = DOTween.Sequence();

            energyBarFillMenu.fillAmount = newFill;

            if (difference > 0.001f)
            {
                energyBarNegativeFill.fillAmount = _lastEnergy / maxEnergy;
                _activeEnergySequence
                    .AppendInterval(1.5f)
                    .Append(energyBarNegativeFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutSine));
        
                energyBarFill.fillAmount = newFill;
                energyBarPositiveFill.fillAmount = newFill;
            }
            else if (difference < -0.001f)
            {
                energyBarFill.fillAmount = _lastEnergy / maxEnergy;
                _activeEnergySequence
                    .AppendInterval(1.5f)
                    .Append(energyBarFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutSine));
            
                energyBarPositiveFill.fillAmount = newFill;
                energyBarNegativeFill.fillAmount = newFill;
            }
            else
            {
                energyBarFill.fillAmount = newFill;
                energyBarNegativeFill.fillAmount = newFill;
                energyBarPositiveFill.fillAmount = newFill;
            }

            energyText.text = $"{newEnergy}/{maxEnergy}";
            _lastEnergy = newEnergy;
            _activeEnergySequence.Play();
        }
        
        /*private void UpdateEnergyBar(float energy)
        {
            var maxEnergy = 100;
            energyBarFillMenu.fillAmount =  energyBarFill.fillAmount = (float)energy / maxEnergy;
            energyText.text = "Энергия: " + energy + "/" + maxEnergy;
        }*/
        
        //rate

        private void UpdateStatsChangeRate(float healthChangeRate, float satietyChangeRate, float moodChangeRate, float energyChangeRate)
        {
            healthChangeRateText.text = healthChangeRate.ToString(CultureInfo.InvariantCulture);
            satietyChangeRateText.text = satietyChangeRate.ToString(CultureInfo.InvariantCulture);
            moodChangeRateText.text = moodChangeRate.ToString(CultureInfo.InvariantCulture);
            energyChangeRateText.text = energyChangeRate.ToString(CultureInfo.InvariantCulture);
        }
        
        //---reputation
        private void UpdateReputation(string npcName, float reputation)
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
        
        private float _lastMotherRep = 100f;
        private Sequence _activeMotherRepSequence;

        private void UpdateMotherReputationBar(float newRep)
        {
            const float maxRep = 100f;
            float newFill = newRep / maxRep;
            float difference = Mathf.Round((_lastMotherRep - newRep) / maxRep * 10000f) / 10000f;

            _activeMotherRepSequence?.Kill();
            _activeMotherRepSequence = DOTween.Sequence();

            motherReputationFillMenu.fillAmount = newFill;

            if (difference > 0.001f)
            {
                motherReputationBarNegativeFill.fillAmount = _lastMotherRep / maxRep;
                _activeMotherRepSequence
                    .AppendInterval(1.5f)
                    .Append(motherReputationBarNegativeFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutSine));
        
                motherReputationBarFill.fillAmount = newFill;
                motherReputationBarPositiveFill.fillAmount = newFill;
            }
            else if (difference < -0.001f)
            {
                motherReputationBarFill.fillAmount = _lastMotherRep / maxRep;
                _activeMotherRepSequence
                    .AppendInterval(1.5f)
                    .Append(motherReputationBarFill.DOFillAmount(newFill, 1.5f).SetEase(Ease.OutSine));
            
                motherReputationBarPositiveFill.fillAmount = newFill;
                motherReputationBarNegativeFill.fillAmount = newFill;
            }
            else
            {
                motherReputationBarFill.fillAmount = newFill;
                motherReputationBarNegativeFill.fillAmount = newFill;
                motherReputationBarPositiveFill.fillAmount = newFill;
            }

            motherReputationText.text = $"{newRep}/{maxRep}";
            _lastMotherRep = newRep;
            _activeMotherRepSequence.Play();
        }
        
        /*private void UpdateMotherReputationBar(float reputation)
        {
            var maxRep = 100;
            motherReputationFillMenu.fillAmount =  motherReputationFill.fillAmount = (float)reputation / maxRep;
            motherReputationText.text = "Мама: " + reputation + "/" + maxRep;
        }*/
        
        //buttons
        public void UpdateHealthForButton(float health)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateHealth(health);
        }
        public void UpdateSatietyForButton(float satiety)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateSatiety(satiety);
        }
        public void UpdateMoodForButton(float mood)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateMood(mood);
        }
        public void UpdateEnergyForButton(float energy)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateEnergy(energy);
        }
        public void UpdateMotherReputationForButton(float reputation)
        {
            EventManager.Instance.ReputationEvents.UpdateReputation("Mother", reputation);
        }
    }
    
}