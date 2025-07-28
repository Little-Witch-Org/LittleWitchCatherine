using System;
using _Scripts.Enums;

namespace _Scripts.Events
{
    public class TimeEvents
    {
        //set
        public event Action <int> OnSecondsSet;
        public void SetSeconds(int seconds) 
        {
            OnSecondsSet?.Invoke(seconds);
        }
        
        public event Action <int> OnMinutesSet;
        public void SetMinutes(int minutes) 
        {
            OnMinutesSet?.Invoke(minutes);
        }
        
        public event Action <int> OnHoursSet;
        public void SetHours(int hours) 
        {
            OnHoursSet?.Invoke(hours);
        }
        public event Action <int> OnDaysSet;
        public void SetDays(int days) 
        {
            OnDaysSet?.Invoke(days);
        }
        
        //add
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
        
        
        //changed without time skip event
        public event Action <int> OnSecondsSetChanged;
        public void SecondsSetChanged(int seconds) 
        {
            OnSecondsSetChanged?.Invoke(seconds);
        }
        
        public event Action <int> OnMinutesSetChanged;
        public void MinutesSetChanged(int minutes) 
        {
            OnMinutesSetChanged?.Invoke(minutes);
        }
        
        public event Action <int> OnHoursSetChanged;
        public void HoursSetChanged(int hours) 
        {
            OnHoursSetChanged?.Invoke(hours);
        }
        public event Action <int> OnDaysSetChanged;
        public void DaysSetChanged(int days) 
        {
            OnDaysSetChanged?.Invoke(days);
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