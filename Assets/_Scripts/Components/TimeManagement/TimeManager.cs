using System;
using _Scripts.Components.TimeManagement.Enums;
using Unity.VisualScripting;
using UnityEngine;

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
    public TimeOfDay timeOfDay;

    // Variables to store formatted date and time
    public string formattedDate; // MM-DD
    public string formattedTime; // hh:mm:ss

    // Start time of the stopwatch
    private float startTime;
    
    //adding events for using by different systems in game
    public Action<string> OnTimeChange;
    public Action<string> OnDateChange;
    public Action<string> OnTimeOfDayChange;

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

    void Update()
    {
        // Calculate elapsed time since the start
        float elapsedTime = Time.time - startTime;

        // Update time variables based on elapsed time
        UpdateTime(elapsedTime);

        // Determine the current time of day
        DetermineTimeOfDay();

        // Update formatted date and time
        UpdateFormattedDateTime();
        
    }

    // Set the initial time (e.g., January 1, 6:00 AM)
    void SetInitialTime(int initialMonth, int initialDay, int initialHours, int initialMinutes, int initialSeconds)
    {
        // Convert initial time to total seconds
        int totalSeconds = initialHours * 3600 + initialMinutes * 60 + initialSeconds;
        int totalDays = (initialDay - 1) * 86400; // Days are 1-based
        int totalMonths = (initialMonth - 1) * 2592000; // Approx 30 days in a month

        // Adjust startTime so that the current time matches the initial time
        startTime = Time.time - (totalSeconds + totalDays + totalMonths);
    }

    // Update time variables based on elapsed time
    void UpdateTime(float elapsedTime)
    {
        seconds = (int)elapsedTime % 60;
        minutes = (int)elapsedTime / 60 % 60;
        hours = (int)elapsedTime / 3600 % 24;
        days = (int)elapsedTime / 86400 % 30; // Approx 30 days in a month
        months = (int)elapsedTime / 2592000; // Approx 30 days in a month
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
        OnTimeOfDayChange?.Invoke(timeOfDay.ToString());
    }

    // Update formatted date (MM-DD) and time (hh:mm:ss)
    void UpdateFormattedDateTime()
    {
        formattedDate = $"{months + 1:00}-{days + 1:00}"; // Months and days are 1-based
        formattedTime = $"{hours:00}:{minutes:00}:{seconds:00}";
        
       
        OnDateChange?.Invoke(formattedDate);
        OnTimeChange?.Invoke(formattedTime);
    }

    // Add seconds to the current time
    public void AddSeconds(int secondsToAdd)
    {
        startTime -= secondsToAdd;
    }

    // Add minutes to the current time
    public void AddMinutes(int minutesToAdd)
    {
        startTime -= minutesToAdd * 60;
    }

    // Add hours to the current time
    public void AddHours(int hoursToAdd)
    {
        startTime -= hoursToAdd * 3600;
    }

    // Add days to the current time
    public void AddDays(int daysToAdd)
    {
        startTime -= daysToAdd * 86400;
    }

    // Add months to the current time
    public void AddMonths(int monthsToAdd)
    {
        startTime -= monthsToAdd * 2592000; // Approx 30 days in a month
    }
}