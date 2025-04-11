//(events) using in quest point by default (accept AdvanceQuest)
EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)

//quest name variable (questId + "Id" for var name)
VAR FirstDevQuestId = "FirstDevQuest"

//quest states (quest id + "state") variable
VAR FirstDevQuestState = "RequirementsNotMet"

//ink files
INCLUDE FirstDevQuest_start_npc.ink
INCLUDE FirstDevQuest_finish_npc.ink