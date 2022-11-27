using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTransitions : MonoBehaviour
{

    public static event Action<bool> TransitionRight;

    [SerializeField] private bool canGoRight = true;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            
                if (collision.transform.position.x > this.transform.position.x)
                {
                    if (canGoRight)
                    {
                        TransitionRight(true);
                        canGoRight = false;
                    }

                    //transition left

                }
                
                else
                {
                    if (!canGoRight)
                    {
                        TransitionRight(false);
                        canGoRight = true;

                    }

                    //transition Right

                }
                //this.gameObject.SetActive(false);
            
        }
    }
}
