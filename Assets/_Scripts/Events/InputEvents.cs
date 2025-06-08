using System;
using _Scripts.Enums;

namespace _Scripts.Events
{
    public class InputEvents
    {
        private InputEventContext InputEventContext { get; set; }
        
        private bool _submitLocked; //for submit in dialogue

        public void SetInputEventContext(InputEventContext newContext)
        {
            InputEventContext = newContext;
        }

        public InputEventContext GetInputEventContext() //must be used with input event which send context param
        {
            return InputEventContext;
        }



        public void SetSubmitLock(bool locked)
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
        
        
        
        //UI menus
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
        
        public event Action OnPlayerInventoryPressed;
        public void PlayerInventoryPressed() 
        {
            OnPlayerInventoryPressed?.Invoke();
        }
        
        public event Action OnStorageInventoryPressed;
        public void StorageInventoryPressed() 
        {
            OnStorageInventoryPressed?.Invoke();
        }
        
        
        

    }
}