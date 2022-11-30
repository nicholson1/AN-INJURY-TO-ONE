using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PlayerSavePointController : MonoBehaviour
{
    private float savedAccel;
    private float savedMoveClamp;
    private bool isOnGround = false;
    private bool isOverwriting = false;
    private bool isFreezing = false;

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

        this.savedAccel = this.GetComponent<PlayerController>().ReturnAcceleration();
        this.savedMoveClamp = this.GetComponent<PlayerController>().ReturnMoveClamp();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOverwriting)
        {
            if (!this.isOnGround)
            {
                FreezeWalking();
            }
            else
            {
                //RecoverWalking();

                StartCoroutine(RecoverWalkingCoroutine());

                this.isOverwriting = false;
                Debug.Log("Overwriting Ends");
            }
        }

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

        if (collision.gameObject.tag == "WalkableArea")
        {
            this.isOnGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WalkableArea")
        {
            this.isOnGround = false;
        }
    }

    private void OverwritePlayerPosition()
    {
        this.isOverwriting = true;
        this.isFreezing = true;

        // using disabled > set > enabled to avoid delay in changing position
        this.GetComponent<PlayerController>().enabled = false;
        this.transform.position = instance.ReturnSavedPlayerPosition();

        // each time the player is reborn, it will appear at a higher y position and fall down
        //Vector3 newPosition = instance.ReturnSavedPlayerPosition();
        //newPosition.y = 14.0f;
        //this.transform.position = newPosition;

        this.GetComponent<PlayerController>().enabled = true;

        Debug.Log("Overwriting");
    }

    private void FreezeWalking()
    {
        // Set isFreezing to make sure the disable of PlayerController script only run once,
        // otherwise it may be stuck at this (!ifOverwriting) part
        if (this.isFreezing)
        {
            this.GetComponent<PlayerController>().enabled = false;
            Debug.Log("not on ground Disable");
        }
        this.GetComponent<PlayerController>().SetAcceleration(0f);
        this.GetComponent<PlayerController>().SetMoveClamp(0f);
        this.GetComponent<PlayerController>().enabled = true;
        this.isFreezing = false;
    }

    private void RecoverWalking()
    {
        this.GetComponent<PlayerController>().enabled = false;
        Debug.Log("on ground Disable");
        this.GetComponent<PlayerController>().SetAcceleration(this.savedAccel);
        this.GetComponent<PlayerController>().SetMoveClamp(this.savedMoveClamp);
        this.GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator RecoverWalkingCoroutine()
    {
        // wait for 1 second, then run RecoverWalking() function
        yield return new WaitForSeconds(1);
        RecoverWalking();
    }
}
