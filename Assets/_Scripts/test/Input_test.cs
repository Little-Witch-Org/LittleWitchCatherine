using System;
using UnityEngine;

namespace _Scripts
{
    public class Input_test : MonoBehaviour
    {
        
        public static Input_test instance;
        

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Submit pressed - 'Space'");
                GameEventsManager_Test.Instance.InputEventsTest.SubmitPressed();
            }
        }
    }
}