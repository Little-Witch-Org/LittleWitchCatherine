===FirstDevQuestStart=== //this is knot name. Used in quest point variable which starts dialogue.

//swich
{FirstDevQuestState:
    - "RequirementsNotMet": -> RequirementsNotMet
    - "CanStart": -> CanStart
    - "InProgress": -> InProgress
    - "CanFinish": -> CanFinish
    - "Finished": -> Finished
    - "Failed": -> Failed
    - else: -> END
}
= RequirementsNotMet
//Need to check IsQuestAvailable for this
Привет, это диалог, сообщающий, что этот квест пока нельзя начать.
-> END

= CanStart
Привет, это диалог начала квеста, в котором тебе предлагается сходить в оранжерею и нажать там на кружок. Нажми Space для продолжения.
Это выбор варианта. (Выбор мышкой)
*[Схожу] //if we use [], this will not appear in next line. Can be combined like. Yes,[I do] thanks for asking!
    Отлично, это старт нового квеста! Отчитаешься этому, слева от меня.
    ~StartQuest(FirstDevQuestId) //call external function with questInfoSo name. !!! Put "questName" as string or add string in var!!!
*[Не схожу]
    Понятно, это завершение диалога без старта нового квеста.    
- -> END //- - > is for all choises

= InProgress
Привет, ну как там с нажатием на кнопку в оранжерее ?
-> END

= CanFinish
Привет, я смотрю кнопка была нажата! Поговори с соседом слева.
-> END

= Finished
Привет, поздравляю с завершением квеста!
-> END

= Failed
Привет, к сожалению мой квест был провален =(
//not for first quest
-> END
