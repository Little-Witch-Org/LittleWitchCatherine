===TimeSkipEntity_BedReadingDialogue===

//если 3 раза подряд уже отдыхали - я не сейчас хочу учиться \ читать

eng1_1 #speaker1name:Kat #portrait1:player_neutral //Отдохнуть на кровати ?
*[eng1_2] //*[10 минут]
~LaunchCutscene("BedTimeSkipCutscene10")
*[eng1_3] //*[30 минут]
~LaunchCutscene("BedTimeSkipCutscene30")
*[eng1_4] //*[60 минут]
~LaunchCutscene("BedTimeSkipCutscene60")
*[dia_bed_skip_1_5] //*[Выход]
-->END
