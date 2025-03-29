using UnityEngine;
using UnityEngine.Events;

public class ColiderEnterTrigger : MonoBehaviour
{
    [SerializeField] private TagsNames TagName;

    public UnityEvent EnterTriggerEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagName.ToString()))
            EnterTriggerEvent.Invoke();
    }
}
