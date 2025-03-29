using System;
using _Scripts.Components.TimeManagement.Enums;

namespace _Scripts.Events
{
    public class TimeEvents
    {
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