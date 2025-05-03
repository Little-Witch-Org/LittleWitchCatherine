using _Scripts.Enums;
using _Scripts.Managers;

namespace _Scripts.NarrativeAndCutscenes
{/// <summary>
 /// Class has methods to control and manipulate an objects, game state, characters etc. (almost for cutscenes and narrative events)
 /// </summary>
    public class GameUtils
    {

        public void TeleportPlayerToNovelPlace(string location, string place)
        {
            //PlayerCharacterManager.Instance.SetPreviousSpawnPositionPoint(pointNamesName); //
            EventManager.Instance.TransitionEvents.PlaceTransitionTrigger(location, place);
            EventManager.Instance.TransitionEvents.ChangeScene(SceneNamesEnum.NovelView);
        }
        
    }
}