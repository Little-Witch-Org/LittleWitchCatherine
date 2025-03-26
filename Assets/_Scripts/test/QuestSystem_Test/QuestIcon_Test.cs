using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _Scripts.test.QuestSystem
{
    public class QuestIcon_Test : MonoBehaviour
    {


        [Header("Icons")] [SerializeField] private GameObject requirementNotMetToStartIcon;
        [Header("Icons")] [SerializeField] private GameObject canStartIcon;
        [Header("Icons")] [SerializeField] private GameObject requirementNotMetToFinishIcon;
        [Header("Icons")] [SerializeField] private GameObject canFinishIcon;


        public void SetState(QuestStateEnum_Test newStateEnumTest, bool startPoint, bool finishPoint)
        {
            //set all to inactive
            requirementNotMetToStartIcon.SetActive(false);
            canStartIcon.SetActive(false);
            requirementNotMetToFinishIcon.SetActive(false);
            canFinishIcon.SetActive(false);
            
            //set the appropriate one to active based on the new stateEnumTest
            switch (newStateEnumTest)
            {
                case QuestStateEnum_Test.RequirementsNotMet:
                    if(startPoint){requirementNotMetToStartIcon.SetActive(true);}
                    break;
                case QuestStateEnum_Test.CanStart:
                    if(startPoint){canStartIcon.SetActive(true);}
                    break;
                case QuestStateEnum_Test.InProgress:
                    if(finishPoint){requirementNotMetToFinishIcon.SetActive(true);}
                    break;
                case QuestStateEnum_Test.CanFinish:
                    if(finishPoint){canFinishIcon.SetActive(true);}
                    break;
                case QuestStateEnum_Test.Finished:
                    break;
                default:
                    Debug.LogWarning("Quest_Test stateEnumTest not recognized");
                    break;
            }
            
        }
        
        
        
    
    }
}