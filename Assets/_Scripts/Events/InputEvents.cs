using System;

namespace _Scripts.Events
{
    public class InputEvents
    {
        public event Action OnSubmitPressed;
        public void SubmitPressed() 
        {
            OnSubmitPressed?.Invoke();
        }
        
        public event Action OnMenuPressed;
        public void MenuPressed() 
        {
            OnMenuPressed?.Invoke();
        }
        
        public event Action OnJournalPressed;
        public void JournalPressed() 
        {
            OnJournalPressed?.Invoke();
        }
    }
}