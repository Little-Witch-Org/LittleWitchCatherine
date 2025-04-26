using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.NarrativeAndCutscenes.UI
{
    /// <summary>
    /// Show cutscene images from storyProgressManager //todo add animation?
    /// </summary>
    public class CutsceneUI : MonoBehaviour
    {
        [SerializeField] private GameObject contentParent;
        [SerializeField] private GameObject cutsceneImage;
        [SerializeField] private Image image;


        private void OnEnable()
        {
            EventManager.Instance.CutsceneEvents.OnShowCutsceneUI += ShowUI;
            EventManager.Instance.CutsceneEvents.OnHideCutsceneBackground += HideUI;
            EventManager.Instance.CutsceneEvents.OnSetCutsceneImage += SetCutsceneImage;
            EventManager.Instance.CutsceneEvents.OnShowCutsceneImage += ShowCutsceneImage;
            EventManager.Instance.CutsceneEvents.OnHideCutsceneImage += HideCutsceneImage;
            
        }

        private void OnDisable()
        {
            EventManager.Instance.CutsceneEvents.OnShowCutsceneUI -= ShowUI;
            EventManager.Instance.CutsceneEvents.OnHideCutsceneBackground -= HideUI;
            EventManager.Instance.CutsceneEvents.OnSetCutsceneImage -= SetCutsceneImage;
            EventManager.Instance.CutsceneEvents.OnShowCutsceneImage -= ShowCutsceneImage;
            EventManager.Instance.CutsceneEvents.OnHideCutsceneImage -= HideCutsceneImage;
        }


        private void ShowUI()
        {
            contentParent.SetActive(true);
        }

        private void HideUI()
        {
            contentParent.SetActive(false);
        }

        private void SetCutsceneImage(Sprite sprite)
        {
            image.sprite = sprite;
        }
        
        private void ShowCutsceneImage()
        {
            cutsceneImage.SetActive(true);
        }
        private void HideCutsceneImage()
        {
            cutsceneImage.SetActive(false);
        }
    }
}