===DoorToLivingRoom_Dialogue3===
{battle_finish_variant :
        -"1": -> FinishSpirit
        -"2": -> FinishStatue
        -"3": -> FinishLovHp
        -"4": -> FinishClavi
        -"5": -> Loose
}

=FinishSpirit
Кет: -И стоило так волноваться из-за этой мелюзги... осталось только прибраться.
->CleanRoomUnbroken

=FinishStatue
~AddItem("inventory_item_id_1_5","stairs")
Кет: «А ты расслабилась Рин... надо бы за подорожником сходить...»
->CleanRoomUnbroken

=FinishLovHp
~AddItem("inventory_item_id_1_5","stairs")
Кет: «Ты что на столько не хочешь убираться Ринни? Что-то мне нехорошо.
->CleanRoomUnbroken


=FinishClavi
~AddItem("inventory_item_id_1_5","stairs")
~SetPlaceStateAndApplyWithFade("LivingRoom","LivingRoom.PlaceStateEnum.MessBrokenClavi")
Кет: «Ну... мама  ведь просила  разобраться с  гостями так ведь... а для победы все средства хороши...»
->CleanRoomBroken

=Loose
~FinishCurrentQuestStep("Quest2CleanLivingroomFixClavecin",true)//3
->DONE

=CleanRoomUnbroken
*[Прибраться в комнате]
~SetPlaceStateAndApplyWithFade("LivingRoom","LivingRoom.PlaceStateEnum.Default")
(+50 минут)
~AddMinutes(50)
(журнал обновлён)
~FinishCurrentQuestStep("Quest2CleanLivingroomFixClavecin",false) //3
->RoomCleaned

=CleanRoomBroken
*[Прибраться в комнате]
~SetPlaceStateAndApplyWithFade("LivingRoom","LivingRoom.PlaceStateEnum.CleanedBrokenClavi")
(+50 минут)
~AddMinutes(50)
(журнал обновлён)
~FinishCurrentQuestStep("Quest2CleanLivingroomFixClavecin",false)//3
->RoomCleaned

=RoomCleaned
Кет: «Другое дело… может не идеально, но не зря ведь говорят — стоит достичь совершенства, и совершенное начинает разрушаться… Кажется в Келхарском халифате...или все же Цан-Цане? Словом пора заканчивать с этим, Рин»
->DONE

-->END
