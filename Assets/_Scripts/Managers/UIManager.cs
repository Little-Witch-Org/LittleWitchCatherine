using System;
using System.Collections.Generic;
using _Scripts.Components.Transition.UI;
using _Scripts.InventorySystem.ByGuide;
using _Scripts.InventorySystem.ByGuide.Inventories;
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
        [SerializeField] private PlayerStatsUI playerStatsUI;
        [SerializeField] private PlayerInventoryUI playerInventoryUI;
        [SerializeField] private StorageInventoryUI storageInventoryUI;

        private IMenu CheatMenuUI => cheatMenuUI;
        private IMenu QuestLogMenuUI => questLogMenuUI;
        private IMenu PlayerStatsUI => playerStatsUI;
        private IMenu PlayerInventoryUI => playerInventoryUI;
        private IMenu StorageInventoryUI => storageInventoryUI;

        //public GameObject cheatMenu;
        //public GameObject questMenu;


        //private bool _isMenuOpen;
        private IMenu _currentSystemMenu;
        private readonly List<IMenu> _inventoryMenus = new();



        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            //register inventories
            _inventoryMenus.Add(PlayerInventoryUI);
            _inventoryMenus.Add(StorageInventoryUI);
        }

        private void OnEnable()
        {
            EventManager.Instance.InputEvents.OnJournalPressed += ToggleQuestLog;
            EventManager.Instance.InputEvents.OnMenuPressed += ToggleCheatMenu;
            EventManager.Instance.InputEvents.OnStatsPressed += ToggleStatsMenu;
            EventManager.Instance.InputEvents.OnPlayerInventoryPressed += TogglePlayerInventoryMenu;
            EventManager.Instance.InputEvents.OnStorageInventoryPressed += ToggleStorageInventoryMenu;
        }

        private void OnDisable()
        {
            EventManager.Instance.InputEvents.OnJournalPressed -= ToggleQuestLog;
            EventManager.Instance.InputEvents.OnMenuPressed -= ToggleCheatMenu;
            EventManager.Instance.InputEvents.OnStatsPressed -= ToggleStatsMenu;
            EventManager.Instance.InputEvents.OnPlayerInventoryPressed -= TogglePlayerInventoryMenu;
            EventManager.Instance.InputEvents.OnStorageInventoryPressed += ToggleStorageInventoryMenu;
        }
        
        private void ToggleSystemMenu(IMenu menu)
        {
            if (menu == _currentSystemMenu)
            {
                _currentSystemMenu.HideMenu();
                _currentSystemMenu = null;
            }
            else
            {
                _currentSystemMenu?.HideMenu();
                _currentSystemMenu = menu;
                menu.ShowMenu();

                // close inventories
                foreach (var inventory in _inventoryMenus)
                {
                    if (inventory.ContentParent.activeInHierarchy)
                        inventory.HideMenu();
                }
            }
        }
        
        private void ToggleInventoryMenu(IMenu menu)
        {
            if (menu.ContentParent.activeInHierarchy)
            {
                menu.HideMenu();
            }
            else
            {
                menu.ShowMenu();

                // close system menu if opened
                if (_currentSystemMenu != null)
                {
                    _currentSystemMenu.HideMenu();
                    _currentSystemMenu = null;
                }
            }
        }



        public void ToggleCheatMenu() => ToggleSystemMenu(CheatMenuUI);
        public void ToggleQuestLog() => ToggleSystemMenu(QuestLogMenuUI);
        public void ToggleStatsMenu() => ToggleSystemMenu(PlayerStatsUI);
        public void TogglePlayerInventoryMenu() => ToggleInventoryMenu(PlayerInventoryUI);
        public void ToggleStorageInventoryMenu() => ToggleInventoryMenu(StorageInventoryUI);
        
        


    }
}
