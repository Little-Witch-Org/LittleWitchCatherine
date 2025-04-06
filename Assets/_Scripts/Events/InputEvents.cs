using System;
using _Scripts.Enums;

namespace _Scripts.Events
{
    public class InputEvents
    {
        public InputEventContext InputEventContext { get; private set; }

        public void ChangeInputEventContext(InputEventContext newContext)
        {
            InputEventContext = newContext;
        }
        public event Action<InputEventContext> OnSubmitPressed;
        public void SubmitPressed() 
        {
            OnSubmitPressed?.Invoke(InputEventContext);
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
        
        public event Action OnInteractPressed;
        public void InteractPressed() 
        {
            OnInteractPressed?.Invoke();
        }
    }
}