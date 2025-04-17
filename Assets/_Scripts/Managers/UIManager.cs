using System;
using _Scripts.Components.Transition.UI;
using _Scripts.UI;
using UnityEngine;
using _Scripts.QuestSystem.UI;
using UnityEngine.Serialization;

namespace _Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public LoadScreenUI loadScreenUI;

        [SerializeField] private CheatMenuUI cheatMenuUI;
        [SerializeField] private QuestLogMenuUI questLogMenuUI;

        private IMenu CheatMenuUI => cheatMenuUI;
        private IMenu QuestLogMenuUI => questLogMenuUI;

        //public GameObject cheatMenu;
        //public GameObject questMenu;


        //private bool _isMenuOpen;
        private IMenu _currentOpenedMenu;



        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.InputEvents.OnJournalPressed += ToggleQuestLog;
            EventManager.Instance.InputEvents.OnMenuPressed += ToggleCheatMenu;
        }

        private void OnDisable()
        {
            EventManager.Instance.InputEvents.OnJournalPressed -= ToggleQuestLog;
            EventManager.Instance.InputEvents.OnMenuPressed -= ToggleCheatMenu;
        }
        
        private void CloseCurrentMenu()
        {
            if (_currentOpenedMenu != null)
            {
                _currentOpenedMenu.HideMenu();
                _currentOpenedMenu = null;
            }
        }
        
        public void ToggleMenu(IMenu menu)
        {
            if (menu.ContentParent.activeInHierarchy)
            {
                CloseCurrentMenu();
            }
            else
            {
                CloseCurrentMenu();
                _currentOpenedMenu = menu;
                menu.ShowMenu();
            }
        }


        public void ToggleCheatMenu() => ToggleMenu(CheatMenuUI);
        public void ToggleQuestLog() => ToggleMenu(QuestLogMenuUI);
        
        


    }
}
