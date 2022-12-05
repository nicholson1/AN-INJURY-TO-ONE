using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Sprite ButtonUp;
    [SerializeField] private Sprite ButtonDown;

    [SerializeField] private SpriteRenderer image;

    private PlatformMoveOnEvent platformMoveOnEvent;
    public bool StopsOnButtonUp;


    private void Start()
    {
        platformMoveOnEvent = gameObject.GetComponentInChildren<PlatformMoveOnEvent>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PushButton();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        LeaveButton();
    }

    private void PushButton()
    {

        if (platformMoveOnEvent != null)
        {
            platformMoveOnEvent.enabled = true;
        }
        
        image.sprite = ButtonDown;
        GetComponent<Interactable>().isInteractable = false;
    }

    private void LeaveButton()
    {
        image.sprite = ButtonUp;
        GetComponent<Interactable>().isInteractable = true;

        if(platformMoveOnEvent != null && StopsOnButtonUp)
        {
            platformMoveOnEvent.enabled = false;
        }
    }
}
