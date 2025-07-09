using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace _Scripts.NarrativeAndCutscenes.Cutscenes
{
    public class BedTimeSkipCutscene60:AbstractCutscene
    {

        [SerializeField] public List<Sprite> alternativeSprites;
        
        private bool _isShowOriginalSprites;

        private new void Awake()
        {
            base.Awake();
            _isShowOriginalSprites  = Random.value > 0.5f;
        }
        public override void LaunchCutscene(PlayableDirector director)
        {
            base.LaunchCutscene(director);
            StartCoroutine(AddStatsDelayed());
        }

        private IEnumerator AddStatsDelayed()
        {
            yield return new WaitForSeconds(0.5f);
            EventManager.Instance.TimeEvents.AddMinutes(60);
            EventManager.Instance.PlayerStatsEvents.UpdateEnergy(30);
            EventManager.Instance.PlayerStatsEvents.UpdateMood(30);
        }

        public override void DisplayNextImage()
        {

            List<Sprite> tempSpriteList;

            if (_isShowOriginalSprites)
            {
                tempSpriteList = cutsceneSprites;
            }
            else
            {
                tempSpriteList = alternativeSprites;
            }

            TimeOfDayEnum timeOfDay = LocationManager.Instance.GetLocationTimeOfDayState("CatherineRoom");

            switch (timeOfDay)
            {
                case TimeOfDayEnum.Morning:
                {
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(tempSpriteList[0]);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
                    EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom");
                    break;
                }
                case TimeOfDayEnum.Afternoon:
                {
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(tempSpriteList[1]);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
                    EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom");
                    break;
                }
                case TimeOfDayEnum.Evening:
                {
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(tempSpriteList[2]);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
                    EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom");
                    break;
                }
                case TimeOfDayEnum.Night:
                {
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(tempSpriteList[3]);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
                    EventManager.Instance.LocationsAndPlacesEvents.UpdatePlaceStateSprite("CatherineRoom");
                    break;
                }
            }
        }

    }
}