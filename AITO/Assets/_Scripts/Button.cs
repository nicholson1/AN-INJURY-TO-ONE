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

    // JT: to make a single button can affect multiple objects (hazards or platforms),
    // only need to drag additional objects to this list in Unity
    [SerializeField]private GameObject[] _additionalTargets;
    private bool haveMultipleTargets = false;


    private void Start()
    {
        platformMoveOnEvent = gameObject.GetComponentInChildren<PlatformMoveOnEvent>();
        startPosition = transform.position;

        // JT: to make a single button can affect multiple objects (hazards or platforms)
        if (_additionalTargets.Length > 0)
        {
            this.haveMultipleTargets = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PushButton();

        // JT: to make a single button can affect multiple objects (hazards or platforms)
        if (this.haveMultipleTargets)
        {
            foreach (GameObject go in _additionalTargets)
            {
                PushButton(go);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        LeaveButton();

        // JT: to make a single button can affect multiple objects (hazards or platforms)
        if (this.haveMultipleTargets)
        {
            foreach (GameObject go in _additionalTargets)
            {
                LeaveButton(go);
            }
        }
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

    // JT: to make a single button can affect multiple objects (hazards or platforms)
    private void PushButton(GameObject go)
    {
        PlatformMoveOnEvent pmoe = go.gameObject.GetComponent<PlatformMoveOnEvent>();

        if (pmoe != null)
        {
            pmoe.MoveToWaypoint = true;
        }

        image.sprite = ButtonDown;
        GetComponent<Interactable>().isInteractable = false;
    }

    private void LeaveButton(GameObject go)
    {
        PlatformMoveOnEvent pmoe = go.gameObject.GetComponent<PlatformMoveOnEvent>();

        image.sprite = ButtonUp;
        GetComponent<Interactable>().isInteractable = true;


        if (pmoe != null && StopsOnButtonUp)
        {
            pmoe.MoveToWaypoint = false;
            pmoe.curIndex = 0;
            pmoe.c = pmoe.Waypoints[0];

        }
    }
}
