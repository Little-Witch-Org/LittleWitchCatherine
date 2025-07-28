using System;
using _Scripts.Enums;
using _Scripts.Managers;
using _Scripts.QuestSystem;
using UnityEngine;

namespace _Scripts.test
{
    public class ChangeSquereColorForTestQuest : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;


        private void OnEnable()
        {
            EventManager.Instance.QuestEvents.OnQuestStateChanged += ChangedColor;
        }
        private void OnDisable()
        {
            EventManager.Instance.QuestEvents.OnQuestStateChanged -= ChangedColor;
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void ChangedColor(Quest quest)
        {
            if (quest.InfoSo.Id.Contains("Second") && quest.StateEnum == QuestStateEnum.CanStart)
            {
                _spriteRenderer.color = Color.green;
            }
        }
    }
    
    
}