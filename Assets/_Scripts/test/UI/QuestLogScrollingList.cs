using System;
using System.Collections.Generic;
using _Scripts.test.QuestSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.test.UI
{
    public class QuestLogScrollingList : MonoBehaviour
    {

        [Header("Components")] [SerializeField]
        private GameObject contentParent;
        
        [Header("Rect Transforms")]
        [SerializeField] private RectTransform scrollRectTransform;
        [SerializeField] private RectTransform contentRectTransform;
        
        
        
        [Header("Quest Log Button")]
        [SerializeField] private GameObject questLogButtonPrefab;
        
        private Dictionary<string, QuestLogButton> idToButtonMap = new Dictionary<string, QuestLogButton>();


        // Below is code to test that the scrolling list is working as expected.
        // For it to work, you'll need to change the QuestInfoSO id field to be publicly settable
        /*private void Start()
        {
            for (int i = 0; i < 20; i++)
            {
                QuestInfoSo_Test questInfoSoTest = ScriptableObject.CreateInstance<QuestInfoSo_Test>();
                questInfoSoTest.Id = "test" + i;
                questInfoSoTest.displayName = "Quest" + i;
                questInfoSoTest.questStepPrefabs = new GameObject[0];
                Quest_Test questTest = new Quest_Test(questInfoSoTest);

                QuestLogButton questLogButton = CreateButtonIfNotExist(questTest,
                    () =>
                    {
                        Debug.Log("Selected: " + questInfoSoTest.displayName);
                    });

                if (i == 0)
                {
                    questLogButton.button.Select();
                }

            }
        }*/


        public QuestLogButton CreateButtonIfNotExist(Quest_Test questTest, UnityAction selectAction)
        {
            QuestLogButton questLogButton = null;
            //only create button if we haven't seen this quest id before
            if (!idToButtonMap.ContainsKey(questTest.info.Id))
            {
                questLogButton = InstantiateQuestLogButton(questTest, selectAction);
            }
            else
            {
                questLogButton = idToButtonMap[questTest.info.Id];
            }
            return questLogButton;
        }
        
        
        private QuestLogButton InstantiateQuestLogButton(Quest_Test questTest, UnityAction selectAction)
        {
            //create the button
            QuestLogButton questLogButton = Instantiate(questLogButtonPrefab, contentParent.transform).GetComponent<QuestLogButton>();
            
            //game object name in the scene
            questLogButton.gameObject.name = questTest.info.Id + "_button";
            //initialize and set up function for when the button is selected
            RectTransform buttonRectTransform = questLogButton.GetComponent<RectTransform>();
            questLogButton.Initialize(questTest.info.displayName, ()=>
            {
                selectAction();
                UpdateScrolling(buttonRectTransform);
            });
            //add to map to keep track of the new button
            idToButtonMap[questTest.info.Id] = questLogButton;
            
            return questLogButton;
        }

        private void UpdateScrolling(RectTransform buttonRectTransform)
        {
            // calculate the min and max for the selected button
            float buttonYMin = Mathf.Abs(buttonRectTransform.anchoredPosition.y);
            float buttonYMax = buttonYMin + buttonRectTransform.rect.height;

            // calculate the min and max for the content area
            float contentYMin = contentRectTransform.anchoredPosition.y;
            float contentYMax = contentYMin + scrollRectTransform.rect.height;

            // handle scrolling down
            if (buttonYMax > contentYMax)
            {
                contentRectTransform.anchoredPosition = new Vector2(
                    contentRectTransform.anchoredPosition.x,
                    buttonYMax - scrollRectTransform.rect.height
                );
            }
            // handle scrolling up
            else if (buttonYMin < contentYMin) 
            {
                contentRectTransform.anchoredPosition = new Vector2(
                    contentRectTransform.anchoredPosition.x,
                    buttonYMin
                );
            }
        }
    }
}
