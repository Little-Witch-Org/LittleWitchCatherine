using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _Scripts.test.QuestSystem
{
    public class QuestIcon : MonoBehaviour
    {


        [Header("Icons")] [SerializeField] private GameObject requirementNotMetToStartIcon;
        [Header("Icons")] [SerializeField] private GameObject canStartIcon;
        [Header("Icons")] [SerializeField] private GameObject requirementNotMetToFinishIcon;
        [Header("Icons")] [SerializeField] private GameObject canFinishIcon;


        public void SetState(QuestState newState, bool startPoint, bool finishPoint)
        {
            //set all to inactive
            requirementNotMetToStartIcon.SetActive(false);
            canStartIcon.SetActive(false);
            requirementNotMetToFinishIcon.SetActive(false);
            canFinishIcon.SetActive(false);
            
            //set the appropriate one to active based on the new state
            switch (newState)
            {
                case QuestState.RequirementsNotMet:
                    if(startPoint){requirementNotMetToStartIcon.SetActive(true);}
                    break;
                case QuestState.CanStart:
                    if(startPoint){canStartIcon.SetActive(true);}
                    break;
                case QuestState.InProgress:
                    if(finishPoint){requirementNotMetToFinishIcon.SetActive(true);}
                    break;
                case QuestState.CanFinish:
                    if(finishPoint){canFinishIcon.SetActive(true);}
                    break;
                case QuestState.Finished:
                    break;
                default:
                    Debug.LogWarning("Quest state not recognized");
                    break;
            }
            
        }
        
        
        
    
    }
}