using System;
using UnityEngine;

namespace _Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        
        private bool _isHotkeysActive = true;
        
        private bool _isSubmitActive = true;
        
        private bool _isSpaceActive = true;
        
        private bool _isInputActive = true;


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
            EventManager.Instance.InputEvents.OnHotkeysActiveChanged += SetHotkeysActive;
            EventManager.Instance.InputEvents.OnSubmitActiveChange += SetSubmitActive;
            EventManager.Instance.InputEvents.OnSpaceActiveChanged += SetSpaceActive;
            EventManager.Instance.InputEvents.OnInputActiveChanged += SetInputActive;
        }
        private void OnDisable()
        {
            EventManager.Instance.InputEvents.OnHotkeysActiveChanged -= SetHotkeysActive;
            EventManager.Instance.InputEvents.OnSubmitActiveChange -= SetSubmitActive;
            EventManager.Instance.InputEvents.OnSpaceActiveChanged -= SetSpaceActive;
            EventManager.Instance.InputEvents.OnInputActiveChanged -= SetInputActive;
        }

        private void SetHotkeysActive(bool state)
        {
            _isHotkeysActive = state;
        }

        private void SetSubmitActive(bool state)
        {
            _isSubmitActive = state;
        }
        
        private void SetSpaceActive(bool state)
        {
            _isSpaceActive = state;
        }
        private void SetInputActive(bool state)
        {
            _isInputActive = state;
        }


        private void Update()
        {
            if (_isInputActive)
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

                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        EventManager.Instance.InputEvents.StatsPressed();
                    }

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        EventManager.Instance.InputEvents.InteractPressed();
                    }

                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        EventManager.Instance.InputEvents.PlayerInventoryPressed();
                    }
                }

                if (Input.GetMouseButtonDown(0)) //lmb
                {
                    if (_isSubmitActive)
                    {
                        EventManager.Instance.InputEvents.SubmitPressed();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_isSpaceActive)
                    {
                        EventManager.Instance.InputEvents.SpacePressed();
                    }
                }
            }
        }
    }
}