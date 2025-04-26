using System;

namespace _Scripts.Events
{
    public class MiscEvents
    {
        public event Action OnCursorChangeToDefault;
        public void CursorChangeToDefault() 
        {
            OnCursorChangeToDefault?.Invoke();
        }
        
        public event Action OnCursorChangeToHoverOnTrigger;
        public void CursorChangeToHoverOnTrigger() 
        {
            OnCursorChangeToHoverOnTrigger?.Invoke();
        }
    }
}