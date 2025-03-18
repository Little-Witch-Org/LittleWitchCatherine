using System;
using System.Collections.Generic;
using _Scripts.Components.TimeManagement.Enums;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Component stores sprites of rooms and changes them depending on game state
/// </summary>
public class RoomStateChangeComponent_spriteSwap_temp : MonoBehaviour
{
    [SerializeField] private List<Sprite> roomStateSprites;
    
    public SpriteRenderer spriteRendererCurrent;
    public SpriteRenderer spriteRendererToChange;
    public float transitionDuration = 2f;

    //todo component need to check game state and change sprite according to it (refactor for using script object or something)
    
    public TimeOfDay timeOfDay;

    private void LoadRoomImageByState()
    {
        //get timeOfDay state
        timeOfDay = TimeManager.Instance.timeOfDay;

        //check that target sprite is full transparent
        spriteRendererToChange.color = new Color(1, 1, 1, 0);

        //Get sprite depending on current timeOfDay spate
        Sprite spriteToChange = null;

        switch (timeOfDay)
        {
            case TimeOfDay.Morning:
            {
                spriteToChange = roomStateSprites[0];
                break;
            }
            case TimeOfDay.Afternoon:
            {
                spriteToChange = roomStateSprites[1];
                break;
            }
            case TimeOfDay.Evening:
            {
                spriteToChange = roomStateSprites[2];
                break;
            }
            case TimeOfDay.Night:
            {
                spriteToChange = roomStateSprites[3];
                break;
            }
        }

        spriteRendererToChange.sprite = spriteToChange;
        
        spriteRendererCurrent.DOFade(0, transitionDuration);
        
        spriteRendererToChange.DOFade(1, transitionDuration);


    }

    private void Start()
    {
        LoadRoomImageByState();
    }
    
}