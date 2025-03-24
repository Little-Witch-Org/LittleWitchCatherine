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


        public void SetState(QuestState_Test newStateTest, bool startPoint, bool finishPoint)
        {
            //set all to inactive
            requirementNotMetToStartIcon.SetActive(false);
            canStartIcon.SetActive(false);
            requirementNotMetToFinishIcon.SetActive(false);
            canFinishIcon.SetActive(false);
            
            //set the appropriate one to active based on the new stateTest
            switch (newStateTest)
            {
                case QuestState_Test.RequirementsNotMet:
                    if(startPoint){requirementNotMetToStartIcon.SetActive(true);}
                    break;
                case QuestState_Test.CanStart:
                    if(startPoint){canStartIcon.SetActive(true);}
                    break;
                case QuestState_Test.InProgress:
                    if(finishPoint){requirementNotMetToFinishIcon.SetActive(true);}
                    break;
                case QuestState_Test.CanFinish:
                    if(finishPoint){canFinishIcon.SetActive(true);}
                    break;
                case QuestState_Test.Finished:
                    break;
                default:
                    Debug.LogWarning("Quest_Test stateTest not recognized");
                    break;
            }
            
        }
        
        
        
    
    }
}