using _Scripts.test.QuestSystem;
using _Scripts.test.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestLogUI_Test : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] public GameObject contentParent;
    [FormerlySerializedAs("scrollingList")] [SerializeField] private QuestLogScrollingList_Test scrollingListTest;
    [SerializeField] private TMP_Text questDisplayNameText;
    [SerializeField] private TMP_Text questStatusText;
    [SerializeField] private TMP_Text goldRewardsText;
    [SerializeField] private TMP_Text experienceRewardsText;
    [SerializeField] private TMP_Text levelRequirementsText;
    [SerializeField] private TMP_Text questRequirementsText;
    
        private Button firstSelectedButton;

    private void OnEnable()
    {
        GameEventsManager_Test.Instance.InputEventsTest.OnMenuPressed += QuestLogTogglePressed;
        GameEventsManager_Test.Instance.QuestEventsTest.OnQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager_Test.Instance.InputEventsTest.OnMenuPressed -= QuestLogTogglePressed;
        GameEventsManager_Test.Instance.QuestEventsTest.OnQuestStateChange -= QuestStateChange;
    }

    private void QuestLogTogglePressed()
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
        if (firstSelectedButton != null)
        {
            firstSelectedButton.Select();
        }
    }

    public void HideUI()
    {
        contentParent.SetActive(false);
        //GameEventsManager_Test.Instance.playerEvents.EnablePlayerMovement();
        EventSystem.current.SetSelectedGameObject(null); //selected button ?
    }

    private void QuestStateChange(Quest_Test quest)
    {
        // add the button to the scrolling list if not already added
        QuestLogButton_Test questLogButtonTest = scrollingListTest.CreateButtonIfNotExist(quest, () => {
            SetQuestLogInfo(quest);
        });

        // initialize the first selected button if not already so that it's
        // always the top button
        if (firstSelectedButton == null)
        {
            firstSelectedButton = questLogButtonTest.button;
        }

        // set the button color based on quest stateEnum
        questLogButtonTest.SetState(quest.StateEnumTest);
    }

    private void SetQuestLogInfo(Quest_Test quest)
    {
        // quest name
        questDisplayNameText.text = quest.info.displayName;

        // status
        questStatusText.text = quest.GetFullStatusText();

        // requirements
        levelRequirementsText.text = "Level " + quest.info.levelRequirement;
        questRequirementsText.text = "";
        foreach (QuestInfoSo_Test prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            questRequirementsText.text += prerequisiteQuestInfo.displayName + "\n";
        }

        // rewards
        goldRewardsText.text = quest.info.goldReward + " Gold";
        experienceRewardsText.text = quest.info.expReward + " XP";
    }
}
