using System.Collections;
using _Scripts.Enums;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace _Scripts.NarrativeAndCutscenes.Cutscenes
{
    public class WakeUpAfterClaviLooseCutscene: AbstractCutscene
    {
        public override void LaunchCutscene(PlayableDirector director)
        {
            base.LaunchCutscene(director);
            StartCoroutine(AddStatsDelayed());
        }

        private IEnumerator AddStatsDelayed()
        {
            yield return new WaitForSeconds(2.1f);
            EventManager.Instance.TransitionEvents.TeleportPlayerBetweenPlaces("CatherineHouse","CatherineRoom");
            EventManager.Instance.TimeEvents.SetMinutes(0);
            EventManager.Instance.TimeEvents.SetHours(8);
            
            EventManager.Instance.PlayerStatsEvents.SetHealth(60);
            EventManager.Instance.PlayerStatsEvents.SetEnergy(50);
            EventManager.Instance.PlayerStatsEvents.SetMood(50);
            EventManager.Instance.PlayerStatsEvents.SetSatiety(50);
            
            EventManager.Instance.ReputationEvents.SetReputation("Mother", 10);

 
 
            /*EventManager.Instance.TimeEvents.set
            EventManager.Instance.PlayerStatsEvents.UpdateEnergy(-5);
            EventManager.Instance.PlayerStatsEvents.UpdateMood(5);*/
        }
        
        /*
        public override void DisplayNextImage()
        {
            DisplayMethod(false);
        }

        public override void DisplayNextImageWithFade()
        {
            DisplayMethod(true);
        }

        private void DisplayMethod(bool useDisplayWithFade)
        {
            
            var currentPlaceSprite = EventManager.Instance.LocationsAndPlacesEvents.GetCurrentPlaceSprite();
            TimeOfDayEnum timeOfDay = LocationManager.Instance.GetLocationTimeOfDayState("CatherineRoom");

            switch (timeOfDay)
            {
                case TimeOfDayEnum.Morning:
                {
                    if (useDisplayWithFade)
                    {
                        EventManager.Instance.CutsceneEvents.SwitchImageWithFade(currentPlaceSprite, cutsceneSprites[0]);

                    }
                    else
                    {
                        EventManager.Instance.CutsceneEvents.SetCutsceneImage(cutsceneSprites[0]);
                        EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false, 0);
                    }

                    break;
                }
                case TimeOfDayEnum.Afternoon:
                {
                    if (useDisplayWithFade)
                    {
                        EventManager.Instance.CutsceneEvents.SwitchImageWithFade(currentPlaceSprite, cutsceneSprites[1]);

                    }
                    else
                    {
                        EventManager.Instance.CutsceneEvents.SetCutsceneImage(cutsceneSprites[1]);
                        EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false, 0);
                    }

                    break;
                }
                case TimeOfDayEnum.Evening:
                {
                    if (useDisplayWithFade)
                    {
                        EventManager.Instance.CutsceneEvents.SwitchImageWithFade(currentPlaceSprite, cutsceneSprites[2]);

                    }
                    else
                    {
                        EventManager.Instance.CutsceneEvents.SetCutsceneImage(cutsceneSprites[2]);
                        EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false, 0);
                    }

                    break;
                }
                case TimeOfDayEnum.Night:
                {
                    if (useDisplayWithFade)
                    {
                        EventManager.Instance.CutsceneEvents.SwitchImageWithFade(currentPlaceSprite, cutsceneSprites[3]);

                    }
                    else
                    {
                        EventManager.Instance.CutsceneEvents.SetCutsceneImage(cutsceneSprites[3]);
                        EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false, 0);
                    }

                    break;
                }
            }

        }*/
    }
}