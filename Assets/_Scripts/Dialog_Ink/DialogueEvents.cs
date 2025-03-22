using System;

namespace _Scripts.Dialog_Ink
{
    public class DialogueEvents
    {
        public Action<string> OnEnterDialogue;


        //this method used to manually trigger event (as auto invoke in some places in start or update)
        public void EnterDialogue(string knotName)
        {
            //same as OnEnterDialogue?.Invoke(knotName)
            if (OnEnterDialogue != null)
            {
                OnEnterDialogue(knotName);
            }
        }
    }
    
}