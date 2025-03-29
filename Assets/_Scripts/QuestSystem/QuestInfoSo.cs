using UnityEngine;

namespace _Scripts.QuestSystem
{
    [CreateAssetMenu(fileName = "QuestInfoSo", menuName = "ScriptableObjects/QuestInfoSo",order = 1)]
    public class QuestInfoSo : ScriptableObject
    {
        [field:SerializeField] public string Id {get; private set;}

        [Header("General")]
        public string displayName;
        public string displayDescription;
        
        [Header("Requirements")]
        public bool isQuestAvailable; //quest on/off
        //todo add custom requirements - quest manager need to get ture or false. custom req class? how to add req update?
        //todo on quest point ?
        
        public QuestInfoSo[] questPrerequisites;
       
        [Header("Steps")]
        public GameObject[] questStepPrefabs;

        
        [Header("Completion Method")]
        public bool isPartialCompletionPossible;
        
        [Header("Rewards")] 
        public string reward;
        //public int goldReward; //for test
        //public int expReward; //for test
        //todo add custom reward class
        
        //todo rewards for partial complete ? Calculate it in reward class?
        
        
        //ensure the id is always the name of the SO asset
        private void OnValidate()
        {
            #if UNITY_EDITOR
            Id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}