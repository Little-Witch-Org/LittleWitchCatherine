===Mother_Dialogue2===
dia_mother_2_1
    *[dia_mother_2_2]
    dia_mother_2_3
dia_mother_2_4
dia_mother_2_5
dia_mother_2_6
    ~UpdateReputation("Mother", -5)
    ~UpdateMood(-15)
    dia_mother_2_7
->StandUp
    *[dia_mother_2_8]
dia_mother_2_9
dia_mother_2_10
dia_mother_2_11
dia_mother_2_12
dia_mother_2_13
    ~UpdateReputation("Mother", -1)
    dia_mother_2_14
->StandUp
    *[dia_mother_2_15]
    dia_mother_2_16
        dia_mother_2_17
        ~UpdateHealth(-5)
        dia_mother_2_18
dia_mother_2_19
dia_mother_2_20
dia_mother_2_21
dia_mother_2_22
dia_mother_2_23
dia_mother_2_24
        ~UpdateReputation("Mother", -8)
        ~UpdateHealth(-5)
        ~UpdateMood(-5)
        ~AddMinutes(5)
        dia_mother_2_25

->Choice      

=Choice
        ++[dia_mother_2_26]
        ~AddMinutes(10)
        ~ SleepCount += 1
        ->SleepLoop
        
        **[dia_mother_2_27]
        ->StandUp
        

    
=SleepLoop
{ SleepCount < 3:
    (dia_mother_2_28 {SleepCount}, dia_mother_2_29)
    -> Choice
- else:
    dia_mother_2_30
    ~UpdateReputation("Mother", -5)
    ~UpdateHealth(-5)
    ~UpdateMood(-5)
    dia_mother_2_31
    -> StandUp
}

=StandUp
dia_mother_2_32
~ResumeCutscene()
~StartQuest(Quest1GoDownToKitchenId)
~CompleteDialogueKnot(CharacterName, "Mother_Dialogue2")
->DONE
        

-->END