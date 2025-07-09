===TimeSkipEntity_BedReadingDialogue===

//если 3 раза подряд уже отдыхали - я не сейчас хочу учиться \ читать

Отдохнуть на кровати ? #speaker1name:Kat #portrait1:player_neutral //Отдохнуть на кровати ?
*[10 минут (+5 энергии +5 настроения)] //*[10 минут]
~LaunchCutscene("BedTimeSkipCutscene10")
*[30 минут (+15 энергии +15 настроения)] //*[30 минут]
~LaunchCutscene("BedTimeSkipCutscene30")
*[60 минут (+30 энергии +30 настроения)] //*[60 минут]
~LaunchCutscene("BedTimeSkipCutscene60")
*[Выход] //*[Выход]
-->END
