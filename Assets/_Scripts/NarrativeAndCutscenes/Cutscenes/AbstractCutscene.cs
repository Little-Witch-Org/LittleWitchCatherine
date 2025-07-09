using System.Collections.Generic;
using _Scripts.Managers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace _Scripts.NarrativeAndCutscenes.Cutscenes
{
    public class AbstractCutscene : MonoBehaviour
    {
        [SerializeField] private string cutsceneName;

        [Header("Timeline object for cutscene")] 
        [SerializeField] private TimelineAsset timelineAsset;

        [Header("Cutscene Sprites")] 
        [SerializeField] protected List<Sprite> cutsceneSprites;

        protected List<KeyValuePair<Sprite, bool>> CutsceneSpritesList= new();


        protected void Awake()
        {
            InitializeCutsceneSprites();
        }

        private void InitializeCutsceneSprites()
        {
            foreach (var sprite in cutsceneSprites)
            {
                CutsceneSpritesList.Add(new KeyValuePair<Sprite, bool>(sprite,false));
            }
            
        }

        public string GetCutsceneName()
        {
            return cutsceneName;
        }
        
        public virtual void LaunchCutscene(PlayableDirector director)
        {
            EventManager.Instance.CutsceneEvents.CutsceneStarted();
            director.playableAsset = GetTimeline();
            director.Play();
            Debug.Log($"Cutscene {cutsceneName} started");
        }

        public TimelineAsset GetTimeline()
        {
            return timelineAsset;
        }

        /*public virtual void DisplayNextImage()
        {
            foreach (var pair in cutsceneSpritesList)
            {
                if (!pair.Value)
                {
                    // Обновляем статус картинки на "показана" (true)
                    // Поскольку KeyValuePair — это структура (struct), мы не можем изменить ее напрямую,
                    // поэтому нужно найти индекс и заменить элемент в списке.
                    int index = cutsceneSpritesList.IndexOf(pair);
                    if (index != -1)
                    {
                        cutsceneSpritesList[index] = new KeyValuePair<Sprite, bool>(pair.Key, true);
                    }
            
                    break; // Выходим после первого найденного
                }
            }
        }*/

        public virtual void DisplayNextImage()
        {
            for (int i = 0; i < CutsceneSpritesList.Count; i++)
            {
                if (!CutsceneSpritesList[i].Value)
                {
                    // Обновляем статус картинки на "показана" (true)
                    // Поскольку KeyValuePair — это структура (struct), мы не можем изменить ее напрямую,
                    // поэтому нужно найти индекс и заменить элемент в списке.
                    EventManager.Instance.CutsceneEvents.SetCutsceneImage(CutsceneSpritesList[i].Key);
                    EventManager.Instance.CutsceneEvents.ShowCutsceneImage(false,0);
                    CutsceneSpritesList[i] = new KeyValuePair<Sprite, bool>(CutsceneSpritesList[i].Key, true);
                    

                    break; // Выходим после первого найденного
                }
            }
        }
        
        public virtual void DisplayNextImageWithFade()
        {
            for (int i = 0; i < CutsceneSpritesList.Count; i++)
            {
                if (!CutsceneSpritesList[i].Value)
                {
                    var sprite = EventManager.Instance.CutsceneEvents.CurrentCutsceneSpriteRequest();
                    
                    EventManager.Instance.CutsceneEvents.SwitchImageWithFade(sprite, CutsceneSpritesList[i].Key); //old sprite (we get current on ui), new sprite (from list)
                    
                    CutsceneSpritesList[i] = new KeyValuePair<Sprite, bool>(CutsceneSpritesList[i].Key, true);
                    

                    break;
                }
            }
        }

        public virtual void DisplayCustomImageWithFade(Sprite customSprite) //use customSprite instead of sprites from cutscene list
        {
            var sprite = EventManager.Instance.CutsceneEvents.CurrentCutsceneSpriteRequest();

            EventManager.Instance.CutsceneEvents.SwitchImageWithFade(sprite,
                customSprite); //old sprite (we get current on ui), new sprite (from parameter)
        }
    }
}