using _Scripts.Enums;
using _Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class CheatMenuUI : MonoBehaviour,IMenu
    {
        [SerializeField] private GameObject contentParent;
        
        [SerializeField] private SpriteRenderer  spriteRenderer;
        [SerializeField] private  Slider transparencySlider;
        
        
        
        public GameObject ContentParent => contentParent;

        void Start()
        {
            transparencySlider.onValueChanged.AddListener(UpdateFilterTransparency);
            UpdateFilterTransparency(transparencySlider.value);
            
        }
        
        public void HideMenu()
        {
            contentParent.SetActive(false);
        }


        public void ShowMenu()
        {
            contentParent.SetActive(true);
        }

        public void SetTimeToMorning()
        {
            TimeManager.Instance.SetInitialTime(10,10,6,0,0);
        }
        public void SetTimeToAfternoon()
        {
            TimeManager.Instance.SetInitialTime(10,10,12,0,0);
        }
        public void SetTimeToEvening()
        {
            TimeManager.Instance.SetInitialTime(10,10,18,0,0);
        }
        public void SetTimeToNight()
        {
            TimeManager.Instance.SetInitialTime(10,10,0,0,0);
        }
        
        void UpdateFilterTransparency(float value)
        {
            Color color = spriteRenderer.color;
            color.a = value;
            spriteRenderer.color = color;
        }

        public void ChangeLanguage(int index)
        {
            switch (index)
            {
                case 0:
                {
                    LocalizationManager.Instance.SetLanguage("ru");
                    break;
                }
                case 1:
                {
                    LocalizationManager.Instance.SetLanguage("en");
                    break;
                }
            }
        }
        
        
        //teleports
        
        public void TeleportPlayerToNovelPlace(string location, string place)
        {
            //PlayerCharacterManager.Instance.SetPreviousSpawnPositionPoint(pointNamesName); //
            EventManager.Instance.TransitionEvents.PlaceTransitionTrigger(location, place);
            EventManager.Instance.TransitionEvents.ChangeScene(SceneNamesEnum.NovelView);
            HideMenu();
        }

        public void TeleportToFFCorridor()
        {
            TeleportPlayerToNovelPlace("CatherineHouse", "FFCorridor");
        }
        
        public void TeleportToSFCorridor()
        {
            TeleportPlayerToNovelPlace("CatherineHouse", "SFCorridor");
        }
        
        public void TeleportToTFCorridor()
        {
            TeleportPlayerToNovelPlace("CatherineHouse", "TFCorridor");
        }
        
        public void TeleportToCatherineRoom()
        {
            TeleportPlayerToNovelPlace("CatherineHouse", "CatherineRoom");
        }

        
        
        
        
    }
}