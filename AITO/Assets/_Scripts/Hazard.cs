using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    //event to alert the player save point script to respawn the player at the respawn point
    public static event Action Respawn;

    //event to alert the player save point script that we need the position at which to respawn a friend
    public static event Action FrRespawn;

    //the player
    public GameObject Player;

    //rigidbody of the player
    private Rigidbody2D rb;

    //this is the friend getting respawned but obviously can't call it "Friend" because that's its own class
    private GameObject Fwiend;

    //private float defAccel;
    //private float accelBump = 15f;
    //private float defDeccel;
    //private float decelBump = -15f;

    //types of hazards
    public enum HazardType 
    {
        Lava,
        Oil,
        Electro
    }

    private void Start()
    {
        //finding the player
        Player = GameObject.FindGameObjectWithTag("Player");
        //getting the rigidbody so we can add force
        rb = Player.GetComponent<Rigidbody2D>();

        //saving the player's default acceleration so we can get back to it after the oil slick
        //defAccel = _acceleration;
        //defDeccel = _deAcceleration;

        PlayerSavePointController.FriendRespawn += FriendLava;
    }

    //variable to hold what type of hazard the current hazard is
    public HazardType ThisHazard;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            //when the player hits the lava they respawn
            if (ThisHazard == HazardType.Lava) 
            {
                Debug.Log("player lava");
                //calls to JT's player save system to respawn the player when they fall into the lava
                Respawn?.Invoke();
            }
        }

        if (collision.gameObject.CompareTag("Friend"))
        {
            //when the friend hits the lava they can respawn
            //I am hoping to think of a more efficient way to do this
            if (ThisHazard == HazardType.Lava)
            {
                Debug.Log("friend lava");
                //save the fwiend because we need to change its position in a second
                Fwiend = collision.gameObject;
                //place a call to the save point controller to get our respawn spot
                FrRespawn?.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ThisHazard == HazardType.Electro) 
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //Vector2 impulse = rb.velocity * -20;
                Vector2 impulse = new (-5, 2);
                rb.AddForce(impulse, ForceMode2D.Impulse);
            }

            //I'm not convinced this is actually working...
            //need to figure out a better way to test it
            if (collision.gameObject.CompareTag("Friend"))
            {
                Rigidbody2D frb = collision.gameObject.GetComponent<Rigidbody2D>();
                //Vector2 impulse = frb.velocity * -20;
                Vector2 impulse = new (-5, 2);
                frb.AddForce(impulse, ForceMode2D.Impulse);
            }
        }
    }

    //function to respawn the player at the saved respawn point
    //and make its velocity zero so it doesn't go flying off lol
    private void FriendLava(Vector3 Respawn) 
    {
        Fwiend.transform.position = Respawn;
        Fwiend.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    //oil currently turned off
    /*private IEnumerator OilEffect()
    {
        //_acceleration += accelBump;
        //_deAcceleration += decelBump;
        yield return new WaitForSeconds(5f);
        //_acceleration = defAccel;
        //_deAcceleration = defDeccel;
    }*/

    //oil is turned off for now
    /*private IEnumerator FriendOilEffect()
    {
        //moveSpeed = +speedBoost;
        //Debug.Log("friend speed " + moveSpeed);
        yield return new WaitForSeconds(5f);
        //moveSpeed = defSpeed;
        //Debug.Log("friend speed " + moveSpeed);
    }*/

}
