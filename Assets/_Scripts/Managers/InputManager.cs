using System;
using UnityEngine;

namespace _Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;


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


        private void Update()
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
            if (Input.GetMouseButtonDown(0)) //todo need to handle click on char and dialogue continue
            //if (Input.GetKeyDown(KeyCode.Space))
            {
                EventManager.Instance.InputEvents.SubmitPressed();
            }
        }
    }
}