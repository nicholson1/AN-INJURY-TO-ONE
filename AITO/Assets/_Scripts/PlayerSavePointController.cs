using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PlayerSavePointController : MonoBehaviour
{
    // singleton pattern only have one static instance, initiated at the beginning in SaveLoadController.cs,
    // write this line for updating the variables inside
    SingletonController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = SingletonController.Instance;

        // save the born position of player at the beginning of game scene
        Debug.Log("Player Information Saved");
        instance.UpdateSavedPlayerPosition(this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // used during development, update the save data for player position when O key is pressed down
        if (Input.GetKeyDown(KeyCode.O))
        {
            // for testing disable PlayerController script from this script
            //this.GetComponent<PlayerController>().enabled = false;
            //Debug.Log("Player Controller Disabled");

            Debug.Log("Player Information Saved");
            instance.UpdateSavedPlayerPosition(this.transform.position);
        }

        // used during development, load the save data for player position when P key is pressed down
        if (Input.GetKeyDown(KeyCode.P))
        {
            // for testing disable PlayerController script from this script
            //this.GetComponent<PlayerController>().enabled = true;
            //Debug.Log("Player Controller Enabled");

            Debug.Log("Player Information Loaded");
            OverwritePlayerPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CameraTransitionTrigger")
        {
            Debug.Log("Player Information Saved");
            instance.UpdateSavedPlayerPosition(this.transform.position);
        }

        if (collision.gameObject.tag == "DeathArea")
        {
            Debug.Log("Entered DeathArea, Back to save point");
            // if enter DeathArea, player dead and will be sent back to the prioior save point
            OverwritePlayerPosition();
        }
    }

    private void OverwritePlayerPosition()
    {
        // using disabled > set > enabled to avoid delay in changing position
        this.GetComponent<PlayerController>().enabled = false;
        //this.transform.position = instance.ReturnSavedPlayerPosition();

        // each time the player is reborn, it will appear at a higher y position and fall down
        Vector3 newPosition = instance.ReturnSavedPlayerPosition();
        newPosition.y = 14.0f;
        this.transform.position = newPosition;

        this.GetComponent<PlayerController>().enabled = true;
    }
}
