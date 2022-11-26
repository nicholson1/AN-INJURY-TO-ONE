using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonpressing : MonoBehaviour
{
    //when the button is pressed, we want to activate the movement script in the child platform

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.GetComponentInChildren<PlatformMoveOnEvent>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("something hit the button");
        if(other.gameObject.tag is "Player")
        {
            Debug.Log("player hit the button!");
            gameObject.GetComponentInChildren<PlatformMoveOnEvent>().enabled = true;
            //activate the platform movement script in the sibling platform object
        }
        //we also want friends to be able to push buttons
        else if (other.gameObject.tag is "Friend")
        {
            gameObject.GetComponentInChildren<PlatformMoveOnEvent>().enabled = true;
            //activate the platform movement script in the sibling platform object
        }
    }
}
