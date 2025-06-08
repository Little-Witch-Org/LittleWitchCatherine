using System.Linq;
using _Scripts.Enums;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Serialization;

namespace _Scripts.InventorySystem.ByGuide.Scriptable
{
    /// <summary>
    /// Default SO for item. Can be inherited to create special item types (consumable/equipment etc).
    /// Stores technical info about item. connects to Unity Localization string table and gets cells by id.
    /// </summary>
    [CreateAssetMenu(fileName = "ItemDataSo", menuName = "ScriptableObjects/Items/ItemDataSo")]
    public class ItemDataSo : ScriptableObject
    {
        [Header("Technical")] 
        public string itemId;

        [Header("UI")] 
        public Sprite itemIcon;

        [Header("Grid size")] 
        public int width = 1;
        public int height = 1;
        

        
        [Header("Shape Configuration")]
        [SerializeField] private bool[] shapeMaskSerialized; // Сериализуемый одномерный массив
        
        
        public bool IsCellOccupiedOnMask(int x, int y)
        {


            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                Debug.LogWarning($"Coordinates ({x}, {y}) out of the range");
            }
            
            //Debug.Log($"Coordinates ({x}, {y}) " + shapeMask[x, y]);


            return ShapeMask[x, y];
        }


        protected virtual void OnValidate()
        {
            InitializeShapeMask();
        }

        // Несериализуемое свойство для доступа к данным
        public bool[,] ShapeMask
        {
            get
            {
                
                bool[,] mask = new bool[width, height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int index = y * width + x;
                        if (index < shapeMaskSerialized.Length)
                        {
                            mask[x, y] = shapeMaskSerialized[index]; 
                        }
                        else
                        {
                            mask[x, y] = true; // Значение по умолчанию
                        }
                    }
                }
                return mask;
            }
            set
            {
                shapeMaskSerialized = new bool[width * height];
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int index = y * width + x;
                        shapeMaskSerialized[index] = value[x, y];
                    }
                }
            }
        }

        public void InitializeShapeMask()
        {
            bool[] previousSerialized = shapeMaskSerialized;

            // Проверка на необходимость инициализации
            if (previousSerialized == null || previousSerialized.Length != width * height)
            {
                // Создание новой маски по умолчанию
                bool[] newSerialized = new bool[width * height];
                for (int i = 0; i < newSerialized.Length; i++)
                {
                    newSerialized[i] = true;
                }

                // Попробуем перенести старые значения
                if (previousSerialized != null)
                {
                    int prevWidth = Mathf.Max(1, previousSerialized.Length / height); // подстраховка
                    int minWidth = Mathf.Min(width, prevWidth);
                    int minHeight = Mathf.Min(height, previousSerialized.Length / prevWidth);

                    for (int x = 0; x < minWidth; x++)
                    {
                        for (int y = 0; y < minHeight; y++)
                        {
                            int oldIndex = y * prevWidth + x;
                            int newIndex = y * width + x;

                            if (oldIndex < previousSerialized.Length && newIndex < newSerialized.Length)
                            {
                                newSerialized[newIndex] = previousSerialized[oldIndex];
                            }
                        }
                    }
                }

                shapeMaskSerialized = newSerialized;
            }
        }
        
        
        
        
        
        //item info from sheet (used unity localization with fake locales as columns)
        protected const string TableName = "InventoryItemsLocalizationTable";

        private const string NameRuColumn = "item_name_ru";
        private const string NameEngColumn = "item_name_eng";
        private const string DescriptionRuColumn = "item_desc_ru";
        private const string DescriptionEngColumn = "item_desc_eng";
        
        public string GetName()
        {
            string currentLang = LocalizationSettings.SelectedLocale.Identifier.Code;
            string column = currentLang == "ru" ? NameRuColumn : NameEngColumn;
            return GetLocalizedCell(TableName, itemId, column);
        }
        
        public string GetDescription()
        {
            string currentLang = LocalizationSettings.SelectedLocale.Identifier.Code;
            string column = currentLang == "ru" ? DescriptionRuColumn : DescriptionEngColumn;
            return GetLocalizedCell(TableName, itemId, column);
        }


        //todo add data cash in inventoryItem to prevent frequency calls to table?
        protected static string GetLocalizedCell(string tableName, string key, string columnAsLocale)
        {
            // Ищем "локаль" с названием колонки
            var locale = LocalizationSettings.AvailableLocales.Locales
                .FirstOrDefault(l => l.Identifier.Code == columnAsLocale);

            if (locale == null)
            {
                Debug.LogWarning($"Локаль с кодом '{columnAsLocale}' не найдена среди доступных.");
                return "";
            }

            // Получаем таблицу в этой "локали"
            var table = LocalizationSettings.StringDatabase.GetTable(tableName, locale) as StringTable;

            if (table == null)
            {
                Debug.LogWarning($"Таблица '{tableName}' не найдена для локали '{columnAsLocale}'.");
                return "";
            }

            var entry = table.GetEntry(key);
            if (entry == null)
            {
                Debug.LogWarning($"Ключ '{key}' не найден в таблице '{tableName}' и локали '{columnAsLocale}'.");
                return "";
            }

            return entry.LocalizedValue;
        }
    }
}

    
