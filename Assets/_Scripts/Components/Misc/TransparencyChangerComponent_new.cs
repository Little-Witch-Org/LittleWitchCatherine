using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Components.Misc
{
    public class TransparencyChangerComponentNew : MonoBehaviour, IFadeable
    {
        [SerializeField] private float minValue = 0.3f;
        [SerializeField] private float maxValue = 1f;
        [SerializeField] private float fadeDuration = 0.5f;
        private SpriteRenderer _spriteRenderer;
        private Tween _tween;


        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void FadeIn()
        {
            KillCurrentTween();
            _tween = _spriteRenderer.DOFade(minValue, fadeDuration).SetEase(Ease.Linear).Play();
        }
        
        public void FadeOut()
        {
            KillCurrentTween();
            _tween = _spriteRenderer.DOFade(maxValue, fadeDuration).SetEase(Ease.Linear).Play();
        }

        
        private void OnDestroy()
        {
            KillCurrentTween(); //kill animation if scene changed
            DOTween.Kill(_spriteRenderer); //additional handling
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