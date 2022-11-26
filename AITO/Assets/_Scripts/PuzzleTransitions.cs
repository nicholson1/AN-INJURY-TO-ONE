using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTransitions : MonoBehaviour
{

    public static event Action<bool> TransitionRight;

    [SerializeField] private bool canTransition = false;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canTransition)
            {
                if (collision.transform.position.x > this.transform.position.x)
                {
                    TransitionRight(true);
                    //transition left

                }
                else
                {
                    TransitionRight(false);
                    //transition Right

                }
                //this.gameObject.SetActive(false);
            }
        }
    }
}
