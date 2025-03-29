using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.QuestSystem
{
    /// <summary>
    /// Checks game manager on quest steps availability (quest started and step is on the quest manager),
    /// and if they exist -> enables object on scene which can trigger quest step progress (in case of game objects that can appear on scene)
    /// </summary>
    
    public class QuestStepTriggerEnabler : MonoBehaviour
    {
        [SerializeField] private QuestInfoSo questSo; //need to check status and self destroy if steps passed (for multiple copies of this trigger)
        [SerializeField] private QuestStep questStepObject;
        [SerializeField] private GameObject questStepTriggerObjectToEnable;

        private void Start()
        {
            EnableQuestStepTriggerObject(); //handle appear on scene (change location case) //
            DestroyIfNoChild();
        }
        

        private void EnableQuestStepTriggerObject()
        {
            if (QuestManager.Instance == null)
            {
                Debug.LogError("QuestManager.Instance no exist!");
                return;
            }

            // check quest step in quest manager children
            Transform foundChild = FindChildByNameWithClone(QuestManager.Instance.transform, questStepObject.name);

            if (foundChild != null)
            {
                //Debug.Log($"Найден дочерний объект: {foundChild.name}");

                if (questStepTriggerObjectToEnable != null)
                {
                    questStepTriggerObjectToEnable.SetActive(true);
                    //Debug.Log($"Активирован объект: {questStepTriggerObjectToEnable.name}");
                }
                else
                {
                    Debug.LogWarning("Не назначен объект для активации!");
                }
            }
            else
            {
                //Debug.Log($"Дочерний объект с именем {questStepObject.name} не найден!");
            }
        }

        // Рекурсивный поиск дочернего объекта по имени (с учётом (Clone))
        private Transform FindChildByNameWithClone(Transform parent, string baseName)
        {
            foreach (Transform child in parent)
            {
                // Удаляем "(Clone)" из имени, если оно есть, перед сравнением
                string cleanName = child.name.Replace("(Clone)", "").Trim();
                
                if (cleanName == baseName)
                    return child;

                Transform result = FindChildByNameWithClone(child, baseName);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void DestroyIfNoChild()
        {
            if (transform.childCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}