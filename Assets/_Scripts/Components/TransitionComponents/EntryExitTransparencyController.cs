using UnityEngine;

public class EntryExitTransparencyController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IFadeable>(out IFadeable fadeableObject))
        {
            fadeableObject.FadeOut();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IFadeable>(out IFadeable fadeableObject))
        {
            fadeableObject.UnFade();
        }
    }
}
