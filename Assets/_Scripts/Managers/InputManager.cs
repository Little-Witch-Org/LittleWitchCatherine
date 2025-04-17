using System;
using UnityEngine;

namespace _Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        
        private bool _isHotkeysActive = true;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.InputEvents.OnHotkeysAreActive += ToggleHotkeysAre;
        }
        private void OnDisable()
        {
            EventManager.Instance.InputEvents.OnHotkeysAreActive -= ToggleHotkeysAre;
        }

        private void ToggleHotkeysAre(bool state)
        {
            _isHotkeysActive = state;
        }


        private void Update()
        {
            if (_isHotkeysActive)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EventManager.Instance.InputEvents.MenuPressed();
                }

                if (Input.GetKeyDown(KeyCode.J))
                {
                    EventManager.Instance.InputEvents.JournalPressed();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    EventManager.Instance.InputEvents.InteractPressed();
                }
            }

            if (Input.GetMouseButtonDown(0))
            //if (Input.GetKeyDown(KeyCode.Space))
            {
                EventManager.Instance.InputEvents.SubmitPressed();
            }
        }
    }
}