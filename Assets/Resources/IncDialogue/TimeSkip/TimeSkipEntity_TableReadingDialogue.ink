===TimeSkipEntity_TableReadingDialogue===
//если 3 раза подряд уже отдыхали - я не сейчас хочу учиться \ читать

dia_table_skip_1_1 //Почитать ?
*[dia_table_skip_1_2] //*[10 минут]
~LaunchCutscene("TableTimeSkipCutscene10")
*[dia_table_skip_1_3] //*[30 минут]
~LaunchCutscene("TableTimeSkipCutscene30")
*[dia_table_skip_1_4] //*[60 минут]
~LaunchCutscene("TableTimeSkipCutscene60")
*[dia_table_skip_1_5] //*[Выход]

-->END