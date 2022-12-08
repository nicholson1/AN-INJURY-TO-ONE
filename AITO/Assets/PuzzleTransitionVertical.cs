using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTransitionVertical : MonoBehaviour
{
    public static event Action<bool> TransitionUp;

    [SerializeField] private bool canGoUp = true;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.transform.position.y > this.transform.position.y)
            {
                if (canGoUp)
                {
                    TransitionUp(true);
                    canGoUp = false;
                }
            }
            else
            {
                if (!canGoUp)
                {
                    TransitionUp(false);
                    canGoUp = true;
                }
            }
            //this.gameObject.SetActive(false);
        }
    }
}
