using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.test
{
    /// <summary>
    /// Keys already been assigned  - J C F I SPACE
    /// </summary>
    public class GameTestInputs : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                EventManager.Instance.CutsceneEvents.LaunchCutscene("WakeUpAfterClaviLooseCutscene");
            }
            
            if (Input.GetKeyDown(KeyCode.V))
            {
                EventManager.Instance.QuestEvents.StartQuest("Quest3GetGreeneryFromGarden");
            }
            
            if (Input.GetKeyDown(KeyCode.N))
            {
                EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("Mother_StoryMain","playerHealthOnStartDialogue", 55.1f);
            }
            
            if (Input.GetKeyDown(KeyCode.M))
            {
                EventManager.Instance.InventoryEvents.AddItem("inventory_item_id_1_5","main");
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("X");
                var data = EventManager.Instance.QuestEvents.RequestQuestByQuestId("Quest2CleanLivingroomFixClavecin")
                    .GetQuestData();
                var index = data.questStepIndex;
                Debug.Log(data.questStepIndex);
                Debug.Log(data.stateEnum);
                Debug.Log(data.questStepValues[index].stepProgress);
                Debug.Log(data.questStepValues[index].stepObjective);
                Debug.Log(data.questStepValues[index].isFailed);
                
                Debug.Log("prev");
                data = EventManager.Instance.QuestEvents.RequestQuestByQuestId("Quest2CleanLivingroomFixClavecin")
                    .GetQuestData();
                index = data.questStepIndex;
                Debug.Log(data.questStepIndex);
                Debug.Log(data.stateEnum);
                Debug.Log(data.questStepValues[index-1].stepProgress);
                Debug.Log(data.questStepValues[index-1].stepObjective);
                Debug.Log(data.questStepValues[index-1].isFailed);
            }
        }
    }
}