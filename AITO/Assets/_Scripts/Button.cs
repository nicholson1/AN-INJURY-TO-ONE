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

    private Vector3 startPosition;


    private void Start()
    {
        platformMoveOnEvent = gameObject.GetComponentInChildren<PlatformMoveOnEvent>();
        startPosition = transform.position;
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
            platformMoveOnEvent.MoveToWaypoint = true;
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
            platformMoveOnEvent.MoveToWaypoint = false;
            platformMoveOnEvent.curIndex = 0;
            platformMoveOnEvent.c = platformMoveOnEvent.Waypoints[0];

        }
    }

    
}
