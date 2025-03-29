using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _Scripts.QuestSystem
{
    public class QuestIcon : MonoBehaviour
    {


        [Header("Icons")] [SerializeField] private GameObject requirementNotMetToStartIcon;
        [Header("Icons")] [SerializeField] private GameObject canStartIcon;
        [Header("Icons")] [SerializeField] private GameObject requirementNotMetToFinishIcon;
        [Header("Icons")] [SerializeField] private GameObject canFinishIcon;


        public void SetState(QuestStateEnum newStateEnum, bool startPoint, bool finishPoint)
        {
            //set all to inactive
            requirementNotMetToStartIcon.SetActive(false);
            canStartIcon.SetActive(false);
            requirementNotMetToFinishIcon.SetActive(false);
            canFinishIcon.SetActive(false);
            
            //set the appropriate one to active based on the new stateEnum
            switch (newStateEnum)
            {
                case QuestStateEnum.RequirementsNotMet:
                    if(startPoint){requirementNotMetToStartIcon.SetActive(true);}
                    break;
                case QuestStateEnum.CanStart:
                    if(startPoint){canStartIcon.SetActive(true);}
                    break;
                case QuestStateEnum.InProgress:
                    if(finishPoint){requirementNotMetToFinishIcon.SetActive(true);}
                    break;
                case QuestStateEnum.CanFinish:
                    if(finishPoint){canFinishIcon.SetActive(true);}
                    break;
                case QuestStateEnum.Finished:
                    break;
                case QuestStateEnum.Failed:
                    break;
                default:
                    Debug.LogWarning("Quest stateEnum not recognized");
                    break;
            }
            
        }
        
        
        
    
    }
}