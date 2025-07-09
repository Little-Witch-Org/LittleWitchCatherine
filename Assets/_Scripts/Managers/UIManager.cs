using System.Collections.Generic;
using _Scripts.Components.Transition.UI;
using _Scripts.InventorySystem.ByGuide.Inventories;
using _Scripts.QuestSystem.UI;
using _Scripts.UI;
using UnityEngine;

//todo refactor class. extract methods from journal menu and reuse it in other toggles.. etc
namespace _Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public LoadScreenUI loadScreenUI;

        [SerializeField] private CheatMenuUI cheatMenuUI;
        [SerializeField] private PlayerStatsUI playerStatsUI;
        
        [SerializeField] private JournalButtonsUI journalButtonsUI;
        [SerializeField] private QuestLogMenuUI questLogMenuUI;
        [SerializeField] private RelationsUI relationsUI;
        
        
        [SerializeField] private PlayerInventoryUI playerInventoryUI;
        [SerializeField] private StorageInventoryUI storageInventoryUI;

        private IMenu CheatMenuUI => cheatMenuUI;
        private IMenu QuestLogMenuUI => questLogMenuUI;
        private IMenu PlayerStatsUI => playerStatsUI;
        private IMenu PlayerInventoryUI => playerInventoryUI;
        private IMenu StorageInventoryUI => storageInventoryUI;
        private IMenu RelationsUI => relationsUI;

        //public GameObject cheatMenu;
        //public GameObject questMenu;


        //private bool _isMenuOpen;
        private IMenu _currentMenu;
        private IMenu _lastJournalMenu;
        private readonly List<IMenu> _inventoryMenus = new();
        private readonly List<IMenu> _journalMenus = new();



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
            
            //add journal menus
            _journalMenus.Add(questLogMenuUI);
            _journalMenus.Add(relationsUI);
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
            if (menu == _currentMenu)
            {
                _currentMenu.HideMenu();
                _currentMenu = null;
            }
            else
            {
                _currentMenu?.HideMenu();
                _currentMenu = menu;
                menu.ShowMenu();

                journalButtonsUI.HideButtons();
                
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
            if (menu.ContentParent.activeInHierarchy) //if current inventory is open -> close
            {
                menu.HideMenu();
            }
            else
            {
                menu.ShowMenu();
                //_currentMenu = menu;

                // close system menu if opened
                if (_currentMenu != null)
                {
                    journalButtonsUI.HideButtons();
                    _currentMenu.HideMenu();
                    _currentMenu = null;
                }
            }
        }
        
        private void ToggleJournalMenu(IMenu menu) 
        {
            if (menu == _currentMenu)
            {
                _currentMenu.HideMenu();
                _currentMenu = null;
                journalButtonsUI.HideButtons();
            }
            else
            {
                _currentMenu?.HideMenu();
                _currentMenu = menu;
                menu.ShowMenu();
                
                journalButtonsUI.ShowButtons();

                // close inventories
                foreach (var inventory in _inventoryMenus)
                {
                    if (inventory.ContentParent.activeInHierarchy)
                        inventory.HideMenu();
                }
            }
        }

        public void ToggleJournalMenu() 
        {
            if (_currentMenu != null && !_journalMenus.Contains(_currentMenu)) //switch from another
            {
                _currentMenu.HideMenu();
                
                if (_lastJournalMenu != null)
                {
                    _lastJournalMenu.ShowMenu();
                    _currentMenu = _lastJournalMenu;
                }
                else
                {
                    questLogMenuUI.ShowMenu();
                    _currentMenu = questLogMenuUI;
                }
                
                _lastJournalMenu = _currentMenu;
                journalButtonsUI.ShowButtons();
                
                // close inventories
                foreach (var inventory in _inventoryMenus)
                {
                    if (inventory.ContentParent.activeInHierarchy)
                        inventory.HideMenu();
                }
            }
            else if (_currentMenu != null && _journalMenus.Contains(_currentMenu)) //close if in journal
            {
                _lastJournalMenu = _currentMenu;
                _currentMenu.HideMenu();
                _currentMenu = null;
                journalButtonsUI.HideButtons();

            }
            else //open if no menus
            {
                if (_lastJournalMenu != null)
                {
                    _lastJournalMenu.ShowMenu();
                    _currentMenu = _lastJournalMenu;
                }
                else
                {
                    questLogMenuUI.ShowMenu();
                    _currentMenu = questLogMenuUI;
                }
                _lastJournalMenu = _currentMenu;
                journalButtonsUI.ShowButtons();
                
                // close inventories
                foreach (var inventory in _inventoryMenus)
                {
                    if (inventory.ContentParent.activeInHierarchy)
                        inventory.HideMenu();
                }
                
            }
        }

        private void SwitchJournalMenu(IMenu menu)
        {
            
            if (menu != _currentMenu)
            {
                _currentMenu.HideMenu();
                _currentMenu = menu;
                menu.ShowMenu();
                _lastJournalMenu = _currentMenu;
            }
        }
        

        public void ToggleCheatMenu() => ToggleSystemMenu(CheatMenuUI);
        public void ToggleStatsMenu() => ToggleSystemMenu(PlayerStatsUI);
        
        public void ToggleQuestLog() => ToggleJournalMenu(QuestLogMenuUI);
        
        
        public void TogglePlayerInventoryMenu() => ToggleInventoryMenu(PlayerInventoryUI);
        public void ToggleStorageInventoryMenu() => ToggleInventoryMenu(StorageInventoryUI);

        public void SwitchToQuestMenu() => SwitchJournalMenu(QuestLogMenuUI);
        public void SwitchToRelationsMenu() => SwitchJournalMenu(RelationsUI);

    }
}
