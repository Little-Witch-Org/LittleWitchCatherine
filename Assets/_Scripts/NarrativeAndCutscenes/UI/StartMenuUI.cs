using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.NarrativeAndCutscenes.UI
{
    public class StartMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject ContentParent;
        [SerializeField] private Button storyButton;
        [SerializeField] private Button devButton;
        
        [SerializeField] private bool isStartMenuActive;
        


        private void Start()
        {
            if (isStartMenuActive)
            {
                ContentParent.SetActive(true);
            }
        }


        public void LaunchStoryMode()
        {
            EventManager.Instance.GameEvents.StoryModActivated(true);
            StoryProgressionManager.Instance.ActivateCheckpoint(1);
            gameObject.SetActive(false);
        }
        
        public void LaunchDevMode()
        {
            EventManager.Instance.GameEvents.StoryModActivated(false);
            gameObject.SetActive(false);
        }
    }
}