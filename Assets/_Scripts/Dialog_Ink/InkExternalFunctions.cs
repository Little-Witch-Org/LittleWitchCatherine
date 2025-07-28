using System;
using System.Collections.Generic;
using System.Reflection;
using _Scripts.Enums.Locations;
using _Scripts.LocationsAndPlaces.Places.CatherineHouse;
using _Scripts.Managers;
using Ink.Runtime;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    public class InkExternalFunctions
    {
        //bind ink external function with class methods //todo do we need to handle all type of methods ? 
        //todo We can bind all ext funk to all stories. But it will be better to add .ink parser and bind only declared.
        public void Bind(Story story)
        {
            story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
            story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
            story.BindExternalFunction("CompleteDialogueKnot", (string characterName, string dialogueKnotName) => CompleteDialogueKnot(characterName, dialogueKnotName));
            story.BindExternalFunction("LaunchCutscene", (string cutsceneId) => LaunchCutscene(cutsceneId));
            story.BindExternalFunction("ResumeCutscene", () => ResumeCutscene());

            story.BindExternalFunction("AddMinutes",  (int minutes) =>
            {
                //Debug.Log($"AddMinutes called with: {minutes}");
                AddMinutes(minutes);
            });
            
            story.BindExternalFunction("FinishCurrentQuestStep", (string questId, bool isFailed) => FinishCurrentQuestStep(questId,isFailed));

            //stats
            story.BindExternalFunction("SetHealth", (int health) => SetHealth(health));
            story.BindExternalFunction("SetSatiety", (int satiety) => SetSatiety(satiety));
            story.BindExternalFunction("SetMood", (int mood) => SetMood(mood));
            story.BindExternalFunction("SetEnergy", (int energy) => SetEnergy(energy));
            
            story.BindExternalFunction("UpdateHealth", (int health) => UpdateHealth(health));
            story.BindExternalFunction("UpdateSatiety", (int satiety) => UpdateSatiety(satiety));
            story.BindExternalFunction("UpdateMood", (int mood) => UpdateMood(mood));
            story.BindExternalFunction("UpdateEnergy", (int energy) => UpdateEnergy(energy));
            
            //rep
            story.BindExternalFunction("UpdateReputation", (string npcName, int reputation) => UpdateReputation(npcName,reputation));
            story.BindExternalFunction("SetReputation", (string npcName, int reputation) => SetReputation(npcName,reputation));
            
            //game
            story.BindExternalFunction("ActivateCheckpoint", (int checkpointId) => ActivateCheckpoint(checkpointId));
            
            //locations and places
            story.BindExternalFunction("SetPlaceStateAndApplyWithFade", (string placeName, string fullEnumPath) => 
            {
                if (TryParsePlaceState(placeName, fullEnumPath, out var state))
                {
                    SetPlaceStateAndApplyWithFade(placeName, state);
                }
            });
            story.BindExternalFunction("TeleportPlayerBetweenPlaces", (string location ,string place) => TeleportPlayerBetweenPlaces(location, place));
            
            //battle
            story.BindExternalFunction("StartBattle", (string battleId) => StartBattle(battleId));
            
            //items
            story.BindExternalFunction("AddItem", (string itemId, string gridName) => AddItem(itemId, gridName));
        }
        
        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("StartQuest");
            story.UnbindExternalFunction("FinishQuest");
            story.UnbindExternalFunction("CompleteDialogueKnot");
            story.UnbindExternalFunction("LaunchCutscene");
            story.UnbindExternalFunction("ResumeCutscene");
            
            story.UnbindExternalFunction("AddMinutes");
            
            story.UnbindExternalFunction("FinishCurrentQuestStep");
            
            
            story.UnbindExternalFunction("UpdateHealth");
            story.UnbindExternalFunction("UpdateSatiety");
            story.UnbindExternalFunction("UpdateMood");
            story.UnbindExternalFunction("UpdateEnergy");
            
            story.UnbindExternalFunction("UpdateReputation");
            
            story.UnbindExternalFunction("ActivateCheckpoint");
            
            story.UnbindExternalFunction("SetPlaceStateAndApplyWithFade");
        }
        
        
        
        
        private void StartQuest(string questId)
        {
            EventManager.Instance.QuestEvents.StartQuest(questId);
        }
        
        private void FinishQuest(string questId)
        {
            EventManager.Instance.QuestEvents.FinishQuest(questId);
        }
        
        private void CompleteDialogueKnot(string characterName, string dialogueKnotName)
        {
            EventManager.Instance.DialogueEvents.CompleteDialogueKnot(characterName, dialogueKnotName);
        }
        
        private void LaunchCutscene( string cutsceneId)
        {
            EventManager.Instance.CutsceneEvents.LaunchCutscene(cutsceneId);
        }

        private void ResumeCutscene()
        {
            EventManager.Instance.CutsceneEvents.ResumeCutscene();
        }

        private void AddMinutes(int minutes)
        {
            TimeManager.Instance.AddMinutes(minutes);
        }

        private void FinishCurrentQuestStep(string questId,bool isFailed)
        {
            EventManager.Instance.QuestEvents.FinishCurrentQuestStep(questId,isFailed);
        }
        
        
        //change player stats
        //set
        private void SetHealth(int health)
        {
            EventManager.Instance.PlayerStatsEvents.SetHealth(health);
        }
        private void SetSatiety(int satiety)
        {
            EventManager.Instance.PlayerStatsEvents.SetSatiety(satiety);
        }
        private void SetMood(int mood)
        {
            EventManager.Instance.PlayerStatsEvents.SetMood(mood);
        }
        private void SetEnergy(int energy)
        {
            EventManager.Instance.PlayerStatsEvents.SetEnergy(energy);
        }

        //update
        private void UpdateHealth(int health)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateHealth(health);
        }
        private void UpdateSatiety(int satiety)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateSatiety(satiety);
        }
        private void UpdateMood(int mood)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateMood(mood);
        }
        private void UpdateEnergy(int energy)
        {
            EventManager.Instance.PlayerStatsEvents.UpdateEnergy(energy);
        }
        
        //reputation
        private void SetReputation(string npcName, int reputation)
        {
            EventManager.Instance.ReputationEvents.SetReputation(npcName, reputation);
        }
        
        private void UpdateReputation(string npcName, int reputation)
        {
            EventManager.Instance.ReputationEvents.UpdateReputation(npcName, reputation);
        }
        
        //game
        private void ActivateCheckpoint(int checkpointId)
        {
            EventManager.Instance.GameEvents.ActivateCheckpoint(checkpointId);
        }
        
        //locations and places
        private void SetPlaceStateAndApplyWithFade(string placeName, Enum state)
        {
            EventManager.Instance.LocationsAndPlacesEvents.SetPlaceStateAndApplyWithFade(placeName, state);
        }
        
        private void TeleportPlayerBetweenPlaces(string location, string place)
        {
            EventManager.Instance.TransitionEvents.TeleportPlayerBetweenPlaces(location, place);
        }
        
        //battle
        private void StartBattle(string battleId)
        {
            EventManager.Instance.BattleEvents.StartBattle(battleId);
        }
        
        //items
        private void AddItem(string itemId, string itemGrid)
        {
            EventManager.Instance.InventoryEvents.AddItem(itemId, itemGrid);
        }
        
        
        
        
        //utils
        private static bool TryParsePlaceState(
            string placeName, 
            string fullEnumPath, 
            out Enum state
        ) {
            state = null;
    
            // Проверяем базовый формат "PlaceName.PlaceStateEnum.StateName"
            if (!fullEnumPath.StartsWith(placeName + "."))
            {
                Debug.LogError($"Название места '{placeName}' не совпадает с путём '{fullEnumPath}'");
                return false;
            }

            // Разбиваем путь на части
            string[] parts = fullEnumPath.Split('.');
            if (parts.Length != 3 || parts[1] != "PlaceStateEnum")
            {
                Debug.LogError($"Неверный формат enum. Ожидается 'PlaceName.PlaceStateEnum.StateName', получено: {fullEnumPath}");
                return false;
            }

            // Получаем имя состояния (последняя часть)
            string stateName = parts[2];

            // Простая ручная привязка мест к их enum типам
            switch (placeName)
            {
                case "FFCorridor":
                    state = ParseEnum<FFCorridor.PlaceStateEnum>(stateName);
                    return true;
                case "GuestBedroom":
                    state = ParseEnum<GuestBedroom.PlaceStateEnum>(stateName);
                    return true;
                case "LivingRoom":
                    state = ParseEnum<LivingRoom.PlaceStateEnum>(stateName);
                    return true;
                
                
                // Добавьте другие места по аналогии
                default:
                    Debug.LogError($"Неизвестное место: {placeName}");
                    return false;
            }
        }

        // Вспомогательный метод для парсинга enum
        private static T ParseEnum<T>(string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}