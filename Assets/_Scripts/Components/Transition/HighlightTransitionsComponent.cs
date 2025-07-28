using _Scripts.Enums;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Components.Transition
{
    public class HighlightTransitionsComponent : MonoBehaviour
    {
        
        private SpriteRenderer _spriteRenderer;
        private readonly float _fadeDuration = 0.5f;
        
        
        
        private void Start()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            
            if (_spriteRenderer == null)
            {
                Debug.LogError("There is no SpriteRenderer on the object!");
            }
        }


        private void Update()
        {
            if (!EventManager.Instance.InputEvents.GetInputEventContext().Equals(InputEventContext.Default))
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Show();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                Hide();
            }
        }

        private void OnDisable()
        {
            KillCurrentTween(); //kill animation after transition
        }
        

        private Tween _tween;
        
        private void Show()
        {
            KillCurrentTween();
            _tween = _spriteRenderer.DOFade(0.3f, _fadeDuration).SetEase(Ease.Linear).Play();
        }

        private void Hide()
        {
            KillCurrentTween();
            _tween = _spriteRenderer.DOFade(0, _fadeDuration).SetEase(Ease.Linear).Play();
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