using System;
using _Scripts.InventorySystem.ByGuide.Interfaces;
using _Scripts.InventorySystem.ByGuide.Scriptable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.InventorySystem.ByGuide
{
    /// <summary>
    /// Raycast target disabled (can't use onPointer events)
    /// </summary>
    public class InventoryItem : MonoBehaviour
    {
        public ItemDataSo itemData;

        public int onGridPositionX;
        public int onGridPositionY;

        public RotationAngle currentRotation = RotationAngle.Angle0;

        public enum RotationAngle
        {
            Angle0,    // 0°
            Angle90,   // -90° (+90°)
            Angle180,  // 180°
            Angle270   // -90° (+270°)
        }
        

        public int Height => 
            (currentRotation == RotationAngle.Angle90 || currentRotation == RotationAngle.Angle270) 
                ? itemData.width 
                : itemData.height;

        public int Width => 
            (currentRotation == RotationAngle.Angle90 || currentRotation == RotationAngle.Angle270) 
                ? itemData.height
                : itemData.width;
        
        public void Set(ItemDataSo itemDataSo)
        {
            itemData = itemDataSo;

            GetComponent<Image>().sprite = itemData.itemIcon;

            //update image scale depending on grid tile size
            Vector2 size = new Vector2();
            size.x = itemData.width * ItemGrid.TileSizeWidth;
            size.y = itemData.height * ItemGrid.TileSizeHeight;
            GetComponent<RectTransform>().sizeDelta = size;
        }

        public void Rotate()
        {
            //switch angle enum (rmb)
            currentRotation = currentRotation switch
            {
                RotationAngle.Angle0   => RotationAngle.Angle90,
                RotationAngle.Angle90  => RotationAngle.Angle180,
                RotationAngle.Angle180 => RotationAngle.Angle270,
                RotationAngle.Angle270 => RotationAngle.Angle0,
                _ => RotationAngle.Angle0
            };

            // switch rotation
            float rotationZ = currentRotation switch
            {
                RotationAngle.Angle0   => 0f,
                RotationAngle.Angle90  => -90f,
                RotationAngle.Angle180 => -180f,
                RotationAngle.Angle270 => -270f,
                _ => 0f
            };

            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.rotation = Quaternion.Euler(0, 0, rotationZ);
        }
        
        public bool IsCellOccupied(int x, int y)
        {
            int newX = x, newY = y;

            switch (currentRotation)
            {
                case RotationAngle.Angle90:
                    newX = y;
                    newY = (Width - 1) - x;
                    break;
                case RotationAngle.Angle180:
                    newX = (Width - 1) - x;
                    newY = (Height - 1) - y;
                    break;
                case RotationAngle.Angle270:
                    newX = (Height - 1) - y;
                    newY = x;
                    break;
            }

            return itemData.IsCellOccupiedOnMask(newX, newY);
        }
    }
}