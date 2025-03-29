using System;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//todo separate cheat menu from ui manager (like journal)
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public GameObject cheatMenu;
    public GameObject questMenu;
    
    public bool isMenuOpen;

    public TMP_Text timeText;
    public TMP_Text dateText;
    public TMP_Text yearText;
    public TMP_Text timeOfDayText;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.TimeEvents.OnTimeOfDayChange += ChangeTimeOfDayText;
        EventManager.Instance.TimeEvents.OnTimeChange += ChangeTimeText;
        EventManager.Instance.TimeEvents.OnDateChange += ChangeDateText;
        
        EventManager.Instance.InputEvents.OnMenuPressed += ToggleCheatMenu;
    }
    
    private void OnDisable()
    {
        EventManager.Instance.TimeEvents.OnTimeOfDayChange -= ChangeTimeOfDayText;
        EventManager.Instance.TimeEvents.OnTimeChange -= ChangeTimeText;
        EventManager.Instance.TimeEvents.OnDateChange -= ChangeDateText;
        
        EventManager.Instance.InputEvents.OnMenuPressed -= ToggleCheatMenu;
    }


    void Start()
    {
        if (TimeManagerTurnBased.Instance != null)
        {
            TimeManagerTurnBased.Instance.AddSeconds(0);//handle first ui update
        }
        else
        {
            Debug.LogError("TimeManagerTurnBased instance is null!");
        }


    }
    

    private void ChangeTimeText(string text)
    {
        timeText.text = text;
    }

    private void ChangeDateText(string text)
    {
        dateText.text = text;
    }

    //todo not supported
    private void ChangeYearText(string text)
    {
        yearText.text = text;
    }

    private void ChangeTimeOfDayText(TimeOfDay timeOfDay)
    {
        timeOfDayText.SetText(timeOfDay.ToString());
    }
    
    public void ToggleCheatMenu()
    {
        isMenuOpen = !isMenuOpen;
        
        if (isMenuOpen)
        {
            Debug.Log("Opening Menu");
            cheatMenu.SetActive(true);
        }
        else
        {
            Debug.Log("Closing Menu");
            cheatMenu.SetActive(false);
        }
    }
    
}
