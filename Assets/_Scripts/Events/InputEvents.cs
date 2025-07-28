using System;
using _Scripts.Enums;
using UnityEngine;

namespace _Scripts.Events
{
    public class InputEvents
    {
        private InputEventContext InputEventContext { get; set; }


        public void SetInputEventContext(InputEventContext newContext)
        {
            InputEventContext = newContext;
        }

        public InputEventContext GetInputEventContext() //must be used with input event which send context param
        {
            return InputEventContext;
        }



        
        //Block inputs
        public event Action<bool> OnSetHotkeysActive;
        public void SetHotkeysActive(bool toggle) 
        {
            //Debug.Log($"Hotkeys isActive = {toggle}");
            OnSetHotkeysActive?.Invoke(toggle);
        }
        
        public event Action<bool> OnSetSubmitActive;
        public void SetSubmitActive(bool toggle) 
        {
            OnSetSubmitActive?.Invoke(toggle);
        }
        public event Action<bool> OnSetSpaceActive;
        public void SetSpaceActive(bool toggle) 
        {
            OnSetSpaceActive?.Invoke(toggle);
        }
        
        public event Action<bool> OnSetInputActive;
        public void SetInputActive(bool toggle) 
        {
            OnSetInputActive?.Invoke(toggle);
        }
        
        
        
        //Keys pressed
        public event Action<InputEventContext> OnSubmitPressed;
        public void SubmitPressed() 
        {
            OnSubmitPressed?.Invoke(InputEventContext);
        }
        
        public event Action OnInteractPressed;
        public void InteractPressed() 
        {
            OnInteractPressed?.Invoke();
        }
        
        public event Action OnSpacePressed;
        public void SpacePressed() 
        {
            OnSpacePressed?.Invoke();
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
        
        public event Action OnStorageUnderStairsInventoryPressed;
        public void StorageUnderStairsInventoryPressed() 
        {
            OnStorageUnderStairsInventoryPressed?.Invoke();
        }
        
        
        

    }
}