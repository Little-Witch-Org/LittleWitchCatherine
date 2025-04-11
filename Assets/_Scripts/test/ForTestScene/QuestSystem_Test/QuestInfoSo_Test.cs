using UnityEngine;

namespace _Scripts.test.QuestSystem
{
    [CreateAssetMenu(fileName = "QuestInfoSo_Test", menuName = "ScriptableObjects/QuestInfoSo_Test",order = 1)]
    public class QuestInfoSo_Test : ScriptableObject
    {
        [field:SerializeField] public string Id {get; private set;}

        [Header("General")]
        public string displayName;
        
        [Header("Requirements")]
        public int levelRequirement; //for test
        
        public QuestInfoSo_Test[] questPrerequisites;
       
        [Header("Steps")]
        public GameObject[] questStepPrefabs;

        [Header("Rewards")]
        public int goldReward; //for test
        public int expReward; //for test
        //todo add custom reward class
        
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