using _Scripts.Managers;
using _Scripts.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.QuestSystem.UI
{
    public class QuestLogMenuUI : MonoBehaviour, IMenu
    {

        [Header("Components")] [SerializeField]
        private GameObject contentParent;

        public GameObject ContentParent => contentParent;
        [SerializeField] private QuestLogScrollingList scrollingList;
        [SerializeField] private TMP_Text questDisplayNameText;

        [SerializeField]
        private TMP_Text questStatusText; // print previous and current quest step statuses + quest status

        [SerializeField] private TMP_Text rewardText;

        //[SerializeField] private TMP_Text levelRequirementsText;
        [SerializeField] private TMP_Text questRequirementsText;
        [SerializeField] private TMP_Text questStateText;

        [SerializeField] private Button questLogButton;

        private Button _firstSelectedButton;

        private void OnEnable()
        {
            EventManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;

            EventManager.Instance.QuestEvents.OnQuestStateChange += HighlightQuestButton;
            
            EventManager.Instance.GameEvents.OnCheckpointActivated += DisableDevQuests;
        }

        private void OnDisable()
        {
            EventManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;

            EventManager.Instance.QuestEvents.OnQuestStateChange -= HighlightQuestButton;
            
            EventManager.Instance.GameEvents.OnCheckpointActivated -= DisableDevQuests;
        }
        


        public void ShowMenu()
        {
            contentParent.SetActive(true);
            //GameEventsManager_Test.Instance.playerEvents.DisablePlayerMovement();
            // note - this needs to happen after the content parent is set active,
            // or else the onSelectAction won't work as expected
            if (_firstSelectedButton != null)
            {
                //Debug.Log(_firstSelectedButton.gameObject.GetComponent<QuestLogButton>().GetButtonText());
                _firstSelectedButton.Select(); //todo to disable?
            }

            //ColorUtility.TryParseHtmlString("#6F2502", out var customColor); //reset highlighted color to default
            questLogButton.image.color = Color.white;
        }

        public void HideMenu()
        {
            contentParent.SetActive(false);
            //GameEventsManager_Test.Instance.playerEvents.EnablePlayerMovement();
            EventSystem.current.SetSelectedGameObject(null);
        }

        //change button color according to quest state.
        private void QuestStateChange(Quest quest)
        {
            // add the button to the scrolling list if not already added
            QuestLogButton questLogButton =
                scrollingList.CreateButtonIfNotExist(quest, () => { SetQuestLogInfo(quest); });

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
            questStatusText.text = quest.GetFullStatusText();

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

        private void HighlightQuestButton(Quest quest)
        {
            questLogButton.image.color = Color.yellow;
            //questLogButton.image.DOColor(Color.yellow, 0.5f).Play();
        }

        //disable all dev quests if we launch story mode
        private void DisableDevQuests(int checkpointId)
        {
            //Debug.Log("Disabling Dev Quests");
            if (checkpointId == 1)
            {
                scrollingList.DisableDevQuestsButtons();
                _firstSelectedButton = scrollingList.GetFirstActiveButton();
                _firstSelectedButton.Select();
            }
        }
    }


}
