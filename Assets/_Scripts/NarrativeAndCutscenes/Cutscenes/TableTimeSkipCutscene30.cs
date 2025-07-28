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
            yield return new WaitForSeconds(4f);
            EventManager.Instance.TimeEvents.AddMinutes(30);
            EventManager.Instance.PlayerStatsEvents.UpdateEnergy(-15);
            EventManager.Instance.PlayerStatsEvents.UpdateMood(15);
        }

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

            EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom", false);

        }
    }
}