//EXTERNAL FUNCTIONS
//mandatory for completing knots
EXTERNAL CompleteDialogueKnot(characterName, dialogueKnotName)
//for quests (use only start and finish (with optional choice appear depending on quest status?))
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)


//global variables
VAR CharacterName = "TestNpcCharacter1"
//quest 1 (number in this story)
VAR SecondDevQuestId = "SecondDevQuest" //current quest used in this story + ID (for external quest events)
VAR SecondDevQuestState = "RequirementsNotMet" //current quest used in this story + State (for tracking quest state and open choices)


INCLUDE TestNpcCharacter1_Dialogue1.ink
INCLUDE TestNpcCharacter1_Dialogue2.ink
INCLUDE TestNpcCharacter1_Dialogue3.ink
INCLUDE TestNpcCharacter1_Dialogue4.ink

