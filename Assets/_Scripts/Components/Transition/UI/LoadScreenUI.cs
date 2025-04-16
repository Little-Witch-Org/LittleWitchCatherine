using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Components.Transition.UI
{
    public class LoadScreenUI : MonoBehaviour
    {
        [SerializeField] private GameObject contentParent;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private Image loadingIconImage;

        private Tween _tween;

        private void Awake()
        {
            contentParent.SetActive(false);
        }

        public void FadeInLoadingScreen()
        {
            KillCurrentTween();
            contentParent.SetActive(true);
            _tween = backgroundImage.DOFade(1f, 0.7f).Play().OnComplete(() =>
                {
                    loadingText.gameObject.SetActive(true);
                    loadingIconImage.gameObject.SetActive(true); //todo use dotween to rotate?
                }
            );
        }

        public void LoadingProgressAnimationStart()
        {
            _tween = loadingIconImage.rectTransform.DORotate(new Vector3(0, 0, -360), 1f,RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1,LoopType.Restart)
                .Play();
        }



        public void FadeOutLoadingScreen()
        {
            KillCurrentTween();
            loadingText.gameObject.SetActive(false);
            loadingIconImage.gameObject.SetActive(false); //todo use dotween to rotate?
            _tween = backgroundImage.DOFade(0f, 0.7f).Play().OnComplete(()=>
            {
                contentParent.SetActive(false);
            });
        }
        
        private bool InAnimation() => _tween != null && _tween.IsActive();

        private void KillCurrentTween()
        {
            if (InAnimation())
            {
                _tween.Kill();
            }
        }
        
    }
}