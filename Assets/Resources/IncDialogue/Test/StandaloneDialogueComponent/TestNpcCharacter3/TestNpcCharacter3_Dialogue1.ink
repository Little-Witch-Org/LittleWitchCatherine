===TestNpcCharacter3_Dialogue1===
{ FirstDevQuestState:
-"Finished": -> Finished
-else : -> default
}


=default
Что надо ?

*{FirstDevQuestState == "CanFinish"}[Квест выполнен!] // appears only if we have canfinish state
~ FinishQuest(FirstDevQuestId)
Ага, поздравляю.
-> END

*[Ничего]
->END

=Finished
Пока.
-> END




