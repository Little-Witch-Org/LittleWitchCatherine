using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.NarrativeAndCutscenes.Checkpoints
{
    /// <summary>
    /// "Open guest door" is current quest.
    /// Open world in house
    /// </summary>
    public class Checkpoint2:Checkpoint
    {
        public override void Activate()
        {
            Debug.Log("Checkpoint2 activated");
        }
    }
}