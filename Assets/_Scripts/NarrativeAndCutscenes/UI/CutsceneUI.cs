using System;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.NarrativeAndCutscenes.UI
{
    /// <summary>
    /// Show cutscene images from storyProgressManager
    /// </summary>
    public class CutsceneUI : MonoBehaviour
    {
        [SerializeField] private GameObject contentParent;
        [SerializeField] private Image backingPanel;
        [SerializeField] private GameObject cutsceneImageGoBottom; //main image
        [SerializeField] private Image cutsceneImageBottom;
        
        [SerializeField] private GameObject cutsceneImageGoTop;//alternative image used for transparent animations
        [SerializeField] private Image cutsceneImageTop;
        
        //todo change to cutsceneImageBottom and cutsceneImageTop. add transparent change (bottom is old image. Top image fadeout (turn on with alpha=0) -> bottom = top -> top disables


        private void OnEnable()
        {
            EventManager.Instance.CutsceneEvents.OnShowCutsceneUI += ShowUI;
            EventManager.Instance.CutsceneEvents.OnHideCutsceneBackground += HideUI;
            
            EventManager.Instance.CutsceneEvents.OnSetCutsceneImage += SetCutsceneImage;
            EventManager.Instance.CutsceneEvents.OnShowCutsceneImage += ShowCutsceneImage;
            EventManager.Instance.CutsceneEvents.OnHideCutsceneImage += HideCutsceneImage;
            
            EventManager.Instance.CutsceneEvents.OnShowBackingPanel += ShowBackingPanel;
            EventManager.Instance.CutsceneEvents.OnHideBackingPanel += HideBackingPanel;
            
            EventManager.Instance.CutsceneEvents.OnSwitchImageWithFade += SwitchImageWithFade;

            EventManager.Instance.CutsceneEvents.OnCurrentCutsceneSpriteRequest += GetCurrentCutsceneSpriteFromUI;



        }

        private void OnDisable()
        {
            EventManager.Instance.CutsceneEvents.OnShowCutsceneUI -= ShowUI;
            EventManager.Instance.CutsceneEvents.OnHideCutsceneBackground -= HideUI;
            
            EventManager.Instance.CutsceneEvents.OnSetCutsceneImage -= SetCutsceneImage;
            EventManager.Instance.CutsceneEvents.OnShowCutsceneImage -= ShowCutsceneImage;
            EventManager.Instance.CutsceneEvents.OnHideCutsceneImage -= HideCutsceneImage;
            
            EventManager.Instance.CutsceneEvents.OnShowBackingPanel -= ShowBackingPanel;
            EventManager.Instance.CutsceneEvents.OnHideBackingPanel -= HideBackingPanel;
            
            EventManager.Instance.CutsceneEvents.OnSwitchImageWithFade -= SwitchImageWithFade;
            
            EventManager.Instance.CutsceneEvents.OnCurrentCutsceneSpriteRequest += GetCurrentCutsceneSpriteFromUI;
            
            KillCurrentTween();
            KillCurrentSequence();
        }


        private void ShowUI()
        {
            contentParent.SetActive(true);
        }

        private void HideUI()
        {
            contentParent.SetActive(false);
        }

        private void SetCutsceneImage(Sprite sprite)
        {
            cutsceneImageBottom.sprite = sprite;
            cutsceneImageBottom.preserveAspect = true;
            
        }
        
        private void ShowCutsceneImage(bool isFadeIn,float fadeDuration)
        {
            if (!isFadeIn)
            {
                cutsceneImageGoBottom.SetActive(true);
            }
            else
            {
                FadeOut(cutsceneImageBottom, fadeDuration);
            }
        }
        private void HideCutsceneImage(bool isFadeIn, float fadeDuration)
        {
            if (!isFadeIn)
            {
                cutsceneImageGoBottom.SetActive(false);
            }
            else
            {
                FadeIn(cutsceneImageBottom, fadeDuration);
            }
        }

        private void ShowBackingPanel(bool isFadeIn,float fadeDuration)
        {
            if (!isFadeIn)
            {
                backingPanel.color = new Color(0f, 0f, 0f, 1f);
            }
            else
            {
                FadeIn(backingPanel, fadeDuration);
            }
        }
        
        private void HideBackingPanel(bool isFadeOut, float fadeDuration)
        {
            if (!isFadeOut)
            {
                backingPanel.color = new Color(0f, 0f, 0f, 0f);
            }
            else
            {
                FadeOut(backingPanel, fadeDuration);
            }
        }

        private void SwitchImageWithFade(Sprite spriteBottom, Sprite spriteTop)
        {
            KillCurrentSequence();

            Debug.Log(spriteBottom.name);
            Debug.Log(spriteTop.name);
            SetCutsceneImage(spriteBottom);
            ShowCutsceneImage(false, 0);


            cutsceneImageTop.color = new Color(1f, 1f, 1f, 0f);
            cutsceneImageGoTop.SetActive(true);
            cutsceneImageTop.sprite = spriteTop;
            cutsceneImageTop.preserveAspect = true;

            _sequence = DOTween.Sequence();

            _sequence
                .Append(cutsceneImageTop.DOFade(1f, 2f).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    SetCutsceneImage(spriteTop);
                    cutsceneImageGoTop.SetActive(false);
                }).Play();
            
        }

        private Sprite GetCurrentCutsceneSpriteFromUI()
        {
            return cutsceneImageBottom.sprite;
        }



        //tween stuff
        private Tween _tween;
        private Sequence _sequence;

        private void FadeIn(Image image, float duration)
        {
            KillCurrentTween();
            _tween = image.DOFade(0f, duration).SetEase(Ease.Linear).Play();
        }
        private void FadeOut(Image image, float duration)
        {
            KillCurrentTween();
            _tween = image.DOFade(1f, duration).SetEase(Ease.Linear).Play();
        }


        private bool InAnimation() => _tween != null && _tween.IsActive();

        private void KillCurrentTween()
        {
            if (InAnimation())
            {
                _tween.Kill();
            }
        }
        
        public bool InSequenceAnimation() => _sequence != null && _sequence.active;

        private void KillCurrentSequence()
        {
            if (InSequenceAnimation())
            {
                _sequence.Kill();
            }
        }
    }
}