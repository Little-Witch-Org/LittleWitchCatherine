using UnityEngine;

namespace _Scripts.UI
{
    public class RelationsUI : MonoBehaviour,IMenu
    {
        [SerializeField] private GameObject contentParent;
        public GameObject ContentParent => contentParent;
        
        public void ShowMenu()
        {
            contentParent.SetActive(true);
        }

        public void HideMenu()
        {
            contentParent.SetActive(false);
        }
    }
}