using UnityEngine;

namespace _Scripts.UI
{
    public interface IMenu
    {
        GameObject ContentParent { get; }
        void ShowMenu();
        void HideMenu();
    }
}