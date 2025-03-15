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


    void Start()
    {
        Debug.Log(TimeManager.Instance);
        if (TimeManager.Instance != null)
        {
            Debug.Log(TimeManager.Instance);
            TimeManager.Instance.OnTimeOfDayChange += ChangeTimeOfDayText;
            TimeManager.Instance.OnTimeChange += ChangeTimeText;
            TimeManager.Instance.OnDateChange += ChangeDateText;
        }
        else
        {
            Debug.LogError("TimeManager instance is null!");
        }


    }

    // open popup
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escape is clicked");
            if (!isMenuOpen)
            {
                OpenMenu();

            }else
            {
                CloseMenu();
            }

        }

    }

    void OnDisable()
    {
        TimeManager.Instance.OnTimeOfDayChange -= ChangeTimeOfDayText;
        TimeManager.Instance.OnTimeChange -= ChangeTimeText;
        TimeManager.Instance.OnDateChange -= ChangeDateText;

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

    private void ChangeTimeOfDayText(string text)
    {
        timeOfDayText.SetText(text);
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
