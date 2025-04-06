using System.Collections.Generic;
using _Scripts.Service.Log;
using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    public class InkDialogueVariables
    {
        private Dictionary<string, Ink.Runtime.Object> _variables;

        public InkDialogueVariables(Story story) 
        {
            // initialize the dictionary using the global variables in the story
            _variables = new Dictionary<string, Ink.Runtime.Object>();
            foreach (string name in story.variablesState)
            {
                Ink.Runtime.Object value = story.variablesState.GetVariableWithName(name);
                _variables.Add(name, value);
                DialogDebug.Instance.Log("Initialized global dialogue variable: " + name + " = " + value);
            }
        }

        public void SyncVariablesAndStartListening(Story story) 
        {
            // it's important that SyncVariablesToStory is before assigning the listener!
            SyncVariablesToStory(story);
            story.variablesState.variableChangedEvent += UpdateVariableState;
        }

        public void StopListening(Story story)
        {
            story.variablesState.variableChangedEvent -= UpdateVariableState;
        }

        //manually update variable (can use it to change dialogue behaviour (switch or something in ink)
        public void UpdateVariableState(string name, Ink.Runtime.Object value)
        {
            // only maintain variables that were initialized from the globals ink file
            if (!_variables.ContainsKey(name)) 
            { 
                return; 
            }
            _variables[name] = value;
            DialogDebug.Instance.Log("Updated dialogue variable: " + name + " = " + value);
        }

        private void SyncVariablesToStory(Story story)
        {
            foreach (KeyValuePair<string, Ink.Runtime.Object> variable in _variables)
            {
                story.variablesState.SetGlobal(variable.Key, variable.Value);
            }
        }
    }
}