using System;
using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Service.Log;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.QuestSystem.UI
{
    public class QuestLogScrollingList : MonoBehaviour
    {

        [Header("Components")] 
        [SerializeField] private GameObject content;

        [Header("Rect Transforms")]
        [SerializeField] private RectTransform scrollRectTransform;

        [SerializeField] private RectTransform contentRectTransform;



        [Header("Quest Log Button")]
        [SerializeField] private GameObject questLogButtonPrefab;

        private Dictionary<string, QuestLogButton> _idToButtonMap = new();


        // Below is code to test that the scrolling list is working as expected.
        // For it to work, you'll need to change the QuestInfoSO id field to be publicly settable
        /*private void Start()
        {
            for (int i = 0; i < 20; i++)
            {
                QuestInfoSo questInfoSoTest = ScriptableObject.CreateInstance<QuestInfoSo>();
                questInfoSoTest.Id = "test" + i;
                questInfoSoTest.displayName = "Quest" + i;
                questInfoSoTest.questStepPrefabs = new GameObject[0];
                Quest questTest = new Quest(questInfoSoTest);

                QuestLogButton_Test questLogButton = CreateButtonIfNotExist(questTest,
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

        //private void Start()
        //{
        //    UpdateQuestsVisibilityInUI();
        //}

        private void OnEnable()
        {
            EventManager.Instance.QuestEvents.OnUpdateQuestVisibilityInUI += UpdateQuestsVisibilityInUI;
        }

        private void OnDisable()
        {
            EventManager.Instance.QuestEvents.OnUpdateQuestVisibilityInUI -= UpdateQuestsVisibilityInUI;
        }

        public QuestLogButton
            CreateButtonIfNotExist(Quest quest,
                UnityAction selectAction) //todo disable button if quest is not visible. Add event to set visibility for quest
        {
            QuestLogButton questLogButton = null;
            //only create button if we haven't seen this quest id before
            if (!_idToButtonMap.ContainsKey(quest.InfoSo.Id))
            {
                questLogButton = InstantiateQuestLogButton(quest, selectAction);
            }
            else
            {
                questLogButton = _idToButtonMap[quest.InfoSo.Id];
            }

            return questLogButton;
        }


        private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
        {
            //create the button
            QuestLogButton questLogButton =
                Instantiate(questLogButtonPrefab, content.transform).GetComponent<QuestLogButton>();

            //game object name in the scene
            questLogButton.gameObject.name = quest.InfoSo.Id + "_button";
            //initialize and set up function for when the button is selected
            RectTransform buttonRectTransform = questLogButton.GetComponent<RectTransform>();
            questLogButton.Initialize(quest.InfoSo.displayName, () =>
            {
                selectAction();
                UpdateScrolling(buttonRectTransform);
            });
            //add to map to keep track of the new button
            _idToButtonMap[quest.InfoSo.Id] = questLogButton;

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

        public void UpdateQuestsVisibilityInUI()
        {
            foreach (var questButton in contentRectTransform.gameObject.GetComponentsInChildren<QuestLogButton>(
                         includeInactive: true))
            {
                //get questId from button name
                string[] buttonNameSplited = questButton.gameObject.name.Split("_button");
                if (buttonNameSplited.Length != 2)
                {
                    QuestDebug.Instance.LogError($"Cant parse this button name: {buttonNameSplited}");
                }

                string questId = buttonNameSplited[0].Trim();

                //get previous button active status
                bool isPreviouslyActive = questButton.gameObject.activeInHierarchy;

                //set status
                var isVisible = EventManager.Instance.QuestEvents.RequestQuestByQuestId(questId).IsQuestVisible;
                questButton.gameObject.SetActive(isVisible);

                //Debug.Log(questId);
                //Debug.Log(isVisible);
                //Debug.Log(questButton.gameObject.name);

                //put button on bottom in the list, if button status changed to active
                if (!isPreviouslyActive && isVisible)
                {
                    questButton.gameObject.transform.SetAsLastSibling();
                }
            }
        }

        //used to get first quest button 
        private Button GetFirstActiveButton()
        {
            foreach (var questLogButton in content.gameObject.GetComponentsInChildren<QuestLogButton>())
            {
                if (questLogButton.gameObject.activeSelf)
                {
                    return questLogButton.Button;
                }
            }

            return null;
        }

        //used to select first quest button 
        public void SelectFirstActiveButton()
        {
            var button = GetFirstActiveButton();
            if (button != null)
            {
                button.Select();
            }
            else
            {
                QuestDebug.Instance.LogError($"Can't select first active button");
            }
        }
    }
}
