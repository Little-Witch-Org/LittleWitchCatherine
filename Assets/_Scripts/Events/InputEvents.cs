using System;

namespace _Scripts.Events
{
    public class InputEvents
    {
        public Action OnSubmitPressed;
        public void SubmitPressed() 
        {
            OnSubmitPressed?.Invoke();
        }
    }
}