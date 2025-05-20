using _Scripts.Managers;

namespace _Scripts.NarrativeAndCutscenes.Checkpoints
{
    /// <summary>
    /// Start story mode. Set start time
    /// </summary>
    public class Checkpoint1: Checkpoint
    {
        public override void Activate()
        {
            
            TimeManager.Instance.SetInitialTime(1,1,12,15,00);
            IsCheckpointActivated = true;
            //CutsceneController.Instance.LaunchCutscene("Chapter1_Cutscene1"); 
            EventManager.Instance.CutsceneEvents.LaunchCutscene("Chapter1_Cutscene1");
        }
    }
}