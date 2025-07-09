using System;
using UnityEngine;

namespace _Scripts.Events
{
    public class CutsceneEvents
    {
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

        public event Action OnResumeCutscene;

        public void ResumeCutscene()
        {
            OnResumeCutscene?.Invoke();
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

        public event Action<Sprite, Sprite> OnSwitchImageWithFade;

        public void SwitchImageWithFade(Sprite bottomSprite, Sprite topSprite)
        {
            OnSwitchImageWithFade?.Invoke(bottomSprite, topSprite);
        }

        public event Action<bool, float> OnShowCutsceneImage;

        public void ShowCutsceneImage(bool isUsingFade, float fadeDuration)
        {
            OnShowCutsceneImage?.Invoke(isUsingFade, fadeDuration);
        }

        public event Action<bool, float> OnHideCutsceneImage;

        public void HideCutsceneImage(bool isUsingFade, float fadeDuration)
        {
            OnHideCutsceneImage?.Invoke(isUsingFade, fadeDuration);
        }

        public event Action<bool, float> OnShowBackingPanel;

        public void ShowBackingPanel(bool isUsingFade, float fadeDuration)
        {
            OnShowBackingPanel?.Invoke(isUsingFade, fadeDuration);
        }

        public event Action<bool, float> OnHideBackingPanel;

        public void HideBackingPanel(bool isUsingFade, float fadeDuration)
        {
            OnHideBackingPanel?.Invoke(isUsingFade, fadeDuration);
        }

        public event Func<Sprite> OnCurrentCutsceneSpriteRequest;

        public Sprite CurrentCutsceneSpriteRequest()
        {
           return OnCurrentCutsceneSpriteRequest?.Invoke();
        }
    }
}