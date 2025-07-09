===TimeSkipEntity_BedReadingDialogue===

//если 3 раза подряд уже отдыхали - я не сейчас хочу учиться \ читать

dia_bed_skip_1_1 //Отдохнуть на кровати ?
*[dia_bed_skip_1_2] //*[10 минут]
~LaunchCutscene("BedTimeSkipCutscene10")
*[dia_bed_skip_1_3] //*[30 минут]
~LaunchCutscene("BedTimeSkipCutscene30")
*[dia_bed_skip_1_4] //*[60 минут]
~LaunchCutscene("BedTimeSkipCutscene60")
*[dia_bed_skip_1_5] //*[Выход]
-->END