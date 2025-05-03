using UnityEngine;

namespace _Scripts.Components.SpawnComponents
{
    public class SpawnComponent : MonoBehaviour
    {
        public void Spawn(GameObject gameobject, Vector2 position)
        {
            Instantiate(gameobject, position, Quaternion.identity);
        }
    }
}
