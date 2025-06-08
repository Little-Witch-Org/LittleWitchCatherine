using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.InventorySystem.ByGuide
{
    /// <summary>
    /// Manages tooltip position/size/text. todo add not hiding tooltip if we hover from one item to another <!---->
    /// </summary>
    public class TooltipController : MonoBehaviour
    {
        //public static TooltipController Instance; //there is one tooltip object on scene.
        
        [SerializeField] private RectTransform itemTooltipPanel;
        [SerializeField] private RectTransform backgroundTransform;
        [SerializeField] private TMP_Text tooltipNameText;
        [SerializeField] private TMP_Text tooltipDescriptionText;
        [SerializeField] private Vector2 offset;

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private bool _isFollowingCursor;
        
        private Coroutine _showTooltipCoroutine;
        private InventoryItem _previousItem;
        
        private readonly float _maxDescriptionWidth = 200f;


        private Dictionary<string, bool> _inventoryOpenedStatus;

        void Awake()
        {
            //Instance = this;
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            HideTooltip();
            _inventoryOpenedStatus = new Dictionary<string,bool>();
            _inventoryOpenedStatus.Add("PlayerInventory", false);
            _inventoryOpenedStatus.Add("StorageInventory", false);
        }

        private void OnEnable()
        {
            EventManager.Instance.InventoryEvents.OnInventoryOpenedStatusChanged += OnInventoryStatusChanged;
        }
        
        private void OnDisable()
        {
            EventManager.Instance.InventoryEvents.OnInventoryOpenedStatusChanged -= OnInventoryStatusChanged;
        }
        
        void Update()
        {
            if (!_isFollowingCursor) return;

            SetTooltipPosition();
        }

        private void SetTooltipPosition()
        {
            Vector2 tooltipSize = itemTooltipPanel.rect.size;
            Vector2 mousePos = Input.mousePosition;
            Vector2 offsetToUse = offset;

            float pivotX = 0f;
            float pivotY = 0f;

            // screen borders in pix
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // check X (width)
            if (mousePos.x + tooltipSize.x + offset.x > screenWidth)
            {
                offsetToUse.x = -offset.x; // reflect horiz
                pivotX = 1f;
            }

            // Проверка по Y (по высоте)
            if (mousePos.y + tooltipSize.y + offset.y > screenHeight)
            {
                offsetToUse.y = -offset.y-20; // reflect vert (-20, чтобы компенсировать иконку мыши.. хуйня из-за разного разрешения - разный размер курсора)
                pivotY = 1f;
            }

            // pivot
            _rectTransform.pivot = new Vector2(pivotX, pivotY);

            // position
            _rectTransform.position = mousePos + offsetToUse;
        }
        

        public void ShowTooltip(InventoryItem item)
        {
            _previousItem = item;
            
            _showTooltipCoroutine = StartCoroutine(ShowDelayed(_previousItem.itemData.GetName(), _previousItem.itemData.GetDescription()));
        }

        private IEnumerator ShowDelayed(string nameT, string descriptionT)
        {
            //Debug.Log("cor started");


            tooltipNameText.text = nameT;
            tooltipDescriptionText.text = descriptionT;



            yield return null;

            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipNameText.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipDescriptionText.rectTransform);

            float descWidth = tooltipDescriptionText.preferredWidth;

            float finalNameWidth = tooltipNameText.preferredWidth;


            float finalDescWidth;

            if (finalNameWidth > descWidth)
            {
                finalDescWidth = finalNameWidth;

            }
            else if (finalNameWidth > _maxDescriptionWidth && finalNameWidth < descWidth)
            {
                finalDescWidth = finalNameWidth;
            }
            else
            {
                finalDescWidth = Mathf.Min(descWidth, _maxDescriptionWidth);

            }



            tooltipNameText.rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                finalNameWidth
            );

            tooltipDescriptionText.rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                finalDescWidth
            );


            LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundTransform);

            itemTooltipPanel.sizeDelta = backgroundTransform.sizeDelta;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(itemTooltipPanel);

            yield return new WaitForSeconds(0.5f);

            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = false;

        }

        /*public void UpdateTooltipImmediately(string nameT, string descriptionT) //todo to use this ?
        {
            StopCoroutine(_showTooltipCoroutine);

            tooltipNameText.text = nameT;
            tooltipDescriptionText.text = descriptionT;
            LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundTransform);

            if(_canvasGroup.alpha > 0) // Если уже виден
            {
                _showTooltipCoroutine = StartCoroutine(ShowDelayed(nameT, descriptionT));
            }
        }*/

        public void HideTooltip()
        {
            //Debug.Log("hide tooltip");
            
            if (_showTooltipCoroutine != null)
            {
                StopCoroutine(_showTooltipCoroutine);
            }

            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        private void OnInventoryStatusChanged(bool isInventoryOpen, string inventoryName)
        {
            _inventoryOpenedStatus[inventoryName] = isInventoryOpen;
            
            _isFollowingCursor = false;
            
            foreach (var pair in _inventoryOpenedStatus)
            {
                if (pair.Value)
                {
                    _isFollowingCursor = true;
                    break;
                }
            }
        }
        
        
        
        
        /*private Tween _tween;
        
        public void ShowTooltip(string content)
        {
            KillCurrentTween();
            tooltipNameText.text = content;
            //_canvasGroup.alpha = 1;.
            
            //add delay before show animation
            _canvasGroup.DOFade(1f, 1f).SetEase(Ease.Linear).Play();
        }

        public void HideContextMenu()
        {
            KillCurrentTween();
            _canvasGroup.alpha = 0;
            
        }
        
        private bool InAnimation() => _tween != null && _tween.IsActive();

        private void KillCurrentTween()
        {
            if (InAnimation())
            {
                _tween.Kill();
            }
        }*/
    }
}