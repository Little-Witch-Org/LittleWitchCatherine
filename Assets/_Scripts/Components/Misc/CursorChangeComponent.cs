using System;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Components.Misc
{
    public class CursorChangeComponent : MonoBehaviour
    {

    
        [SerializeField] private string cursorDefaultPath = "Art/Cursors/cursorDefault1";
        [SerializeField] private string cursorHoverOnTriggerPath = "Art/Cursors/cursorTransition1";
    
        private Texture2D _cursorTextureDefault;
        private Texture2D _cursorTextureHoverOnTrigger;
        private Vector2 _cursorHotspot;

        private void Awake()
        {
            _cursorTextureDefault = UnityEngine.Resources.Load<Texture2D>(cursorDefaultPath);
            _cursorTextureHoverOnTrigger = UnityEngine.Resources.Load<Texture2D>(cursorHoverOnTriggerPath);

            //Debug.Log("cursorDefault1: " + CursorTextureDefault);
            //Debug.Log("cursorTransition1: " + CursorTextureToChange);

            if (_cursorTextureDefault == null || _cursorTextureHoverOnTrigger == null)
            {
                Debug.LogError("Не удалось загрузить текстуры курсоров по указанным путям!");
            }

        }

        private void OnEnable()
        {
            
            EventManager.Instance.MiscEvents.OnCursorChangeToDefault += ToDefaultCursorTexture;
            EventManager.Instance.MiscEvents.OnCursorChangeToHoverOnTrigger += ToHoverOnTriggerCursorTexture;
        }

        private void OnDisable()
        {
            EventManager.Instance.MiscEvents.OnCursorChangeToDefault -= ToDefaultCursorTexture;
            EventManager.Instance.MiscEvents.OnCursorChangeToHoverOnTrigger -= ToHoverOnTriggerCursorTexture;
        }

        public void ToDefaultCursorTexture()
        {
            Cursor.SetCursor(_cursorTextureDefault, _cursorHotspot, CursorMode.Auto);
        }

        public void ToHoverOnTriggerCursorTexture() 
        {
            Cursor.SetCursor(_cursorTextureHoverOnTrigger, _cursorHotspot, CursorMode.Auto);
        }
        
    }
}
