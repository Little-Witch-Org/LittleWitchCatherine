===TestNpcCharacter1_Dialogue2===
Это второй диалог данного нпс. #currentSpeaker:speaker2 #speaker1name:player #portrait1:player_neutral #speaker2name:npc #portrait2:npc_neutral 
Показываю доступные варианты.
    +[Провекра возможности взять квест]
        //swich
        {SecondDevQuestState:
        - "RequirementsNotMet": -> RequirementsNotMet
        - "CanStart": -> CanStart
        }
    *[Завершить этот узел диалога и перейти к следующему ?]
        ~CompleteDialogueKnot(CharacterName, "TestNpcCharacter1_Dialogue2")//invoked with var(string) and string param
        ->TestNpcCharacter1_Dialogue3
    +[Вернуться к старту этого диалога]
        -> TestNpcCharacter1_Dialogue2
    *[Закрыть диалог]
        Выходим из диалога.
            
- -> END //end of global divert

//switch variants
=RequirementsNotMet
Квест пока недоступен.
Нажмите на квадрат!
->END

=CanStart
Квест доступен!
*[Взять квест]
Квест Взят! Описание в журнале. Удачи!
~StartQuest(SecondDevQuestId)
~CompleteDialogueKnot(CharacterName, "TestNpcCharacter1_Dialogue2")
+[Вернуться к началу диалога]
->TestNpcCharacter1_Dialogue2
*[Закрыть диалог]
Выходим из диалога.
-->END

