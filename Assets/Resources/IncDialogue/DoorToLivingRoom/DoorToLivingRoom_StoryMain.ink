EXTERNAL CompleteDialogueKnot(characterName, dialogueKnotName)

EXTERNAL AddMinutes(int)

EXTERNAL UpdateHealth(int)
EXTERNAL UpdateSatiety(int)
EXTERNAL UpdateMood(int)
EXTERNAL UpdateEnergy(int)

EXTERNAL SetHealth(int)
EXTERNAL SetSatiety(int)
EXTERNAL SetMood(int)
EXTERNAL SetEnergy(int)

EXTERNAL SetPlaceStateAndApplyWithFade(string,string)
EXTERNAL FinishCurrentQuestStep(string,bool)

EXTERNAL SetReputation(string, int)
EXTERNAL UpdateReputation(string, int)

EXTERNAL TeleportPlayerBetweenPlaces(string, string)

EXTERNAL StartBattle(string)

EXTERNAL AddItem(string,string)

VAR tried_to_open = false
VAR moss_count = 0
VAR tried_to_play = false
VAR tried_to_conversate = false

VAR battle_finish_variant = "0"

INCLUDE DoorToLivingRoom_Dialogue1.ink
INCLUDE DoorToLivingRoom_Dialogue2.ink
INCLUDE DoorToLivingRoom_Dialogue3.ink