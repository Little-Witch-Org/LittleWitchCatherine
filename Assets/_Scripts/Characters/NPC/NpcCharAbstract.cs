using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace _Scripts.Characters.NPC
{
    public abstract class NpcCharAbstract : MonoBehaviour
    {
        //-in dialogue component- contains list of mainStories and list(dictionaries ?) of knots
        //-in dialogue component- must subscribe on knot finish event (to create and add to knots) -> this is signal that unic dialog was played end ended
        //-in dialogue component- current (actual?) story
        //-in dialogue component- current (actual?) knot //todo do we need to follow step by step?
        //-in dialogue component- get current story and get current knot - for dialogue component
        //some unic variables (mb in the script realization)

        //start point of dialogues and quests. Click on npc  (or trigger inner dialogue) -> open actual dialogue from component -> start quest if it actual (quest point must subscribe?)

        [SerializeField] protected string npcName;
        [SerializeField] protected string currentLocationName;
        [SerializeField] protected string currentPlaceName;

        [SerializeField] protected GameObject npcBody;
        
        [SerializeField] protected int maxReputation;
        [SerializeField] protected int currentReputation;

        private void Start()
        {
            maxReputation = 100;
            currentReputation = 60;
            EventManager.Instance.ReputationEvents.ReputationChanged(npcName,currentReputation);
        }

        private void OnEnable()
        {
            EventManager.Instance.ReputationEvents.OnUpdateReputation += UpdateReputation;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.ReputationEvents.OnUpdateReputation -= UpdateReputation;
        }

        public string GetNpcName()
        {
            return npcName;
        }

        public string GetCurrentLocationName()
        {
            return currentLocationName;
        }

        public string GetCurrentPlaceName()
        {
            return currentPlaceName;
        }

        public void EnableBody()
        {
            npcBody.SetActive(true);
        }

        public void DisableBody()
        {
            npcBody.SetActive(false);
        }

        public bool IsNpcActive()
        {
            return npcBody.activeSelf;
        }
        
        //rep
        private void UpdateReputation(string npc, int reputation)
        {
            if (npcName.Equals(npc))
            {
                currentReputation = Math.Clamp(currentReputation + reputation, 0, maxReputation);
                EventManager.Instance.ReputationEvents.ReputationChanged(npcName,currentReputation);
            }
        }
    }
}