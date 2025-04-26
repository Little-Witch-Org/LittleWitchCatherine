//EXTERNAL FUNCTIONS
//mandatory for completing knots
EXTERNAL CompleteDialogueKnot(characterName, dialogueKnotName)
//for quests (use only start and finish (with optional choice appear depending on quest status?))
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)
EXTERNAL ResumeCutscene()


//global variables
VAR CharacterName = "CutsceneEntity"

//quest vars if needed


INCLUDE CutsceneEntity_Dialogue1.ink


