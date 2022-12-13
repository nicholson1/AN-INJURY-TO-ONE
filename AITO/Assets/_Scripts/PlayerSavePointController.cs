using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;
using System;

public class PlayerSavePointController : MonoBehaviour
{
    // get the sprite for character to ask it run Flash() function
    public GameObject playerSprite;
    
    // save the _acceleration and _moveClamp value in PlayerController.sc > #region Walk,
    // and _jumpHeight in #region Jump
    // modify them to 0 can make player cannot move nor jump
    private float savedAccel;
    private float savedMoveClamp;
    private float savedJumpHeight;
    // these three boolean values are used to check the player's status,
    // players cannot move after falling into DeathArea until they are on the ground and wait for a specific time period
    private bool isOnGround = false;
    private bool isOverwriting = false;
    private bool isFreezing = false;
    // duration of freezing movement time after player respawn
    private float freezingDuration = 1f;
    // check whether player is getting through a horizontal camera transition trigger
    private bool getThroughHorizontal = false;

    // singleton pattern only have one static instance, initiated at the beginning in SaveLoadController.cs,
    // write this line for updating the variables inside
    SingletonController instance;

    //HL: event to provide respawn point to friends if they fall into lava
    public static event Action<Vector3> FriendRespawn;

    // Start is called before the first frame update
    void Start()
    {
        instance = SingletonController.Instance;

        // save the born position of player at the beginning of game scene
        Debug.Log("Player Information Saved");
        instance.UpdateSavedPlayerPosition(this.transform.position);

        this.savedAccel = this.GetComponent<PlayerController>().ReturnAcceleration();
        this.savedMoveClamp = this.GetComponent<PlayerController>().ReturnMoveClamp();
        this.savedJumpHeight = this.GetComponent<PlayerController>().ReturnJumpHeight();

        //HL: subscribing to hazard events which respawn the player and friends when they fall into lava
        Hazard.Respawn += OverwritePlayerPosition;
        Hazard.FrRespawn += SendRespawnPosition;
        CameraMove.CameraDoneMoving += OnNotifyCameraMoveEnds;
        GameManager.MoveCamera += OnNotifyCameraMoveStarts;

        // JT: as an observer of camera moves
        //foreach (GameManager gm in FindObjectsOfType<GameManager>())
        //{
        //    gm.AddObverserOfCameraMoveStarts(this);
        //}

        //foreach (CameraMove cm in FindObjectsOfType<CameraMove>())
        //{
        //    cm.AddObverserOfCameraMoveEnds(this);
        //}
    }

    //HL: unsubscribing from the hazard event
    private void OnDestroy()
    {
        Hazard.Respawn -= OverwritePlayerPosition;
        Hazard.FrRespawn -= SendRespawnPosition;
        CameraMove.CameraDoneMoving -= OnNotifyCameraMoveEnds;
        GameManager.MoveCamera -= OnNotifyCameraMoveStarts;
    }

    // Update is called once per frame
    void Update()
    {
        // respawn will overwrite play's position,
        // this process will freeze the player's movement until play is on walkable areas and wait for specific period of time
        if (isOverwriting)
        {
            if (!this.isOnGround)
            {
                FreezeWalking();
            }
            else
            {
                Debug.Log("begin recover movement");
                playerSprite.GetComponent<FlashEffect>().Flash(freezingDuration);

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

            // check whether player gets through a horizonral camera transition trigger
            if (collision.gameObject.GetComponent<PuzzleTransitionVertical>() != null)
            {
                this.getThroughHorizontal = true;
            }
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

        if (collision.gameObject.tag == "Platform")
        {
            this.isOnGround = true;
            this.transform.SetParent(collision.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WalkableArea")
        {
            this.isOnGround = false;
        }

        if (collision.gameObject.tag == "Platform")
        {
            this.isOnGround = false;
            this.transform.SetParent(null);
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
        
        //this.GetComponent<PlayerController>().SetMoveClamp(0f);

        // if player gets through a horizonral camera transition trigger, don't set MoveClamp to 0,
        // otherwise it will fall down directly
        if (!this.getThroughHorizontal)
        {
            this.GetComponent<PlayerController>().SetMoveClamp(0f);
        }
        else
        {
            this.GetComponent<PlayerController>().SetMoveClamp(this.savedMoveClamp);
        }
        this.getThroughHorizontal = false;

        this.GetComponent<PlayerController>().SetJumpHeight(0f);
        this.GetComponent<PlayerController>().enabled = true;
        this.isFreezing = false;
        Debug.Log("is freezing");
    }

    private void RecoverWalking()
    {
        this.GetComponent<PlayerController>().enabled = false;
        Debug.Log("on ground Disable");
        this.GetComponent<PlayerController>().SetAcceleration(this.savedAccel);
        this.GetComponent<PlayerController>().SetMoveClamp(this.savedMoveClamp);
        this.GetComponent<PlayerController>().SetJumpHeight(this.savedJumpHeight);
        this.GetComponent<PlayerController>().enabled = true;
    }

    IEnumerator RecoverWalkingCoroutine()
    {
        // wait for 1 second, then run RecoverWalking() function
        yield return new WaitForSeconds(freezingDuration);
        RecoverWalking();
    }

    //HL: function that provides respawn coordinates to the hazard script
    private void SendRespawnPosition(GameObject f)
    {
        f.transform.position = instance.ReturnSavedPlayerPosition();
        
        f.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    // JT: as an observer of camera moves
    public void OnNotifyCameraMoveStarts(Vector3 cameraPos)
    {
        FreezeWalking();
    }

    public void OnNotifyCameraMoveEnds()
    {
        Debug.Log("camera done moving");
        RecoverWalking();
        instance.UpdateSavedPlayerPosition(this.transform.position);
    }
}
