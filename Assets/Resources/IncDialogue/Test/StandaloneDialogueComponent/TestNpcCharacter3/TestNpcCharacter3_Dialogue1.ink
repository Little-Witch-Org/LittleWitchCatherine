===TestNpcCharacter3_Dialogue1===
{ FirstDevQuestState:
-"Finished": -> Finished
-else : -> default
}


=default
<color=\#ff0000> Что надо ? </color> A?#currentSpeaker:speaker2 #speaker1name:player #portrait1:player_neutral #speaker2name:npc #portrait2:npc_neutral 

*{FirstDevQuestState == "CanFinish"}[Квест выполнен!] // appears only if we have canfinish state
~ FinishQuest(FirstDevQuestId)
Ага, поздравляю.
-> END

*[Ничего]
->END

=Finished
Пока.
-> END




