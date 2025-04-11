===TestNpcCharacter1_Dialogue3===
Это третий диалог данного нпс.
Показываю доступные варианты.
             +[Провекра возможности завершить квест]
                //swich
                {SecondDevQuestState:
                - "CanStart": -> CanStart
                - "InProgress": -> InProgress
                - "CanFinish": -> CanFinish
                }

            *[Завершить этот узел диалога и перейти к следующему ?]
                ~CompleteDialogueKnot(CharacterName, "TestNpcCharacter1_Dialogue3")//invoked with var(string) and string param
                ->TestNpcCharacter1_Dialogue4
            +[Вернуться к старту этого диалога]
                -> TestNpcCharacter1_Dialogue3
            *[Закрыть диалог]
                 Выходим из диалога.

                
- -> END //end of global divert

//switch variants
=CanStart
Возможность взять квест упущена в предыдущем диалоге.
->END

=InProgress
Квест в процессе выполнения.
->END

=CanFinish
Квест можно сдать.
    +[Сдать квест и завершить этот диалог.]
        ~FinishQuest(SecondDevQuestId)
        ~CompleteDialogueKnot(CharacterName, "TestNpcCharacter1_Dialogue3")
    +[Вернуться к началу диалога]
        -> TestNpcCharacter1_Dialogue3
- ->END