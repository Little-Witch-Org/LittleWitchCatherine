===TestNpcCharacter1_Dialogue1===
=Greetings 
NPC: Привет, это первый диалог данного нпс.#currentSpeaker:speaker2 #speaker1name:player #portrait1:player_neutral #speaker2name:npc #portrait2:npc_neutral 
NPC: В нём будет демонстрация эмуляция общения персонажей с изменениями визуальной части. #currentSpeaker:speaker2
Player: Да? #currentSpeaker:speaker1
Player: Круто. #currentSpeaker:speaker1 #portrait1:player_happy
Player: А в других что ? #currentSpeaker:speaker1 #portrait1:player_neutral
NPC: А в других просто будет демонстрация возможностей диалоговой системы (далеко не всех). #currentSpeaker:speaker2
Player: Понятно. #currentSpeaker:speaker1 #portrait1:player_neutral
NPC: Отлично. #currentSpeaker:speaker2 #portrait2:npc_happy
->FirstChoice

=FirstChoice
NPC: Можем делать выбор таким образом. #currentSpeaker:speaker2 #portrait2:npc_neutral 
    +[Я выбираю этот вариант и проговариваю свой выбор]
    Player: Я выбираю этот вариант и проговариваю свой выбор#currentSpeaker:speaker1 #portrait1:player_neutral
    NPC: Да, так можно, теперь возвращаемся к выбору вариантов. #currentSpeaker:speaker2 #portrait2:npc_neutral 
    ->FirstChoice
    +[Я выбираю этот вариант (просто кнопка выбора)]
    NPC: В этом случае игрок не говорит, а просто выбирает, что ответить без текста.#currentSpeaker:speaker2 #portrait2:npc_neutral 
    NPC: В следующих диалогах будет такая схема.#currentSpeaker:speaker2
    NPC: Возвращаемся к выбору вариантов#currentSpeaker:speaker2
    -> FirstChoice
    +[Завершить этот диалог для перехода к следующему]
    NPC: Завершаем диалог(узел) и переходим к следующему?#currentSpeaker:speaker2 #portrait2:npc_neutral 
            **[Да]
                Player: Погнали дальше!#currentSpeaker:speaker1 #portrait1:player_happy
                ~CompleteDialogueKnot(CharacterName, "TestNpcCharacter1_Dialogue1")//invoked with var(string) and string param
                -> TestNpcCharacter1_Dialogue2
            ++[Нет, возвращаемся к старту диалога]
                Player: Нет, давай с самого начала!#currentSpeaker:speaker1 #portrait1:player_happy
                -> TestNpcCharacter1_Dialogue1
                
    *[Закрыть диалог]
        NPC: Выходим из диалога.#currentSpeaker:speaker2 #portrait2:npc_sad
- -> END