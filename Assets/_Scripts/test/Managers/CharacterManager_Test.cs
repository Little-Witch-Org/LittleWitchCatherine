using System;
using UnityEngine;

namespace _Scripts
{
    public class CharacterManager_Test : MonoBehaviour
    {
        public int lvl = 0;

        public int experience = 0;

        public int gold = 0;


        private void OnEnable()
        {
            GameEventsManager_Test.Instance.ExpEventsTest.OnExperienceGained += Leveling;
            GameEventsManager_Test.Instance.GoldEventsTest.OnGoldGained += GoldCollecting;
        }

        private void OnDisable()
        {
            GameEventsManager_Test.Instance.ExpEventsTest.OnExperienceGained -= Leveling;
            GameEventsManager_Test.Instance.GoldEventsTest.OnGoldGained -= GoldCollecting;
        }

        private void Leveling(int exp)
        {
            experience += exp;
            //Debug.Log("char manager exp gain = " + exp);

            if (experience >= 30)
            {
                lvl = 1;
            }
            if (experience >= 60)
            {
                lvl = 2;
            }

            GameEventsManager_Test.Instance.ExpEventsTest.PlayerExperienceChange(experience);
            GameEventsManager_Test.Instance.ExpEventsTest.PlayerLevelChange(lvl);
        }

        private void GoldCollecting(int goldCount)
        {
            gold += goldCount;
            GameEventsManager_Test.Instance.GoldEventsTest.GoldChange(gold);
        }
    }
}