using UnityEngine;

public class DestroyComponentManager : MonoBehaviour
{
    private DestroyComponent destroyComponent;
    public void FindAndDestroy()
    {
        destroyComponent = FindFirstObjectByType<DestroyComponent>();
        destroyComponent.Destroy();
    }
}
