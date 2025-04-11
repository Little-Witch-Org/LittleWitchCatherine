using UnityEngine;

public class CursorChangeComponent : MonoBehaviour
{
    /*[SerializeField] private Texture2D CursorTextureDefault;
    [SerializeField] private Texture2D CursorTextureToChange;

    private Vector2 cursorHotspot;


    private void Start()
    {
        //cursorHotspot = new Vector2(CursorTextureToChange.width / 2, CursorTextureToChange.height / 2);
        cursorHotspot = new Vector2(8, 0);
    }*/
    
    [SerializeField] private string cursorDefaultPath = "Art/Cursors/cursorDefault1";
    [SerializeField] private string cursorTransitionPath = "Art/Cursors/cursorTransition1";
    
    private Texture2D CursorTextureDefault;
    private Texture2D CursorTextureToChange;
    private Vector2 cursorHotspot;

    private void Awake()
    {
        CursorTextureDefault = UnityEngine.Resources.Load<Texture2D>("Art/Cursors/cursorDefault1");
        CursorTextureToChange = UnityEngine.Resources.Load<Texture2D>("Art/Cursors/cursorTransition1");

        Debug.Log("cursorDefault1: " + CursorTextureDefault);
        Debug.Log("cursorTransition1: " + CursorTextureToChange);

        if (CursorTextureDefault == null || CursorTextureToChange == null)
        {
            Debug.LogError("Не удалось загрузить текстуры курсоров по указанным путям!");
        }
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
