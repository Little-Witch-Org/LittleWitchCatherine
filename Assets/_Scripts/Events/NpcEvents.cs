using System;

namespace _Scripts.Events
{
    public class NpcEvents
    {
        public event Action<string, bool> OnSetNpcIgnoredStatus;
        public void SetNpcIgnoredStatus(string npcName, bool isIgnored) 
        {
            OnSetNpcIgnoredStatus?.Invoke(npcName, isIgnored);
        }
        
        public event Action<string, string,string> OnMoveNpc;
        public void MoveNpc(string npcName, string location, string place) 
        {
            OnMoveNpc?.Invoke(npcName, location, place);
        }
        
        public event Action<string> OnDeleteNpc;
        public void DeleteNpc(string npcName) 
        {
            OnDeleteNpc?.Invoke(npcName);
        }
    }
}