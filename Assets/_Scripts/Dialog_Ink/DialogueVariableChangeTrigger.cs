using _Scripts.Managers;
using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{/// <summary>
 /// Can be used to change dialogue variables.
 /// </summary>
    public class DialogueVariableChangeTrigger : MonoBehaviour
    {

        public string storyName;
        public string variableName;
        public string variableValue;


        public void UpdateStoryVariable()
        {
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable(storyName, variableName, new StringValue(variableValue));
            Debug.Log("story var updated");
        }
    }
}