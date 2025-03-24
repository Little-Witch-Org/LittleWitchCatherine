using System;
using UnityEngine;

namespace _Scripts.Events
{
    public class TransitionEvents
    {
        public Action<string,string> OnPlaceTransitionTriggered;
        public void TriggerTransition(string location, string place) 
        {
            OnPlaceTransitionTriggered?.Invoke(location, place);
        }
    }
}