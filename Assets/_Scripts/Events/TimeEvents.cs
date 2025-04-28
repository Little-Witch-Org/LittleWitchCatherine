using System;
using _Scripts.Components.TimeManagement.Enums;

namespace _Scripts.Events
{
    public class TimeEvents
    {
        //update
        public event Action <int> OnUpdateSeconds;
        public void UpdateSeconds(int seconds) 
        {
            OnUpdateSeconds?.Invoke(seconds);
        }
        
        public event Action <int> OnUpdateMinutes;
        public void UpdateMinutes(int minutes) 
        {
            OnUpdateMinutes?.Invoke(minutes);
        }
        
        public event Action <int> OnUpdateHours;
        public void UpdateHours(int hours) 
        {
            OnUpdateHours?.Invoke(hours);
        }
        public event Action <int> OnUpdateDays;
        public void UpdateDays(int days) 
        {
            OnUpdateDays?.Invoke(days);
        }
        
        
        
        
        //change
        
        public event Action <int> OnSecondsChanged;
        public void SecondsChange(int seconds) 
        {
            OnSecondsChanged?.Invoke(seconds);
        }
        
        public event Action <int> OnMinutesChanged;
        public void MinutesChange(int minutes) 
        {
            OnMinutesChanged?.Invoke(minutes);
        }
        
        public event Action <int> OnHoursChanged;
        public void HoursChange(int hours) 
        {
            OnHoursChanged?.Invoke(hours);
        }
        public event Action <int> OnDaysChanged;
        public void DaysChange(int days) 
        {
            OnDaysChanged?.Invoke(days);
        }
        
        
        
        public event Action <string> OnTimeChange;
        public void TimeChange(string time) 
        {
            OnTimeChange?.Invoke(time);
        }
        
        public event Action <string> OnDateChange;
        public void DateChange(string date) 
        {
            OnDateChange?.Invoke(date);
        }
        
        public event Action <TimeOfDay> OnTimeOfDayChange;
        public void TimeOfDayChange(TimeOfDay timeOfDay) 
        {
            OnTimeOfDayChange?.Invoke(timeOfDay);
        }
    }
    
}