using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

//todo - separate detect and animation logic
namespace _Scripts.Components._UEventsTriggers
{
    public class ColliderEnterTriggerActivatorForLocationEnter : MonoBehaviour
    {
        [SerializeField] private TagsNames tagName;

        public UnityEvent enterTriggerEvent;

        private bool _isPlayerInTrigger = false;
        private Sequence _sequence;
        private Tween _tween;
        [SerializeField] private SpriteRenderer spriteRenderer1;
        [SerializeField] private SpriteRenderer spriteRenderer2;
        [SerializeField] private SpriteRenderer interactionHintRenderer;


        private void OnEnable()
        {
            EventManager.Instance.InputEvents.OnInteractPressed += TriggerOnButtonPress;
        }

        private void OnDisable()
        {
            EventManager.Instance.InputEvents.OnInteractPressed -= TriggerOnButtonPress;
        }
        

        private void OnDestroy()
        {

            KillCurrentSequenceIfActive();
            KillCurrentAnimationIfActive();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(tagName.ToString()))
            {
                ShowHint();
                _isPlayerInTrigger = true;
                HideDot();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(tagName.ToString()))
            {
                HideHint();
                _isPlayerInTrigger = false;
                ShowDot();
            }
        }

        private void TriggerOnButtonPress()
        {
            if (_isPlayerInTrigger)
            {
                enterTriggerEvent.Invoke();
            }

        }


        //add dotween auto fade animation 
        private void ShowDot()
        {
            KillCurrentSequenceIfActive();

            _sequence = DOTween.Sequence();
            _sequence.Append(spriteRenderer1.DOFade(1f, 0.5f))
                .Join(spriteRenderer2.DOFade(1f, 0.5f)).Play();
        }

        private void HideDot()
        {
            KillCurrentSequenceIfActive();
            _sequence = DOTween.Sequence();
            _sequence.Append(spriteRenderer1.DOFade(0f, 0.5f))
                .Join(spriteRenderer2.DOFade(0f, 0.5f)).Play();
        }

        private bool InSequence() => _sequence != null && _sequence.IsActive();

        private void KillCurrentSequenceIfActive()
        {
            if (InSequence())
            {
                _sequence.Kill();
            }
        }


        private bool InAnimation() => _tween != null && _tween.IsActive();

        private void KillCurrentAnimationIfActive()
        {
            if (InAnimation())
            {
                _tween.Kill();
            }
        }

        private void ShowHint()
        {
            KillCurrentAnimationIfActive();

            _tween = interactionHintRenderer.DOFade(1f, 0.3f).Play();
        }

        private void HideHint()
        {
            KillCurrentAnimationIfActive();

            _tween = interactionHintRenderer.DOFade(0f, 0.3f).Play();


        }
    }
}
