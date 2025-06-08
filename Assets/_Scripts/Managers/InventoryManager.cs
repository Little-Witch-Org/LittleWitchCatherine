using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.InventorySystem.ByGuide;
using _Scripts.InventorySystem.ByGuide.Interfaces;
using _Scripts.InventorySystem.ByGuide.Inventories;
using _Scripts.InventorySystem.ByGuide.Scriptable;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Scripts.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        [SerializeField] private ItemGrid selectedItemGrid;

        public ItemGrid SelectedItemGrid
        {
            get => selectedItemGrid;
            set
            {
                selectedItemGrid = value;
                _itemHighlight.SetParent(value);
            }
        }

        [Header("Dynamic fields")] [SerializeField]
        private InventoryItem selectedItem;

        [SerializeField] private RectTransform selectedItemRectTransform;
        [SerializeField] private InventoryItem overlapItem;
        [SerializeField] private InventoryItem itemToHighlight;
        [SerializeField] private InventoryItem itemToOnHover;

        [Header("ItemSo List")] [SerializeField]
        private List<ItemDataSo> itemsSo;

        [Header("References")] [SerializeField]
        private GameObject itemPrefab;

        [SerializeField] private Transform itemDraggingContainerTransform; //for drag items if inventory turned off
        [SerializeField] private TooltipController itemTooltipController;
        [SerializeField] private ContextMenuController contextMenuController;

        [SerializeField] PlayerInventoryUI playerInventoryUI;
        [SerializeField] StorageInventoryUI storageInventoryUI;

        private ItemGrid _playerMainGrid;
        private ItemGrid _playerMagicGrid;
        private ItemGrid _playerPotionGrid;
        private ItemGrid _playerStorageGrid;

        private GameObject _playerInventoryContentParent;
        private GameObject _storageInventoryContentParent;
        
        private Transform _playerInventoryItemBufferTransform; //for items to add in background


        private ItemHighlight _itemHighlight;

        private bool _isInventoryWindowDragging; //check if we drag window - don't interact with grids

        private bool _isContextMenuShown;

        private List<InventoryItem> _inventoryItemsList; //all items in inventories
        private List<ItemGrid> _itemGridsList; //all grids
        
        private bool _submitPressed;
        
        private Vector2Int? _selectedItemPreviousPosition; //used to return item if inventory was closed (todo to check with rotated item)
        [CanBeNull] private ItemGrid _selectedPreviousItemGrid;
        
        private void Awake()
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
            EventManager.Instance.InputEvents.OnSubmitPressed += OnSubmitPressed;
            EventManager.Instance.TransitionEvents.OnChangeScene += OnTransition;
            EventManager.Instance.TransitionEvents.OnPlaceTransitionTrigger += OnTransition;
            EventManager.Instance.InventoryEvents.OnContextMeniOpenStatusSet += SetContextMenuStatus;
        }

        private void OnDisable()
        {
            EventManager.Instance.InputEvents.OnSubmitPressed -= OnSubmitPressed;
            EventManager.Instance.TransitionEvents.OnChangeScene -= OnTransition;
            EventManager.Instance.TransitionEvents.OnPlaceTransitionTrigger -= OnTransition;
            EventManager.Instance.InventoryEvents.OnContextMeniOpenStatusSet += SetContextMenuStatus;
        }

        private void Start()
        {
            Initialize();
            SpawnAllItems();
        }

        private void Initialize()
        {
            _itemHighlight = GetComponent<ItemHighlight>();

            _playerMainGrid = playerInventoryUI.GetPlayerMainItemGrid();
            _playerMagicGrid = playerInventoryUI.GetPlayerMagicItemGrid();
            _playerPotionGrid = playerInventoryUI.getPlayerPotionGrid();
            _playerStorageGrid = storageInventoryUI.GetStorageItemGrid();

            _itemGridsList = new List<ItemGrid>
            {
                _playerMainGrid,
                _playerMagicGrid,
                _playerPotionGrid,
                _playerStorageGrid
            };
            

            _inventoryItemsList = new List<InventoryItem>();

            _playerInventoryContentParent = playerInventoryUI.GetContentParent();
            _playerInventoryItemBufferTransform = playerInventoryUI.GetItemBufferContainerTransform();
            
            _storageInventoryContentParent = storageInventoryUI.GetContentParent();

            //grids must be initialized (activate Start() in ItemGrid class) for correct work 
            InitializeAllInventories();
        }

        private void SpawnAllItems()
        {
            foreach (var itemDataSo in itemsSo)
            {
                AddItemInBackground(itemDataSo.itemId,"main");
            }
            
            //AddItemInBackground("inventory_item_id_1_1", "main");
            //AddItemInBackground("inventory_item_id_1_2", "main");
            //AddItemInBackground("inventory_item_id_1_3", "main");
            //AddItemInBackground("inventory_item_id_2_1", "main");
        }

        private void SpawnRandomItem()
        {
            AddItemInBackground(itemsSo[Random.Range(0, itemsSo.Count)].itemId, "main");
        }

        private void InitializeAllInventories()
        {
            List<GameObject> contentParentGameObjects = new List<GameObject>();
            contentParentGameObjects.Add(_playerInventoryContentParent);
            contentParentGameObjects.Add(_storageInventoryContentParent);

            foreach (var contentParentObjects in contentParentGameObjects)
            {
                StartCoroutine(DelayedGridsInitialization(contentParentObjects));
            }
        }

        private IEnumerator DelayedGridsInitialization(GameObject cpGo)
        {
            cpGo.gameObject.SetActive(true);
            yield return null;
            cpGo.gameObject.SetActive(false);
            //Debug.Log("Initialized " + cpGo.gameObject.name);
        }


        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                SpawnRandomItem();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                SpawnAllItems();
            }

            if (selectedItem != null)
            {
                ItemIconDrag(); //can drag if item selected and inventory closed (but cant place it) //todo mb if inventory closed -> return item ? 
                SetTransitionAvailability(true);

            }
            else
            {
                SetTransitionAvailability(false);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                RotateItem();
                RmbForMenuOpen();
            }
            
            

            //if all closed
            if (!_playerInventoryContentParent.gameObject
                    .activeInHierarchy && !_storageInventoryContentParent.gameObject.activeInHierarchy)
            {
                {
                    //Debug.Log("Inventory closed - reset state");
                    ClearHoverState(); //trigger onHoverExit + hide tooltip
                    selectedItemGrid = null; 
                    ReturnItemIfInventoryCloses();
                }
                return;
            }

            if (_isInventoryWindowDragging)
            {
                return;
            }
            

            /*if (selectedItemGrid == null)
            {
                _itemHighlight.DisplayHighlight(false);
                return;
            }*/

            if (selectedItemGrid == null)
            {
                _itemHighlight.DisplayHighlight(false);
                ClearHoverState();
                return;
            }

            UpdateHoverItem();
            HandleHighlight();

            /*if (_submitPressed)
            {
                _submitPressed = false;

                ClickOnItem();

            }*/

            SendSelectedItemStatusEvent();
        }

        private void SetTransitionAvailability(bool isOn)
        {
            EventManager.Instance.TransitionEvents.SetTransitionDisabled(isOn);
        }


        public void AddItemInBackground(string itemNameOrID, string itemGridName) //todo to handle finding by name (when structure will be added)
        {
            // find item
            ItemDataSo itemData = itemsSo.Find(x => x.itemId == itemNameOrID);

            if (itemData == null)
            {
                Debug.Log($"Item \"{itemNameOrID}\" not found");
                return;
            }

            //find grid by name (ignore register)
            ItemGrid gridToAdd = _itemGridsList.Find(x =>
                x.gameObject.name.Contains(itemGridName, StringComparison.OrdinalIgnoreCase)
            );

            if (gridToAdd == null)
            {
                Debug.Log($"Grid with string \"{itemGridName}\" not found");
                return;
            }

            //create item in background (add to buffer)
            InventoryItem newItem = CreateItemBackground(itemData);

            //add item in list
            _inventoryItemsList.Add(newItem);

            //find place in inventory
            Vector2Int? position = gridToAdd.FindSpaceForItem(newItem);
            if (position != null)
            {
                InventoryItem overlapItemTemp = null;
                if (gridToAdd.PlaceItem(newItem, position.Value.x, position.Value.y, ref overlapItemTemp))
                {
                    newItem.gameObject.SetActive(true);
                    newItem.transform.SetParent(gridToAdd.transform);
                }
            }
            else
            {
                Debug.Log("No space in inventory");
                // Можно добавить логику для обработки переполненного инвентаря
            }
        }


        private InventoryItem CreateItemBackground(ItemDataSo itemData)
        {
            GameObject itemObj = Instantiate(itemPrefab, _playerInventoryItemBufferTransform);
            //itemObj.SetActive(false); // create inactive

            InventoryItem item = itemObj.GetComponent<InventoryItem>();
            item.Set(itemData);

            return item;
        }


        private void HandleHighlight()
        {
            if (selectedItemGrid == null)
            {
                _itemHighlight.DisplayHighlight(false);
                return;
            }



            Vector2Int gridPos = GetTileGridPosition();

            if (selectedItem == null)
            {
                InventoryItem item = selectedItemGrid.GetItem(gridPos.x, gridPos.y);
                _itemHighlight.DisplayHighlight(item != null);
                if (item != null)
                {
                    _itemHighlight.UpdateHighlight(item, true);
                    _itemHighlight.SetPosition(selectedItemGrid, item, item.onGridPositionX, item.onGridPositionY);
                }
            }
            else
            {
                //handle highlight out of grid bounds
                if (!selectedItemGrid.BoundaryCheck(gridPos.x, gridPos.y,
                        selectedItem.Width, selectedItem.Height))
                {
                    _itemHighlight.DisplayHighlight(false);
                    return;
                }

                bool canPlace = selectedItemGrid.BoundaryCheck(gridPos.x, gridPos.y,
                                    selectedItem.Width, selectedItem.Height) &&
                                selectedItemGrid.CheckAvailableSpace(gridPos.x, gridPos.y, selectedItem);
                
                _itemHighlight.UpdateHighlight(selectedItem, canPlace);
                _itemHighlight.SetPosition(selectedItemGrid, selectedItem, gridPos.x, gridPos.y);
                _itemHighlight.DisplayHighlight(true);
            }
        }


        private void UpdateHoverItem()
        {
            if (selectedItemGrid == null)
            {
                ClearHoverState();
                return;
            }

            Vector2Int positionOnGrid = GetTileGridPosition();
            var hoveredItem = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (hoveredItem == itemToOnHover) return;

            // Выход из прошлого hover
            if (itemToOnHover != null)
            {
                OnHoverExit(itemToOnHover);
            }

            itemToOnHover = hoveredItem;

            // Вход в новый hover
            if (itemToOnHover != null)
            {
                OnHoverEnter(itemToOnHover);
            }
        }

        private void ClearHoverState()
        {
            //Debug.Log("Clear hover state");
            if (itemToOnHover != null)
            {
                OnHoverExit(itemToOnHover);
                itemToOnHover = null;
            }
        }


        private void OnHoverEnter(InventoryItem itemOnHover)
        {
            //Debug.Log($"Hover enter: {itemOnHover.itemData.GetName()}");

            //PrintShapeMask(itemOnHover);
            //PrintGrid();
            //PrintCellsOccupied(itemOnHover);
            
            //show tooltip if hovering cursor on item and no selected item
            if (selectedItem == null)
            {
                ShowTooltip(itemOnHover);
            }
            
        }

        private void OnHoverExit(InventoryItem item)
        {
            //Debug.Log($"Hover exit: {item?.itemData.GetName()}");
            
            HideTooltip(); 
            
        }
        
        public void PrintCellsOccupied(InventoryItem item)
        {
        
            string output = "Occupied\n";
          
            Debug.Log("Rotated "+ item.currentRotation);
            Debug.Log("Height "+ item.Height);
            Debug.Log("Width "+ item.Width);
            
            for (int y = 0; y < item.Height; y++)
            {
                for (int x = 0; x < item.Width; x++)
                {
                    //Debug.Log($"Cell x{x} y{y} ="+ item.IsCellOccupiedOnMask(x,y));
                    output += item.IsCellOccupied(x, y) ? "■ " : "□ ";
                }
                output += "\n";
            }

            Debug.Log(output);
        }
        
        public void PrintShapeMask(InventoryItem item) //print using original form and rotation
        {
            string output = "Shape Mask (" + item.itemData.width + "x" + item.itemData.height + "):\n";
    
            for (int y = 0; y < item.itemData.height; y++)
            {
                for (int x = 0; x < item.itemData.width; x++)
                {
                    output += item.itemData.ShapeMask[x, y] ? "■ " : "□ ";
                }
                output += "\n";
            }
    
            Debug.Log(output);
        }
        
        public void PrintGrid()
        {
            string output = "Shape Mask (" + selectedItemGrid.GetGridSizeWidth() + "x" + selectedItemGrid.GetGridSizeHeight() + "):\n";
    
            for (int y = 0; y < selectedItemGrid.GetGridSizeHeight(); y++)
            {
                for (int x = 0; x < selectedItemGrid.GetGridSizeWidth(); x++)
                {
                    output += selectedItemGrid.GetItem(x,y) ? "■ " : "□ ";
                }
                output += "\n";
            }
    
            Debug.Log(output);
        }
        

        private void ReturnItemIfInventoryCloses()
        {
            if (selectedItem != null)
            {
                if (_selectedPreviousItemGrid == null)
                {
                    Debug.LogWarning("No previous grid was saved");
                    return;
                }

                if (_selectedItemPreviousPosition == null)
                {
                    Debug.LogWarning("No previous item position was saved");
                    return;
                }
                
                
                bool complete = _selectedPreviousItemGrid.PlaceItem(selectedItem, _selectedItemPreviousPosition.Value.x, _selectedItemPreviousPosition.Value.y,
                    ref overlapItem);
                if (complete)
                {
                    selectedItem = null;
                    _selectedItemPreviousPosition = null;
                }
            }
        }


        private void ClickOnItem()
        {
            var tileGridPosition = GetTileGridPosition();
            
            
            //pick
            if (selectedItem == null)
            {
                selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
                if (selectedItem != null)
                {
                    _selectedPreviousItemGrid = selectedItemGrid;
                    _selectedItemPreviousPosition = new Vector2Int(selectedItem.onGridPositionX, selectedItem.onGridPositionY); //save prev position to return if inv. closes
                    selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
                    selectedItemRectTransform.SetParent(itemDraggingContainerTransform);
                }

            }

            //place
            else
            {
                //if selectet grid no potion belt - return
                bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y,
                    ref overlapItem);
                if (complete)
                {
                    selectedItem = null;
                    _selectedItemPreviousPosition = null;
                    if (overlapItem !=
                        null) //if we have item (>0 && <2) under picked - place current item and pick overlapped 
                    {
                        selectedItem = overlapItem;
                        overlapItem = null;

                        _selectedPreviousItemGrid = selectedItemGrid;
                        _selectedItemPreviousPosition = new Vector2Int(selectedItem.onGridPositionX, selectedItem.onGridPositionY); //save prev position to return if inv. closes
                        selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
                        selectedItemRectTransform.SetParent(
                            itemDraggingContainerTransform); // if item overlapped -> we place old item, pick new and add it to content parent (above grids)
                    }
                }
            }
        }

        private Vector2Int GetTileGridPosition()
        {
            Vector2 position = Input.mousePosition;

            if (selectedItem != null)
            {
                position.x -= (selectedItem.Width - 1) * ItemGrid.TileSizeWidth / 2;
                position.y += (selectedItem.Height - 1) * ItemGrid.TileSizeHeight / 2;
            }

            return selectedItemGrid.GetTileGridPosition(position);
        }

        private void ItemIconDrag()
        {
            if (selectedItem != null)
            {
                selectedItemRectTransform.position = Input.mousePosition;
            }
        }


        private void RotateItem()
        {
            if (selectedItem == null)
            {
                return;
            }

            selectedItem.Rotate();
        }

        public void SetWindowDraggingState(bool isDragging, GameObject currentDraggingWindow)
        {
            if (currentDraggingWindow.GetComponent<PlayerInventoryUI>() != null)
            {
                playerInventoryUI.gameObject.transform.SetAsLastSibling();
            }

            if (currentDraggingWindow.GetComponent<StorageInventoryUI>() != null)
            {
                storageInventoryUI.gameObject.transform.SetAsLastSibling();
            }
            _isInventoryWindowDragging = isDragging;

        }

        public bool GetIsInventoryWindowDragging()
        {
            return _isInventoryWindowDragging;
        }


        /*public bool GetIsInventoryWindowDragging()
        {
            return _isInventoryWindowDragging;
        }*/

        private bool _lastSelectedItemStatus;

        private void SendSelectedItemStatusEvent()
        {
            bool hasSelectedItem = selectedItem != null;
            if (_lastSelectedItemStatus != hasSelectedItem)
            {
                _lastSelectedItemStatus = hasSelectedItem;
                EventManager.Instance.InventoryEvents.SelectedItemStatusChanged(hasSelectedItem);
            }
        }

        private void OnSubmitPressed(InputEventContext context) //todo handle context  
        {
            //_submitPressed = true;
            
            
                if (!_playerInventoryContentParent.gameObject.activeInHierarchy && !_storageInventoryContentParent.gameObject.activeInHierarchy) return; //duplicates functional (cascade of checks) in update method. Cn be extracted and used with other methods to reed of Update()
                if (_isInventoryWindowDragging) return;
                if (selectedItemGrid == null) return;

                ClickOnItem();
            
        }

        private void RmbForMenuOpen() //todo to integrate like lmb (add separete methods)
        {
            if (selectedItem != null)
            {
                return;
            }
            
            if (itemToOnHover != null)
            {
                itemTooltipController.HideTooltip();
               contextMenuController.ShowContextMenu(itemToOnHover);
            }
        }

        //if scene or place are changed -> close inventories
        private void OnTransition(string a, string b)
        {
            //Debug.Log("place trans");
            playerInventoryUI.HideMenu();
            storageInventoryUI.HideMenu();
        }

        private void OnTransition(SceneNamesEnum a) 
        {
            //Debug.Log("scene trans");
            playerInventoryUI.HideMenu();
            storageInventoryUI.HideMenu();
        }

        //for tooltip

        public void ShowTooltip(InventoryItem item)
        {
            if (_isContextMenuShown)
            {
                return;
            }
            itemTooltipController.ShowTooltip(item);
        }

        public void HideTooltip()
        {
            itemTooltipController.HideTooltip();
        }

        private void SetContextMenuStatus(bool isOpen)
        {
            _isContextMenuShown = isOpen;
        }

        public void RemoveItem(InventoryItem currentItem)
        {
            Destroy(currentItem.gameObject);
            _inventoryItemsList.Remove(currentItem);
        }
    }


}