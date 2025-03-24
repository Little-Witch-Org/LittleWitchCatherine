using System;
using _Scripts.Components.TimeManagement.Enums;
using _Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject cheatMenu;
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
        EventManager.Instance.timeEvents.OnTimeOfDayChange += ChangeTimeOfDayText;
        EventManager.Instance.timeEvents.OnTimeChange += ChangeTimeText;
        EventManager.Instance.timeEvents.OnDateChange += ChangeDateText;
    }
    
    private void OnDisable()
    {
        EventManager.Instance.timeEvents.OnTimeOfDayChange -= ChangeTimeOfDayText;
        EventManager.Instance.timeEvents.OnTimeChange -= ChangeTimeText;
        EventManager.Instance.timeEvents.OnDateChange -= ChangeDateText;
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

    // open popup
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("escape is clicked");
            if (!isMenuOpen)
            {
                OpenMenu();

            }else
            {
                CloseMenu();
            }

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

    public void OpenMenu()
    {
        cheatMenu.SetActive(true);
        isMenuOpen = true;


    }

    public void CloseMenu()
    {
        cheatMenu.SetActive(false);
        isMenuOpen = false;

    }
    
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        cheatMenu.SetActive(isMenuOpen);
    }
}
