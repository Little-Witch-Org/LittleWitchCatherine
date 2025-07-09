===TimeSkipEntity_TableReadingDialogue===
//если 3 раза подряд уже отдыхали - я не сейчас хочу учиться \ читать

Почитать ? #speaker1name:Kat #portrait1:player_neutral //Почитать ?
*[10 минут (-5 энергии +5 настроения)] //*[10 минут]
~LaunchCutscene("TableTimeSkipCutscene10")
*[30 минут (-15 энергии +15 настроения)] //*[30 минут]
~LaunchCutscene("TableTimeSkipCutscene30")
*[60 минут (-30 энергии +30 настроения)] //*[60 минут]
~LaunchCutscene("TableTimeSkipCutscene60")
*[Выход] //*[Выход]

-->END
