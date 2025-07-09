using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Dialog_Ink;
using _Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.UI.CheatMenu
{
    public class DialogueScrollComponent:MonoBehaviour
    {
        
		[Header("Configuration")]
        [SerializeField] private GameObject viewportContent;
        [SerializeField] private GameObject dialogItemUIPrefab;
        [SerializeField] private string npcName;
        
        
        private StandaloneDialogueComponent _standaloneDialogueComponent;
        private List<GameObject> _dialogueItems = new List<GameObject>();



        private void OnEnable()
        {
            StartCoroutine(InitializeDelayed());
        }
        
        
        //unsubscribe
        private void OnDestroy()
        {
            // Отписываемся от всех событий при уничтожении
            foreach (var item in _dialogueItems)
            {
                if (item != null)
                {
                    item.GetComponent<DialogItemUI>().OnCheckboxValueChanged = null;
                }
            }
        }

        private IEnumerator InitializeDelayed()
        {
            yield return null; //need to skip frame to standalone component initialized in his start
            
            var npcGo = NpcCharactersManager.Instance.GetNpcCharacters().FirstOrDefault(go => 
                go.name.Contains(npcName) && go.GetComponent<StandaloneDialogueComponent>() != null);

            if (npcGo != null) _standaloneDialogueComponent = npcGo.GetComponent<StandaloneDialogueComponent>();
            
            
            UpdateViewportContentElements(_standaloneDialogueComponent.GetDialogueKnotStates());
            
            
        }

        //gets dictionary from standalone component and creates dialogItemUI GO's depending on count and states in dictionary
        private void UpdateViewportContentElements(Dictionary<string, bool> dialogueStates)
        {

            //create buttons if non exist
            if (_dialogueItems.Count == 0)
            {
                var sortedKeys = dialogueStates.Keys.OrderBy(key => key).ToList();

                foreach (var key in sortedKeys)
                {

                    GameObject dialogueItemUIGo = Instantiate(dialogItemUIPrefab, viewportContent.transform);
                    var dialogItemUIScript = dialogueItemUIGo.GetComponent<DialogItemUI>();
                    dialogItemUIScript.DialogueName = key;
                    dialogItemUIScript.IsCompleted = dialogueStates[key];
                    

                    dialogItemUIScript.OnCheckboxValueChanged += UpdateDialogueStatesInStandalone;
                    
                    _dialogueItems.Add(dialogueItemUIGo);
                }
            }
            //update items state if initialized already
            else
            {
                
                Dictionary<GameObject, bool> dialogueStatesTemp = new();
                foreach (var dialogPair in dialogueStates)
                {
                    var item = _dialogueItems.Find(x => x.GetComponent<DialogItemUI>().DialogueName == dialogPair.Key);
                    dialogueStatesTemp.Add(item, dialogPair.Value);
                }

                foreach (var pair in dialogueStatesTemp)
                {
                    //pair.Key.GetComponent<DialogItemUI>().IsCompleted = pair.Value;
                    pair.Key.GetComponent<DialogItemUI>().UpdateCompleteStateExternal(pair.Value);
                    
                }
            }
        }

        private void UpdateDialogueStatesInStandalone(string dialogueKnotName,bool isChecked)
        {
            //Debug.Log("update from checkbox "+ dialogueKnotName);
            
            EventManager.Instance.DialogueEvents.UpdateDialogueStatesFromUI(npcName, dialogueKnotName, isChecked);
            
        }
        
        //+todo we need to update items states when window opened or else
        //+todo add ability to change state in component when checkbox changed
        //todo add check box - check all
    }
}