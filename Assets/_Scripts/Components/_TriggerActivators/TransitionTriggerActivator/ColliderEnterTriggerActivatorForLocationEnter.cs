using System;
using _Scripts.Enums;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Sequence = DG.Tweening.Sequence;

//todo - separate detect and animation logic
namespace _Scripts.Components._TriggerActivators.TransitionTriggerActivator
{
    public class ColliderEnterTriggerActivatorForLocationEnter : MonoBehaviour
    {
        [SerializeField] private TagsNames tagName;

        private int defaultSorting;
        public UnityEvent enterTriggerEvent;
        
        private bool _isPlayerInTrigger = false;
        private Sequence _sequence;
        private Tween _tween;
        [SerializeField] private SpriteRenderer spriteRenderer1;
        [SerializeField] private SpriteRenderer spriteRenderer2;
        [SerializeField] private SpriteRenderer interactionHintRenderer;

        private void Start()
        {
            defaultSorting = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            spriteRenderer1.sortingOrder = defaultSorting+2;
            spriteRenderer2.sortingOrder = defaultSorting+1;
        }

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
            if (collision.transform.parent.CompareTag(tagName.ToString()))
            {
                gameObject.GetComponent<SpriteRenderer>().sortingOrder =
                    collision.transform.parent.GetComponent<SpriteRenderer>().sortingOrder - 5;
                
                spriteRenderer1.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder+2;
                spriteRenderer2.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder+1;
                
                ShowHint();
                _isPlayerInTrigger = true;
                HideDot();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.parent.CompareTag(tagName.ToString()))
            {
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = defaultSorting;
                spriteRenderer1.sortingOrder = defaultSorting+2;
                spriteRenderer2.sortingOrder = defaultSorting+1;
                
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
