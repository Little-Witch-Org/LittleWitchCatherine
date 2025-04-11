using System;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    public class UiManager_Test : MonoBehaviour
    {
        public TMP_Text Exp;
        public TMP_Text Lvl;
        public TMP_Text Gold;


        private void OnEnable()
        {
            GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerExperienceChange+=UpdateExp;
            GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerLevelChange += UpdateLvl;
            GameEventsManager_Test.Instance.GoldEventsTest.OnGoldChange+=UpdateGold;
        }

        private void OnDisable()
        {
            GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerExperienceChange-=UpdateExp;
            GameEventsManager_Test.Instance.ExpEventsTest.OnPlayerLevelChange -= UpdateLvl;
            GameEventsManager_Test.Instance.GoldEventsTest.OnGoldChange-=UpdateGold;
        }


        private void UpdateExp(int exp)
        {
            Exp.text = "Exp: "+ exp;
        }
        
        private void UpdateLvl(int lvl)
        {
            Lvl.text = "LvL: "+ lvl;
        }
        
        private void UpdateGold(int gold)
        {
            Gold.text = "Gold: "+ gold;
        }
        
    }
}