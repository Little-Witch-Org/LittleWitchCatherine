using System;
using UnityEngine;

namespace _Scripts.Events
{
    public class CutsceneEvents
    {
        //todo will be used in progression manager after separation to cutscene manager
        public event Action<string> OnLaunchCutscene;

        public void LaunchCutscene(string cutsceneName) 
        {
            OnLaunchCutscene?.Invoke(cutsceneName);
        }
        
        
        public event Action OnCutsceneStarted;

        public void CutsceneStarted()
        {
            OnCutsceneStarted?.Invoke();
        }
        
        public event Action OnCutsceneFinished;

        public void CutsceneFinished()
        {
            OnCutsceneFinished?.Invoke();
        }
        
        public event Action OnShowCutsceneUI;

        public void ShowCutsceneUI()
        {
            OnShowCutsceneUI?.Invoke();
        }

        public event Action OnHideCutsceneBackground;

        public void HideCutsceneBackground()
        {
            OnHideCutsceneBackground?.Invoke();
        }

        public event Action<Sprite> OnSetCutsceneImage;

        public void SetCutsceneImage(Sprite sprite)
        {
            OnSetCutsceneImage?.Invoke(sprite);
        }

        public event Action OnShowCutsceneImage;

        public void ShowCutsceneImage()
        {
            OnShowCutsceneImage?.Invoke();
        }

        public event Action OnHideCutsceneImage;

        public void HideCutsceneImage()
        {
            OnHideCutsceneImage?.Invoke();
        }
        
        public event Action OnResumeCutscene;

        public void ResumeCutscene()
        {
            OnResumeCutscene?.Invoke();
        }
    }
}