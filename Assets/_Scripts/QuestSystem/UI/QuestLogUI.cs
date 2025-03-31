using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.QuestSystem.UI
{
    public class QuestLogUI : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] private GameObject contentParent;
        [SerializeField] private QuestLogScrollingList scrollingList;
        [SerializeField] private TMP_Text questDisplayNameText;
        [SerializeField] private TMP_Text questStatusText; // print previous and current quest step statuses + quest status
        [SerializeField] private TMP_Text rewardText;
        //[SerializeField] private TMP_Text levelRequirementsText;
        [SerializeField] private TMP_Text questRequirementsText;
        [SerializeField] private TMP_Text questStateText;
    
        private Button _firstSelectedButton;

        private void OnEnable()
        {
            EventManager.Instance.InputEvents.OnJournalPressed += QuestLogTogglePressed;
            EventManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;
        }

        private void OnDisable()
        {
            EventManager.Instance.InputEvents.OnJournalPressed -= QuestLogTogglePressed;
            EventManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
        }

        public void QuestLogTogglePressed()
        {
            if (contentParent.activeInHierarchy)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }

        public void ShowUI()
        {
            contentParent.SetActive(true);
            //GameEventsManager_Test.Instance.playerEvents.DisablePlayerMovement();
            // note - this needs to happen after the content parent is set active,
            // or else the onSelectAction won't work as expected
            if (_firstSelectedButton != null)
            {
                _firstSelectedButton.Select();
            }
        }

        public void HideUI()
        {
            contentParent.SetActive(false);
            //GameEventsManager_Test.Instance.playerEvents.EnablePlayerMovement();
            EventSystem.current.SetSelectedGameObject(null);
        }

        //change button color according to quest state. //todo to remake ?
        private void QuestStateChange(Quest quest)
        {
            // add the button to the scrolling list if not already added
            QuestLogButton questLogButton = scrollingList.CreateButtonIfNotExist(quest, () => {
                SetQuestLogInfo(quest);
            });

            // initialize the first selected button if not already so that it's
            // always the top button
            if (_firstSelectedButton == null)
            {
                _firstSelectedButton = questLogButton.button;
            }

            // set the button color based on quest stateEnum
            questLogButton.SetState(quest.StateEnum);
            
        }

        private void SetQuestLogInfo(Quest quest)
        {
            // quest name
            questDisplayNameText.text = quest.InfoSo.displayDescription;

            // status
            questStatusText.text = quest.GetFullStatusText(); //todo to remake 

            // requirements
            //levelRequirementsText.text = "Is quest available = " + quest.InfoSo.isQuestAvailable;
            questRequirementsText.text = "";
            foreach (QuestInfoSo prerequisiteQuestInfo in quest.InfoSo.questPrerequisites)
            {
                questRequirementsText.text += prerequisiteQuestInfo.displayName + "\n";
            }

            // rewards
            rewardText.text = quest.InfoSo.reward;
            
            //set quest state text
            questStateText.text = quest.StateEnum.ToString();
        }
    }
}
