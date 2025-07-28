===DoorToLivingRoom_Dialogue1===

\*Дверь закрыта

{ tried_to_open == false:
*[Попытаться открыть]
~ tried_to_open = true
->TryToOpen

*[Отойти от двери]
->DONE

- else:
->Continue
}


=TryToOpen
\*Дверь не открывается
-Не помню что бы   к нам должны   пожаловать гости... а у этой двери был замок... Не вырос же он за ночь.
->Continue



=Continue
*[Попробовать открыть дверь заклинанием]
->DialogueBattle
*[Отойти от вдери]
->DONE

=DialogueBattle
Какое заклинание использовать ?
*[Манодный толчок (низкий риск)]
(эффект толчка)
->DoorOpens1
*[Файрбол (высокий риск)]
(эффект удара файрболом)

->DoorOpens2

=DoorOpens1
~SetPlaceStateAndApplyWithFade("FFCorridor","FFCorridor.PlaceStateEnum.BrokenDoor")
(Дверь ломается)
~AddMinutes(15)
(+15 минут)
->FinishDialogue

=DoorOpens2
~SetPlaceStateAndApplyWithFade("FFCorridor","FFCorridor.PlaceStateEnum.BrokenDoor")
(Дверь ломается) (мама прикрывает щитом)
Анна: Темные боги, Рин... о том, что  размахивать огнеными шарами в помещении  – не лучшая идея знает каждый мальчишка. Ты ведь кажется читала сборник о похождения Лианы Инвор. Не думала что у моей Рин – медузьи мозги.
Кетрин: Прости... я не подумала. (грустная и мокрая)
Анна: Право слово, ты бы и драгу слейв против несчастной двери использовала б, коль знала б.
~UpdateReputation("Mother", -15)
~UpdateHealth(-10)
~UpdateMood(-35)
(Репутация Анны -15 / здоровье-17 / настроение -35) 
~AddMinutes(15)
(+15 минут)
->FinishDialogue

=FinishDialogue
~FinishCurrentQuestStep("Quest2CleanLivingroomFixClavecin",false)
~CompleteDialogueKnot("DoorToLivingRoom", "DoorToLivingRoom_Dialogue1")


-->END
