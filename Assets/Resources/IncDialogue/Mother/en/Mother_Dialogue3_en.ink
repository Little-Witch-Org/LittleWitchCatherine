===Mother_Dialogue3===
~FinishQuest(Quest1GoDownToKitchenId)
eng3_1
eng3_2
eng3_3
eng3_4
~AddMinutes(10)
~UpdateSatiety(30)
eng3_5

    *[eng3_6] 
        ~UpdateSatiety(10)
        ~UpdateMood(5)
        eng3_7
        ->toEat
    *[eng3_8] 
        ~UpdateSatiety(1)
        ~UpdateMood(-1)
        eng3_9
        ->toEat
        
=toEat
*[eng3_10]
    ~UpdateSatiety(5)
    ~UpdateMood(10)
    eng3_11
    -> continue_dialogue
*[eng3_12]
    ~ randomRoll = RANDOM(1, 100)
    { randomRoll <= 60:
    eng3_13
    eng3_14 
    -> continue_dialogue
    - else:
    eng3_15
    eng3_16
    -> continue_dialogue
    }

=continue_dialogue
*[eng3_17]
    ~UpdateReputation("Mother", -3)
    eng3_18
    ->autumnCleaning
*eng3_19
eng3_20
    ~UpdateSatiety(10)
    ~AddMinutes(10)
    eng3_21
    ->autumnCleaning
=autumnCleaning
eng3_22
eng3_23
eng3_24
eng3_25
eng3_26
eng3_27
eng3_28
eng3_29
eng3_30
eng3_31
eng3_32
eng3_33
eng3_34
eng3_35
~UpdateMood(10)
eng3_36
eng3_37
eng3_38
eng3_39
eng3_40
eng3_41
~CompleteDialogueKnot(CharacterName, "Mother_Dialogue3")
-->END
