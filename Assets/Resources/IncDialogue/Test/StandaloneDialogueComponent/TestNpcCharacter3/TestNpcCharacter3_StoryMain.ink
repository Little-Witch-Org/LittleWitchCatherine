EXTERNAL CompleteDialogueKnot(characterName, dialogueKnotName)
//(events) using in quest point by default (accept AdvanceQuest)
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)

//global variables
VAR CharacterName = "TestNpcCharacter3"

//quest name variable (questId + "Id" for var name)
VAR FirstDevQuestId = "FirstDevQuest"

//quest states (quest id + "state") variable
VAR FirstDevQuestState = "RequirementsNotMet"

//ink files
INCLUDE TestNpcCharacter3_Dialogue1.ink