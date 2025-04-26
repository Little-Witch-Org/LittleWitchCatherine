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
        
        private bool _submitLocked; //for submit in dialogue

        public void LockSubmit(bool locked)
        {
            _submitLocked = locked;
        }
        
        public event Action<bool> OnHotkeysAreActive;
        public void HotkeysAreActive(bool toggle) 
        {
            OnHotkeysAreActive?.Invoke(toggle);
        }
        
        
        
        public event Action<InputEventContext> OnSubmitPressed;
        public void SubmitPressed() 
        {
            if (_submitLocked) return;
            OnSubmitPressed?.Invoke(InputEventContext);
        }
        
        public event Action OnInteractPressed;
        public void InteractPressed() 
        {
            OnInteractPressed?.Invoke();
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
        
        public event Action OnStatsPressed;
        public void StatsPressed() 
        {
            OnStatsPressed?.Invoke();
        }
        
        
        

    }
}