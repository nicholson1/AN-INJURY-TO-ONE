using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Sprite ButtonUp;
    [SerializeField] private Sprite ButtonDown;

    [SerializeField] private SpriteRenderer image;


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
        gameObject.GetComponentInChildren<PlatformMoveOnEvent>().enabled = true;
        image.sprite = ButtonDown;
        GetComponent<Interactable>().isInteractable = false;
    }

    private void LeaveButton()
    {
        image.sprite = ButtonUp;
        GetComponent<Interactable>().isInteractable = true;


    }
}
