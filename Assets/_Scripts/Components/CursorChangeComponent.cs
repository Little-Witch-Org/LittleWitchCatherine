using UnityEngine;

public class CursorChangeComponent : MonoBehaviour
{
    [SerializeField] private Texture2D CursorTextureDefault;
    [SerializeField] private Texture2D CursorTextureToChange;

    private Vector2 cursorHotspot;


    private void Start()
    {
        //cursorHotspot = new Vector2(CursorTextureToChange.width / 2, CursorTextureToChange.height / 2);
        cursorHotspot = new Vector2(8, 0);
    }

    public void ChangeCursorTexture() 
    {
        Cursor.SetCursor(CursorTextureToChange, cursorHotspot, CursorMode.Auto);
    }
    public void ToDefaultCursorTexture()
    {
        Cursor.SetCursor(CursorTextureDefault, cursorHotspot, CursorMode.Auto);
    }
}
