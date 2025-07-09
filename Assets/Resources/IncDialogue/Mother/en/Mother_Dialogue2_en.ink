===Mother_Dialogue2===
eng2_1 #speaker1name:Kat #portrait1:player_neutral #speaker2name:Annaliksa #portrait2:mother_neutral
    *[eng2_2]
    eng2_3
eng2_4
eng2_5
eng2_6
    ~UpdateReputation("Mother", -5)
    ~UpdateMood(-15)
    eng2_7
->StandUp
    *[eng2_8]
eng2_9
eng2_10
eng2_11
eng2_12
eng2_13
    ~UpdateReputation("Mother", -1)
    eng2_14
->StandUp
    *[eng2_15]
    eng2_16
        eng2_17
        ~UpdateHealth(-5)
        eng2_18
eng2_19
eng2_20
eng2_21
eng2_22
eng2_23
eng2_24
        ~UpdateReputation("Mother", -8)
        ~UpdateHealth(-5)
        ~UpdateMood(-5)
        ~AddMinutes(5)
        eng2_25

->Choice      

=Choice
        ++[eng2_26]
        ~AddMinutes(10)
        ~ SleepCount += 1
        ->SleepLoop
        
        **[eng2_27]
        ->StandUp
        

    
=SleepLoop
{ SleepCount < 3:
    (eng2_28 {SleepCount}, eng2_29)
    -> Choice
- else:
    eng2_30
    ~UpdateReputation("Mother", -5)
    ~UpdateHealth(-5)
    ~UpdateMood(-5)
    eng2_31
    -> StandUp
}

=StandUp
eng2_32
~StartQuest(Quest1GoDownToKitchenId)
~CompleteDialogueKnot(CharacterName, "Mother_Dialogue2")
->DONE
        

-->END
