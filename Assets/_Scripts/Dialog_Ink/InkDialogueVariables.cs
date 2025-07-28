using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Service.Log;
using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    //todo check what type of parameter can be passed throw this method (string/int/float/bool) add appropriate events
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

        //uses if variables changing inside ink story by story logic -> if so, we need to update it in _variables in script (map in manager)
        public void SyncLocalToInkVariablesAndStartListening(Story story, string storyName)
        {
            // it's important that SyncVariablesToStory is before assigning the listener!
            SyncVariablesToStory(story);
            story.variablesState.variableChangedEvent += UpdateVariableState; //accumulates current story variables to their _car list.
        }

        public void StopListening(Story story)
        {
            story.variablesState.variableChangedEvent -= UpdateVariableState;
        }

        //update variables in current variable dictionary
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

        //update variables in current story file
        public void SyncVariablesToStory(Story story)
        {
            foreach (KeyValuePair<string, Ink.Runtime.Object> variable in _variables)
            {
                story.variablesState.SetGlobal(variable.Key, variable.Value);
            }
        }

        public Dictionary<string, Ink.Runtime.Object> GetVariables()
        {
            return _variables;
        }
    }
}