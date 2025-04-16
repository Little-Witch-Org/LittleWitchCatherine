using System;
using UnityEngine;

namespace _Scripts.Components.Transition
{
    public class ChangeSceneTriggerComponent : MonoBehaviour
    {
        [SerializeField] private SceneNames scene;

        public void ChangeSceneTrigger()
        {
            EventManager.Instance.TransitionEvents.ChangeScene(scene);
        }
    }
}