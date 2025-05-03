using _Scripts.Managers;
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

        public void SetTimeToMorning()
        {
            TimeManager.Instance.SetInitialTime(10,10,6,0,0);
        }
        public void SetTimeToAfternoon()
        {
            TimeManager.Instance.SetInitialTime(10,10,12,0,0);
        }
        public void SetTimeToEvening()
        {
            TimeManager.Instance.SetInitialTime(10,10,18,0,0);
        }
        public void SetTimeToNight()
        {
            TimeManager.Instance.SetInitialTime(10,10,0,0,0);
        }
    }
}