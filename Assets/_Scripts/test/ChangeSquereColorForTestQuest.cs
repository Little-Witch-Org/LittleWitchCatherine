using System;
using _Scripts.QuestSystem;
using UnityEngine;

namespace _Scripts.test
{
    public class ChangeSquereColorForTestQuest : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;


        private void OnEnable()
        {
            EventManager.Instance.QuestEvents.OnQuestStateChange += ChangeColor;
        }
        private void OnDisable()
        {
            EventManager.Instance.QuestEvents.OnQuestStateChange -= ChangeColor;
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void ChangeColor(Quest quest)
        {
            if (quest.InfoSo.Id.Contains("Second") && quest.StateEnum == QuestStateEnum.CanStart)
            {
                _spriteRenderer.color = Color.green;
            }
        }
    }
    
    
}