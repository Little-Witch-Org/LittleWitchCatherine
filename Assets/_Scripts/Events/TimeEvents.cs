using System;
using _Scripts.Enums;

namespace _Scripts.Events
{
    public class TimeEvents
    {
        //update
        public event Action <int> OnAddSeconds;
        public void AddSeconds(int seconds) 
        {
            OnAddSeconds?.Invoke(seconds);
        }
        
        public event Action <int> OnAddMinutes;
        public void AddMinutes(int minutes) 
        {
            OnAddMinutes?.Invoke(minutes);
        }
        
        public event Action <int> OnAddHours;
        public void AddHours(int hours) 
        {
            OnAddHours?.Invoke(hours);
        }
        public event Action <int> OnAddDays;
        public void AddDays(int days) 
        {
            OnAddDays?.Invoke(days);
        }
        
        
        
        
        //changed (on what amount of units time was changed)
        public event Action <int> OnSecondsAmountChanged;
        public void SecondsAmountChange(int seconds) 
        {
            OnSecondsAmountChanged?.Invoke(seconds);
        }
        
        public event Action <int> OnMinutesAmountChanged;
        public void MinutesAmountChange(int minutes) 
        {
            OnMinutesAmountChanged?.Invoke(minutes);
        }
        
        public event Action <int> OnHoursAmountChanged;
        public void HoursAmountChange(int hours) 
        {
            OnHoursAmountChanged?.Invoke(hours);
        }
        public event Action <int> OnDaysAmountChanged;
        public void DaysAmountChange(int days) 
        {
            OnDaysAmountChanged?.Invoke(days);
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
        
        public event Action <TimeOfDayEnum> OnUpdateTimeOfDay;
        public void UpdateTimeOfDay(TimeOfDayEnum timeOfDayEnum) 
        {
            OnUpdateTimeOfDay?.Invoke(timeOfDayEnum);
        }
    }
    
}