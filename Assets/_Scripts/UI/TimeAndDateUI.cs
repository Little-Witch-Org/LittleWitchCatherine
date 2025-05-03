using _Scripts.Enums;
using _Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class TimeAndDateUI : MonoBehaviour
    {

        [SerializeField] private GameObject contentParent;
        
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text dateText;
        [SerializeField] private TMP_Text yearText;
        [SerializeField] private TMP_Text timeOfDayText;


        private void OnEnable()
        {
            EventManager.Instance.TimeEvents.OnUpdateTimeOfDay += UpdateTimeOfDayText;
            EventManager.Instance.TimeEvents.OnTimeChange += ChangeTimeText;
            EventManager.Instance.TimeEvents.OnDateChange += ChangeDateText;

            //EventManager.Instance.InputEvents.OnMenuPressed += ToggleCheatMenu;


        }

        private void OnDisable()
        {
            EventManager.Instance.TimeEvents.OnUpdateTimeOfDay -= UpdateTimeOfDayText;
            EventManager.Instance.TimeEvents.OnTimeChange -= ChangeTimeText;
            EventManager.Instance.TimeEvents.OnDateChange -= ChangeDateText;

            //EventManager.Instance.InputEvents.OnMenuPressed -= ToggleCheatMenu;

        }
        

        private void ChangeTimeText(string text)
        {
            timeText.text = text;
        }

        private void ChangeDateText(string text)
        {
            dateText.text = text;
        }

        //todo not supported
        private void ChangeYearText(string text)
        {
            yearText.text = text;
        }

        private void UpdateTimeOfDayText(TimeOfDayEnum timeOfDayEnum)
        {
            timeOfDayText.SetText(timeOfDayEnum.ToString());
        }
        
    }
    
}