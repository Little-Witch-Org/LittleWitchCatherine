===TestNpcCharacter1_Dialogue4===
Это четвёртый и последний диалог данного нпс.

{SecondDevQuestState == "CanStart":
    Квест не был взят.
    -> END
-else:
    В этом диалоге можно проверить результаты выполнения квеста.
    
    * [Проверить результаты]
        -> SecondDevQuestState_Check
    * [Закрыть диалог]
        -> END
}

=== SecondDevQuestState_Check 
{SecondDevQuestState:
    - "Finished": -> Finished
    - "Failed": -> Failed
    - "InProgress": -> InProgress
    - "CanFinish": -> CanFinish
    -else: -> InProgress
}

=InProgress
Возможность сдать квест упущена в предыдущем диалоге.
->END

=CanFinish
Возможность сдать квест упущена в предыдущем диалоге.
->END


=Finished
Квест выполнен! +Rep
->END

=Failed
Квест провален =( -Rep
->END
