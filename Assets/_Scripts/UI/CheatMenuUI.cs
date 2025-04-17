using UnityEngine;

namespace _Scripts.UI
{
    public class CheatMenuUI : MonoBehaviour,IMenu
    {
        [SerializeField] private GameObject contentParent;
        public GameObject ContentParent => contentParent;

        
        public void HideMenu()
        {
            contentParent.SetActive(false);
        }


        public void ShowMenu()
        {
            contentParent.SetActive(true);
        }
    }
}