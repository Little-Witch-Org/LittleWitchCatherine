//EXTERNAL FUNCTIONS
//mandatory for completing knots
EXTERNAL CompleteDialogueKnot(characterName, dialogueKnotName)
//for quests (use only start and finish (with optional choice appear depending on quest status?))
EXTERNAL StartQuest(questId)
EXTERNAL FinishQuest(questId)

EXTERNAL ResumeCutscene()

EXTERNAL AddMinutes(int)

EXTERNAL UpdateHealth(int)
EXTERNAL UpdateSatiety(int)
EXTERNAL UpdateMood(int)
EXTERNAL UpdateEnergy(int)

EXTERNAL UpdateReputation(string, int)

EXTERNAL ActivateCheckpoint(int)

EXTERNAL AddItem(string,string)

EXTERNAL FinishCurrentQuestStep(string,bool)

EXTERNAL TeleportPlayerBetweenPlaces(string, string)

//global variables 
VAR CharacterName = "Mother"
//quest 1 (number in this story)
VAR Quest1GoDownToKitchenId = "Quest1GoDownToKitchen" //current quest used in this story + ID (for external quest events)
VAR Quest1GoDownToKitchenState = "CanStart" //current quest used in this story + State (for tracking quest state and open choices)

VAR SleepCount =0 //for dialogue 2

VAR randomRoll = 0 //using roll in dialogue

VAR playerHealthOnStartDialogue = -1 //using in 4th dialogue after clavi battle ends

VAR claviBroken = false

INCLUDE Mother_Dialogue1.ink
INCLUDE Mother_Dialogue2.ink
INCLUDE Mother_Dialogue3.ink
INCLUDE Mother_Dialogue4.ink
INCLUDE Mother_Dialogue5.ink
