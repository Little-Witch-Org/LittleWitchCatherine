using System;
using _Scripts.InventorySystem.ByGuide.Scriptable;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Scripts.InventorySystem.ByGuide
{
    public class ItemGrid : MonoBehaviour
    {
        public const float TileSizeWidth = 32;
        public const float TileSizeHeight = 32;

        private InventoryItem[,] _inventoryItemSlot;

        [SerializeField] int gridSizeWidth;
        [SerializeField] int gridSizeHeight;

        RectTransform _rectTransform;
        Vector2 _positionOnTheGrid = new Vector2(); //scene position (transform)
        Vector2Int _tileGridPosition = new Vector2Int(); //grid coordinates [1,1]....

        [SerializeField] private bool forPotions;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            Initialization(gridSizeWidth, gridSizeHeight);
        }

        //dynamically resizes grid by using gridSize params. //todo add methods to resize ? 
        private void Initialization(int gridWidth, int gridHeight)
        {
            _inventoryItemSlot = new InventoryItem[gridWidth, gridHeight];
            Vector2 gridSize =
                new Vector2(gridWidth * TileSizeWidth,
                    gridHeight * TileSizeHeight); //calculating grid size = slots * tile size
            _rectTransform.sizeDelta = gridSize;
        }

        public Vector2Int GetTileGridPosition(Vector2 mousePosition)
        {
            _positionOnTheGrid.x = mousePosition.x - _rectTransform.position.x;
            _positionOnTheGrid.y = _rectTransform.position.y - mousePosition.y;

            _tileGridPosition.x = (int)(_positionOnTheGrid.x / TileSizeWidth);
            _tileGridPosition.y = (int)(_positionOnTheGrid.y / TileSizeHeight);

            return _tileGridPosition;
        }
        
        public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY,
            ref InventoryItem overlapItem) //if unable to place - return false
        {
            if (forPotions)
            {
                if (!CheckItemTypeIsPotion(inventoryItem))
                {
                    return false;
                }
            }
    
            if (BoundaryCheck(posX, posY, inventoryItem.Width, inventoryItem.Height) == false)
            {
                return false;
            }
            
            
            if (!SimpleOverlapCheck(posX, posY, inventoryItem, ref overlapItem))
            {
                overlapItem = null;
                return false;
            }

            if (overlapItem != null)
            {
                CleanGridReference(overlapItem);
            }

            // Вызываем метод размещения с учетом формы
            PlaceItemWithShape(inventoryItem, posX, posY);

            return true;
        }
        
        private void PlaceItemWithShape(InventoryItem inventoryItem, int posX, int posY)
        {
            // Размещаем в иерархии
            RectTransform itemRectTransform = inventoryItem.GetComponent<RectTransform>();
            itemRectTransform.SetParent(_rectTransform);

            // Занимаем только нужные ячейки согласно маске формы
            for (int x = 0; x < inventoryItem.Width; x++)
            {
                for (int y = 0; y < inventoryItem.Height; y++)
                {
                    if (inventoryItem.IsCellOccupied(x, y))
                    {
                        _inventoryItemSlot[posX + x, posY + y] = inventoryItem;
                    }
                }
            }

            // Сохраняем позицию
            inventoryItem.onGridPositionX = posX;
            inventoryItem.onGridPositionY = posY;

            // Устанавливаем позицию
            itemRectTransform.localPosition = CalculatePositionOnGrid(inventoryItem, posX, posY);
        }
        
        private bool SimpleOverlapCheck(int posX, int posY, InventoryItem item, ref InventoryItem overlapItem)
        {
            for (int x = 0; x < item.Width; x++)
            {
                for (int y = 0; y < item.Height; y++)
                {
                    if (item.IsCellOccupied(x, y))
                    {
                        int gridX = posX + x;
                        int gridY = posY + y;
                    
                        if (!PositionCheck(gridX, gridY))
                            return false;

                        var existingItem = _inventoryItemSlot[gridX, gridY];
                        if (existingItem != null)
                        {
                            if (overlapItem == null)
                                overlapItem = existingItem;
                            else if (overlapItem != existingItem)
                                return false;
                        }
                    }
                }
            }
            return true;
        }
        
        public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
        {
            Vector2 position = new Vector2();
            position.x = posX * TileSizeWidth + TileSizeWidth * inventoryItem.Width / 2;
            position.y = -(posY * TileSizeHeight + TileSizeHeight * inventoryItem.Height / 2);
            return position;
        }


    

    public InventoryItem PickUpItem(int x, int y)
        {
            //get item (which body occupies provided cells). top-left corner of item
            InventoryItem toReturn = _inventoryItemSlot[x, y];

            if(toReturn == null){return null;} //if no item - return null
            
            //clear cells (filled by item body(script)) in array
            CleanGridReference(toReturn);
            
            //return item (current in that cells)
            return toReturn;
        }
    
        
        private void CleanGridReference(InventoryItem item)
        {
            for (int x = 0; x < item.Width; x++)
            {
                for (int y = 0; y < item.Height; y++)
                {
                    if (item.IsCellOccupied(x, y))
                    {
                        int gridX = item.onGridPositionX + x;
                        int gridY = item.onGridPositionY + y;
                        if (_inventoryItemSlot[gridX, gridY] == item)
                            _inventoryItemSlot[gridX, gridY] = null;
                    }
                }
            }
        }

        private bool PositionCheck(int posX, int posY)
        {
            if (posX < 0 || posY < 0)
            {
                return false;
            }

            if (posX >= gridSizeWidth || posY >= gridSizeHeight)
            {
                return false;
            }
            return true;
        }

        public bool BoundaryCheck(int posX, int posY, int width, int height)
        {
            //check top-left position
            if (PositionCheck(posX, posY) == false)
            {
                return false;
            }
            
            //check bottom right
            posX += width-1;
            posY += height-1;
            
            if (PositionCheck(posX, posY) == false)
            {
                return false;
            }
            return true;
        }
        
        internal InventoryItem GetItem(int x, int y)
        {
            if (!PositionCheck(x, y)) return null;
        
            InventoryItem item = _inventoryItemSlot[x, y];
            if (item == null) return null;
        
            // Проверяем, что это занятая ячейка предмета, а не "дырка" в его форме
            int localX = x - item.onGridPositionX;
            int localY = y - item.onGridPositionY;
        
            return (localX >= 0 && localY >= 0 && 
                    localX < item.Width && localY < item.Height && 
                    item.IsCellOccupied(localX, localY)) 
                ? item 
                : null;
        }
        
        public Vector2Int? FindSpaceForItem(InventoryItem itemToInsert)
        {
            int height = gridSizeHeight - itemToInsert.Height + 1;
            int width = gridSizeWidth - itemToInsert.Width + 1;

            for (int y = 0; y < height; y++) //inverted caus finding place in inventory from top left to bottom right 
            {
                for (int x = 0; x < width; x++)
                {
                    if (CheckAvailableSpace(x, y, itemToInsert))
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            return null;
        }
        
        public bool CheckAvailableSpace(int posX, int posY, InventoryItem item)
        {
            for (int x = 0; x < item.Width; x++)
            {
                for (int y = 0; y < item.Height; y++)
                {
                    if (item.IsCellOccupied(x, y) && 
                        (posX + x >= gridSizeWidth || 
                         posY + y >= gridSizeHeight || 
                         _inventoryItemSlot[posX + x, posY + y] != null))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        private bool CheckItemTypeIsPotion(InventoryItem item)
        {
            
            if (item.itemData is PotionItemSo)
            {
                return true;
            }

            return false;
        }

        public int GetGridSizeWidth()
        {
            return gridSizeWidth;
        }

        public int GetGridSizeHeight()
        {
            return gridSizeHeight;
        }


    }
}
