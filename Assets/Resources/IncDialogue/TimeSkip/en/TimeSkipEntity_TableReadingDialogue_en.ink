===TimeSkipEntity_TableReadingDialogue===
//если 3 раза подряд уже отдыхали - я не сейчас хочу учиться \ читать

eng1_1 #speaker1name:Kat #portrait1:player_neutral //Почитать ?
*[eng1_2] //*[10 минут]
~LaunchCutscene("TableTimeSkipCutscene10")
*[eng1_3] //*[30 минут]
~LaunchCutscene("TableTimeSkipCutscene30")
*[eng1_4] //*[60 минут]
~LaunchCutscene("TableTimeSkipCutscene60")
*[dia_table_skip_1_5] //*[Выход]

-->END
