using System;
using _Scripts.Components.TimeManagement.Enums;
using UnityEngine;

//todo make time turn based ?
namespace _Scripts.Managers
{
    public class TimeManagerTurnBased : MonoBehaviour
    {
        public static TimeManagerTurnBased Instance;
    
        // Variables to store time
        public int seconds;
        public int minutes;
        public int hours;
        public int days;
        public int months;

        // Variable to store the time of day
        public TimeOfDay timeOfDay;

        // Variables to store formatted date and time
        public string formattedDate; // MM-DD
        public string formattedTime; // hh:mm:ss

        // Total elapsed time in seconds
        private float totalElapsedTime;
        

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        
            // Set initial time to January 1, 6:00 AM
            SetInitialTime(1, 1, 6, 0, 0); // Month, day, hour, minute, second
        }

        // Set the initial time (e.g., January 1, 6:00 AM)
        void SetInitialTime(int initialMonth, int initialDay, int initialHours, int initialMinutes, int initialSeconds)
        {
            // Calculate total time in seconds
            totalElapsedTime = initialSeconds + 
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
            seconds = (int)totalElapsedTime % 60;
            minutes = (int)totalElapsedTime / 60 % 60;
            hours = (int)totalElapsedTime / 3600 % 24;
            days = (int)totalElapsedTime / 86400 % 30; // Approx 30 days in a month
            months = (int)totalElapsedTime / 2592000; // Approx 30 days in a month
        }

        // Determine the time of day based on the current hour
        void DetermineTimeOfDay()
        {
            if (hours >= 6 && hours < 12)
            {
                timeOfDay = TimeOfDay.Morning;
            }
            else if (hours >= 12 && hours < 18)
            {
                timeOfDay = TimeOfDay.Afternoon;
            }
            else if (hours >= 18 && hours < 24)
            {
                timeOfDay = TimeOfDay.Evening;
            }
            else
            {
                timeOfDay = TimeOfDay.Night;
            }
            EventManager.Instance.TimeEvents.TimeOfDayChange(timeOfDay);
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
            totalElapsedTime += secondsToAdd;
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
        }

        // Add minutes to the current time
        public void AddMinutes(int minutesToAdd)
        {
            totalElapsedTime += minutesToAdd * 60;
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
        }

        // Add hours to the current time
        public void AddHours(int hoursToAdd)
        {
            totalElapsedTime += hoursToAdd * 3600;
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
        }

        // Add days to the current time
        public void AddDays(int daysToAdd)
        {
            totalElapsedTime += daysToAdd * 86400;
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
        }

        // Add months to the current time
        public void AddMonths(int monthsToAdd)
        {
            totalElapsedTime += monthsToAdd * 2592000; // Approx 30 days in a month
            UpdateTimeVariables();
            DetermineTimeOfDay();
            UpdateFormattedDateTime();
        }
    }
}