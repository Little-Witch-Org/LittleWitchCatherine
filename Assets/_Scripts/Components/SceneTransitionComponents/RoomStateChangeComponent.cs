using System;
using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using UnityEngine;

/// <summary>
/// Component stores sprites of rooms and changes them depending on game state
/// </summary>
public class RoomStateChangeComponent : MonoBehaviour
{
    [SerializeField] private List<Sprite> roomStateSprites;

    //todo component need to check game state and change sprite according to it (refactor for using script object or something)
    
    public TimeOfDay timeOfDay;

    private void loadRoomState()
    {
        timeOfDay = TimeManager.Instance.timeOfDay;

        switch (timeOfDay)
        {
            case TimeOfDay.Morning:
            {
                GetComponent<SpriteRenderer>().sprite = roomStateSprites[0];
                break;
            }
            case TimeOfDay.Afternoon:
            {
                GetComponent<SpriteRenderer>().sprite = roomStateSprites[1];
                break;
            }
            case TimeOfDay.Evening:
            {
                GetComponent<SpriteRenderer>().sprite = roomStateSprites[2];
                break;
            }
            case TimeOfDay.Night:
            {
                GetComponent<SpriteRenderer>().sprite = roomStateSprites[3];
                break;
            }
        }
        
        //GetComponent<SpriteRenderer>().sprite = roomStateSprites[0];
    }

    private void Start()
    {
        loadRoomState();
    }
}
