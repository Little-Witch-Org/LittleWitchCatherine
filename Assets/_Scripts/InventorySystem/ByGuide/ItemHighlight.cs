using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.InventorySystem.ByGuide
{
    public class ItemHighlight:MonoBehaviour //todo add shapemask without highlight - then update in (need to multiple objects on cell ?
    {
        [SerializeField] RectTransform highlighter;
        [SerializeField] private Image highlightImage;
        [SerializeField] private Color validColor = new Color(0, 1, 0, 0.3f);
        [SerializeField] private Color invalidColor = new Color(1, 0, 0, 0.3f);

        
        public void DisplayHighlight(bool highlighted)
        {
            highlighter.gameObject.SetActive(highlighted);
        }

        public void UpdateHighlight(InventoryItem item, bool isValid)
        {
            if (item == null) return;

            // Создаем текстуру формы
            Texture2D shapeTex = CreateShapeTexture(item);
            
            // Устанавливаем спрайт
            highlightImage.sprite = Sprite.Create(
                shapeTex,
                new Rect(0, 0, shapeTex.width, shapeTex.height),
                new Vector2(0.5f, 0.5f)
            );

            // Цвет в зависимости от валидности
            highlightImage.color = isValid ? validColor : invalidColor;

            // Размеры
            highlighter.sizeDelta = new Vector2(
                item.Width * ItemGrid.TileSizeWidth,
                item.Height * ItemGrid.TileSizeHeight
            );
        }



        //for highlight on grid
        public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
        {
            Vector2 position = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);
            
            highlighter.localPosition = position;
        }

        public void SetParent(ItemGrid targetGrid)
        {
            if(targetGrid == null){return;}
            
            highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
            
            highlighter.transform.SetAsFirstSibling();
        } 
        
        private Texture2D CreateShapeTexture(InventoryItem item)
        {
            Texture2D tex = new Texture2D(item.Width, item.Height)
            {
                filterMode = FilterMode.Point
            };

            // Заполняем с учетом правильной ориентации
            for (int x = 0; x < item.Width; x++)
            {
                for (int y = 0; y < item.Height; y++)
                {
                    // Инвертируем Y-координату для правильного отображения
                    int textureY = item.Height - 1 - y;
                    bool occupied = item.IsCellOccupied(x, y);
                    tex.SetPixel(x, textureY, occupied ? validColor : Color.clear);
                }
            }
            tex.Apply();
            return tex;
        }
    }
    
    
}