using System;
using _Scripts.Components.TimeManagement.Enums;

namespace _Scripts.Events
{
    public class TimeEvents
    {
        public Action<string> OnTimeChange;
        public void TimeChange(string time) 
        {
            OnTimeChange?.Invoke(time);
        }
        
        public Action<string> OnDateChange;
        public void DateChange(string date) 
        {
            OnDateChange?.Invoke(date);
        }
        
        public Action<TimeOfDay> OnTimeOfDayChange;
        public void TimeOfDayChange(TimeOfDay timeOfDay) 
        {
            OnTimeOfDayChange?.Invoke(timeOfDay);
        }
    }
    
}