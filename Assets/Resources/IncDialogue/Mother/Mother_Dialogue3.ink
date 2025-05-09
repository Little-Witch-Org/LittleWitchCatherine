===Mother_Dialogue3===
~FinishQuest(Quest1GoDownToKitchenId)
dia_mother_3_1
dia_mother_3_2
dia_mother_3_3
dia_mother_3_4
~AddMinutes(10)
~UpdateSatiety(30)
dia_mother_3_5

    *[dia_mother_3_6] 
        ~UpdateSatiety(10)
        ~UpdateMood(5)
        dia_mother_3_7
        ->toEat
    *[dia_mother_3_8] 
        ~UpdateSatiety(1)
        ~UpdateMood(-1)
        dia_mother_3_9
        ->toEat
        
=toEat
*[dia_mother_3_10]
    ~UpdateSatiety(5)
    ~UpdateMood(10)
    dia_mother_3_11
    -> continue_dialogue
*[dia_mother_3_12]
    ~ randomRoll = RANDOM(1, 100)
    { randomRoll <= 60:
    dia_mother_3_13
    dia_mother_3_14 
    -> continue_dialogue
    - else:
    dia_mother_3_15
    dia_mother_3_16
    -> continue_dialogue
    }

=continue_dialogue
*[dia_mother_3_17]
    ~UpdateReputation("Mother", -3)
    dia_mother_3_18
    ->autumnCleaning
*dia_mother_3_19
dia_mother_3_20
    ~UpdateSatiety(10)
    ~AddMinutes(10)
    dia_mother_3_21
    ->autumnCleaning
=autumnCleaning
dia_mother_3_22
dia_mother_3_23
dia_mother_3_24
dia_mother_3_25
dia_mother_3_26
dia_mother_3_27
dia_mother_3_28
dia_mother_3_29
dia_mother_3_30
dia_mother_3_31
dia_mother_3_32
dia_mother_3_33
dia_mother_3_34
dia_mother_3_35
~UpdateMood(10)
dia_mother_3_36
dia_mother_3_37
dia_mother_3_38
dia_mother_3_39
dia_mother_3_40
dia_mother_3_41
~CompleteDialogueKnot(CharacterName, "Mother_Dialogue3")
-->END