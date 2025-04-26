using _Scripts.Managers;

namespace _Scripts.NarrativeAndCutscenes.Checkpoints
{
    /// <summary>
    /// Start story mode. Set start time
    /// disable dev npc
    /// disable displaying not met quests (buttons in ui)
    /// </summary>
    public class Checkpoint1: Checkpoint
    {
        public override void Activate()
        {
            
            
            TimeManager.Instance.SetInitialTime(1,1,12,15,00);
            
            StoryProgressionManager.Instance.LaunchCutscene("Chapter1_Cutscene1"); //todo add cutscene class
        }
    }
}