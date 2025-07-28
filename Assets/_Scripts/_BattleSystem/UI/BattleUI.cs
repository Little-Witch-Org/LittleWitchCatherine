using System;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts._BattleSystem.UI
{
    /// <summary>
    /// todo add battle debug log
    /// </summary>
    public class BattleUI : MonoBehaviour
    {
         
        [Header("Components")] 
        [SerializeField] private GameObject contentParent;

        
        
        private void OnEnable()
        {
            EventManager.Instance.BattleEvents.OnBattleStarted += BattleStarted;
            EventManager.Instance.BattleEvents.OnBattleFinished += BattleFinished;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.BattleEvents.OnBattleStarted -= BattleStarted;
            EventManager.Instance.BattleEvents.OnBattleFinished -= BattleFinished;
        }

        private void BattleStarted(Battle obj) //todo obj can be used to display info (spawn points, enemies, etc (battle will be implemented using ui)
        {
            Debug.Log($"Battle started. battle id: {obj.battleId}");
            contentParent.SetActive(true);
        }
        
        private void BattleFinished(Battle obj)
        {
            contentParent.SetActive(false);
        }





        public void FinishSpirit() //door dialogue battle_finish_variant = 1
        {
            Debug.Log("Finish Spirit");
            EventManager.Instance.BattleEvents.FinishBattle("id_1");
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("DoorToLivingRoom_StoryMain","battle_finish_variant", "1");
            EventManager.Instance.DialogueEvents.StartDialogueWithNpc("DoorToLivingRoom");
            EventManager.Instance.PlayerStatsEvents.SetHealth(100);
            
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("Mother_StoryMain", "claviBroken", false);
        }
        
        public void FinishStatue()//door dialogue battle_finish_variant = 2
        {
            Debug.Log("Finish Statue");
            EventManager.Instance.BattleEvents.FinishBattle("id_1");
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("DoorToLivingRoom_StoryMain","battle_finish_variant", "2");
            EventManager.Instance.DialogueEvents.StartDialogueWithNpc("DoorToLivingRoom");
            EventManager.Instance.PlayerStatsEvents.SetHealth(70);
            
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("Mother_StoryMain", "claviBroken", false);
        }
        
        public void FinishLovHp()//door dialogue battle_finish_variant = 3 
        {
            Debug.Log("Finish LovHp");
            EventManager.Instance.BattleEvents.FinishBattle("id_1");
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("DoorToLivingRoom_StoryMain","battle_finish_variant", "3");
            EventManager.Instance.DialogueEvents.StartDialogueWithNpc("DoorToLivingRoom");
            EventManager.Instance.PlayerStatsEvents.SetHealth(20);
            
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("Mother_StoryMain", "claviBroken", false);
        }

        public void FinishClavi()//door dialogue battle_finish_variant = 4 (clavi broken)
        {
            Debug.Log("Finish Clavi");
            EventManager.Instance.BattleEvents.FinishBattle("id_1");
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("DoorToLivingRoom_StoryMain","battle_finish_variant", "4");
            EventManager.Instance.DialogueEvents.StartDialogueWithNpc("DoorToLivingRoom");
            EventManager.Instance.PlayerStatsEvents.SetHealth(20);
            
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("Mother_StoryMain", "claviBroken", true);
        }
        
        public void Loose()//door dialogue battle_finish_variant = 5
        {
            Debug.Log("Loose");
            EventManager.Instance.BattleEvents.FinishBattle("id_1");
            EventManager.Instance.DialogueEvents.UpdateInkDialogueVariable("DoorToLivingRoom_StoryMain","battle_finish_variant", "5");
            EventManager.Instance.DialogueEvents.StartDialogueWithNpc("DoorToLivingRoom");
            EventManager.Instance.PlayerStatsEvents.SetHealth(3);
        }
    }
}