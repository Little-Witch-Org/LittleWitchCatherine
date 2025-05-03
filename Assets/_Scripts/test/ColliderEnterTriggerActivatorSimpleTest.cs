using System;
using _Scripts.Enums;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

namespace _Scripts.Components._UEventsTriggers
{
    public class ColliderEnterTriggerActivatorSimpleTest : MonoBehaviour
    {
        [SerializeField] private TagsNames tagName;

        public UnityEvent enterTriggerEvent;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(tagName.ToString()))
            {
                enterTriggerEvent.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(tagName.ToString()))
            {
                
            }
        }
    }
}
