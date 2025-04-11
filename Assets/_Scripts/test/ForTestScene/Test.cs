using System;
using UnityEngine;

namespace _Scripts
{
    public class Test: MonoBehaviour
    {
        public void print()
        {
            print("Method Invoked");
            Destroy(gameObject);
        }

    }
}