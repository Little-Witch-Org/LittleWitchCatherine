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
                //Debug.Log("Submit pressed - 'Space'");
                GameEventsManager_Test.Instance.InputEventsTest.SubmitPressed();
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Debug.Log("Esc pressed - 'Escape'");
                GameEventsManager_Test.Instance.InputEventsTest.OnMenuPressed();
            }
        }
    }
}