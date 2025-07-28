using System;
using _Scripts.Enums;
using _Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.UI
{
    public class TimeAndDateUI : MonoBehaviour
    {

        [SerializeField] private GameObject contentParent;
        
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text dateText;
        [SerializeField] private TMP_Text yearText;
        [SerializeField] private TMP_Text timeOfDayText;

        [SerializeField] private GameObject minuteArrow;
        [SerializeField] private GameObject hourArrow;
        private readonly float _minuteArrowAngleOffset = 72f;
        private readonly float _hourArrowAngleOffset = 17f;


        private void OnEnable()
        {
            EventManager.Instance.TimeEvents.OnUpdateTimeOfDay += UpdateTimeOfDayText;
            EventManager.Instance.TimeEvents.OnTimeChange += ChangeTimeText;
            EventManager.Instance.TimeEvents.OnDateChange += ChangeDateText;
            
            EventManager.Instance.TimeEvents.OnMinutesAmountChanged += ChangeArrowAngle;
            EventManager.Instance.TimeEvents.OnHoursAmountChanged += ChangeHourArrowAngle;
            
            EventManager.Instance.TimeEvents.OnMinutesSetChanged += ChangeArrowAngle;
            EventManager.Instance.TimeEvents.OnHoursSetChanged += ChangeHourArrowAngle;

            //EventManager.Instance.InputEvents.OnMenuPressed += ToggleCheatMenu;
        }

        private void OnDisable()
        {
            EventManager.Instance.TimeEvents.OnUpdateTimeOfDay -= UpdateTimeOfDayText;
            EventManager.Instance.TimeEvents.OnTimeChange -= ChangeTimeText;
            EventManager.Instance.TimeEvents.OnDateChange -= ChangeDateText;
            
            EventManager.Instance.TimeEvents.OnMinutesAmountChanged -= ChangeArrowAngle;
            EventManager.Instance.TimeEvents.OnHoursAmountChanged -= ChangeHourArrowAngle;
            
            EventManager.Instance.TimeEvents.OnMinutesSetChanged -= ChangeArrowAngle;
            EventManager.Instance.TimeEvents.OnHoursSetChanged -= ChangeHourArrowAngle;

            //EventManager.Instance.InputEvents.OnMenuPressed -= ToggleCheatMenu;

        }

        //set current time on the watch from start
        private void Start()
        {
            ChangeArrowAngle(0);
            ChangeHourArrowAngle(0);
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

        private void ChangeArrowAngle(int time) //not using param
        {
            
            //Debug.Log($"minute changed = {time}");
            //Debug.Log(TimeManager.Instance.GetMinutes());
            
            var currentMinutes = TimeManager.Instance.GetMinutes();
            int minuteStepAngle = 6;
            var targetAngle =  _minuteArrowAngleOffset - (minuteStepAngle * currentMinutes);
            //Debug.Log(targetAngle);

            minuteArrow.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, targetAngle);
        }

        private void ChangeHourArrowAngle(int time)
        {
            //Debug.Log($"hour changed = {time}");
            //Debug.Log(TimeManager.Instance.GetHours());
            
            var currentHours = TimeManager.Instance.GetHours();
            int hourStepAngle = 30;
            var targetAngle =  _hourArrowAngleOffset - (hourStepAngle * currentHours);
            //Debug.Log(targetAngle);

            hourArrow.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, targetAngle);
        }
        
    }
    
}