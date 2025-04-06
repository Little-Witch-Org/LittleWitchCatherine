===FirstDevQuestFinish===
{ FirstDevQuestState:
-"Finished": -> Finished
-else : -> default
}

=Finished
Ага, спасибо за работу.
-> END

=default
Что надо ?
*[Ничего]
->END
*{ FirstDevQuestState == "CanFinish" }[Квест выполнен!.] // appears only if we have canfinish state
~ FinishQuest(FirstDevQuestId)
Кнопка нажата ? Отлично.
-> END

