using System.Collections;
using _Scripts.Enums;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace _Scripts.NarrativeAndCutscenes.Cutscenes
{
    public class TableTimeSkipCutscene30 :AbstractCutscene
    {
        public override void LaunchCutscene(PlayableDirector director)
        {
            base.LaunchCutscene(director);
            StartCoroutine(AddStatsDelayed());
        }

        private IEnumerator AddStatsDelayed()
        {
            yield return new WaitForSeconds(0.5f);
            EventManager.Instance.TimeEvents.AddMinutes(30);
            EventManager.Instance.PlayerStatsEvents.UpdateEnergy(-15);
            EventManager.Instance.PlayerStatsEvents.UpdateMood(15);
        }

        public override void DisplayNextImage()
        {


            TimeOfDayEnum timeOfDay = LocationManager.Instance.GetLocationTimeOfDayState("CatherineRoom");

            switch (timeOfDay)
            {
                case TimeOfDayEnum.Morning:
                {
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(cutsceneSprites[0]);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
                    EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom");
                    break;
                }
                case TimeOfDayEnum.Afternoon:
                {
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(cutsceneSprites[1]);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
                    EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom");
                    break;
                }
                case TimeOfDayEnum.Evening:
                {
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(cutsceneSprites[2]);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
                    EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom");
                    break;
                }
                case TimeOfDayEnum.Night:
                {
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(cutsceneSprites[3]);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
                    EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom");
                    break;
                }
            }
        }
    }
}