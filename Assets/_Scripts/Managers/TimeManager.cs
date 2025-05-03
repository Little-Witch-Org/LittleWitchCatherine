using System;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.Serialization;

//todo add events and incapsulate
namespace _Scripts.Managers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance;
    
        // Variables to store time
        public int seconds;
        public int minutes;
        public int hours;
        public int days;
        public int months;

        // Variable to store the time of day
        public TimeOfDayEnum timeOfDayEnum;

        // Variables to store formatted date and time
        public string formattedDate; // MM-DD
        public string formattedTime; // hh:mm:ss

        // Total elapsed time in seconds
        private float _totalElapsedTime;


        private void OnEnable()
        {
            EventManager.Instance.TimeEvents.OnUpdateSeconds += AddSeconds;
            EventManager.Instance.TimeEvents.OnUpdateMinutes += AddMinutes;
            EventManager.Instance.TimeEvents.OnUpdateHours += AddHours;
            EventManager.Instance.TimeEvents.OnUpdateDays += AddDays;
        }
        private void OnDisable()
        {
            EventManager.Instance.TimeEvents.OnUpdateSeconds -= AddSeconds;
            EventManager.Instance.TimeEvents.OnUpdateMinutes -= AddMinutes;
            EventManager.Instance.TimeEvents.OnUpdateHours -= AddHours;
            EventManager.Instance.TimeEvents.OnUpdateDays -= AddDays;
        }


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // Set initial time to January 1, 6:00 AM
                SetInitialTime(1, 1, 6, 0, 0); // Month, day, hour, minute, second
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        

        }

        // Set the initial time (e.g., January 1, 6:00 AM)
        public void SetInitialTime(int initialMonth, int initialDay, int initialHours, int initialMinutes, int initialSeconds)
        {
            // Calculate total time in seconds
            _totalElapsedTime = initialSeconds + 
                               initialMinutes * 60 + 
                               initialHours * 3600 + 
                               (initialDay - 1) * 86400 + 
                               (initialMonth - 1) * 2592000; // Approx 30 days in a month
        
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
        }

        // Update time variables based on total elapsed time
        void UpdateTimeVariables()
        {
            seconds = (int)_totalElapsedTime % 60;
            minutes = (int)_totalElapsedTime / 60 % 60;
            hours = (int)_totalElapsedTime / 3600 % 24;
            days = (int)_totalElapsedTime / 86400 % 30; // Approx 30 days in a month
            months = (int)_totalElapsedTime / 2592000; // Approx 30 days in a month
        }

        // Determine the time of day based on the current hour
        void DetermineTimeOfDay()
        {
            if (hours >= 6 && hours < 12)
            {
                timeOfDayEnum = TimeOfDayEnum.Morning;
            }
            else if (hours >= 12 && hours < 18)
            {
                timeOfDayEnum = TimeOfDayEnum.Afternoon;
            }
            else if (hours >= 18 && hours < 24)
            {
                timeOfDayEnum = TimeOfDayEnum.Evening;
            }
            else
            {
                timeOfDayEnum = TimeOfDayEnum.Night;
            }
            EventManager.Instance.TimeEvents.UpdateTimeOfDay(timeOfDayEnum);
        }

        // Update formatted date (MM-DD) and time (hh:mm:ss)
        void UpdateFormattedDateTime()
        {
            formattedDate = $"{months + 1:00}-{days + 1:00}"; // Months and days are 1-based
            formattedTime = $"{hours:00}:{minutes:00}:{seconds:00}";
        
            EventManager.Instance.TimeEvents.DateChange(formattedDate);
            EventManager.Instance.TimeEvents.TimeChange(formattedTime);
        }

        // Add seconds to the current time
        public void AddSeconds(int secondsToAdd)
        {
            _totalElapsedTime += secondsToAdd;
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
            
            EventManager.Instance.TimeEvents.SecondsChange(secondsToAdd);
        }

        // Add minutes to the current time
        public void AddMinutes(int minutesToAdd)
        {
            _totalElapsedTime += minutesToAdd * 60;
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
            
            EventManager.Instance.TimeEvents.MinutesChange(minutesToAdd);
        }

        // Add hours to the current time
        public void AddHours(int hoursToAdd)
        {
            _totalElapsedTime += hoursToAdd * 3600;
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
            
            EventManager.Instance.TimeEvents.HoursChange(hoursToAdd);
        }

        // Add days to the current time
        public void AddDays(int daysToAdd)
        {
            _totalElapsedTime += daysToAdd * 86400;
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
            
            EventManager.Instance.TimeEvents.DaysChange(daysToAdd);
        }

        // Add months to the current time
        public void AddMonths(int monthsToAdd)
        {
            _totalElapsedTime += monthsToAdd * 2592000; // Approx 30 days in a month
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
        }
    }
}