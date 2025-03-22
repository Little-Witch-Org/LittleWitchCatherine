using System;

namespace _Scripts.events_test
{
    public class InputEvents_Test
    {
        public Action OnSubmitPressed;
        public void SubmitPressed() 
        {
            OnSubmitPressed?.Invoke();
        }
    }
}