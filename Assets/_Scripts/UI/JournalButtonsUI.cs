using _Scripts.QuestSystem.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.UI
{
    public class JournalButtonsUI:MonoBehaviour
    {
        [SerializeField] private GameObject buttonsContainer;




        public void ShowButtons()
        {
            buttonsContainer.SetActive(true);
        }

        public void HideButtons()
        {
            buttonsContainer.SetActive(false);
        }
    }
}