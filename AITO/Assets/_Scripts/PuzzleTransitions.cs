using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTransitions : MonoBehaviour
{

    public static event Action Transition;

    [SerializeField] private bool canTransition = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canTransition)
            {
                Transition();
                this.gameObject.SetActive(false);
            }
        }
    }
}
