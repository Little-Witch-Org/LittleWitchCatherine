using System;
using UnityEngine;

namespace _Scripts
{
    public class Test: MonoBehaviour
    {
        private void Start()
        {
            Debug.Log(PlayerPrefs.GetString("LocationName"));
            PlayerPrefs.DeleteAll();
            Debug.Log(PlayerPrefs.GetString("LocationName"));

        }

    }
}