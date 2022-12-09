using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using Unity.Mathematics;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    //event to alert the player save point script to respawn the player at the respawn point
    public static event Action Respawn;

    //event to alert the player save point script that we need the position at which to respawn a friend
    public static event Action FrRespawn;
    public static event Action<bool> ReturnFriends;
    
    public static event Action<Boolean> StunThePlayer;
    public static event Action<GameObject> StunTheFriend;
    
    


    //the player
    public GameObject Player;

    //rigidbody of the player
    private Rigidbody2D rb;
    private Rigidbody2D frb;



    //this is the friend getting respawned but obviously can't call it "Friend" because that's its own class
    private GameObject Fwiend;

    //private float defAccel;
    //private float accelBump = 15f;
    //private float defDeccel;
    //private float decelBump = -15f;

    private float electricTimer = 0;
    private PlayerController pc;

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
        
         pc = Player.GetComponent<PlayerController>();

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
                ReturnFriends(true);
            }

            if (ThisHazard == HazardType.Electro)
            {
                if (electricTimer <= 0)
                {
                    //rb.AddForce(pc.Velocity * -1.5f , ForceMode2D.Impulse);
                    AddForceZappers(pc.transform.position,pc.Velocity ,rb);

                    //pc.SetVelocity(pc.Velocity * -.5f);
                    electricTimer = .5f;
                    StunPlayer();

                }
                
            }
        }

        if (collision.gameObject.CompareTag("Friend"))
        {
            Fwiend = collision.gameObject;
            //when the friend hits the lava they can respawn
            //I am hoping to think of a more efficient way to do this
            if (ThisHazard == HazardType.Lava)
            {
                Debug.Log("friend lava");
                //save the fwiend because we need to change its position in a second
                
                //place a call to the save point controller to get our respawn spot
                FrRespawn?.Invoke();
            }
            
            if (ThisHazard == HazardType.Electro)
            {
                if (electricTimer <= 0)
                {
                    frb = Fwiend.GetComponent<Rigidbody2D>();
                    
                    AddForceZappers(Fwiend.transform.position, frb.velocity, frb);
                    //frb.AddForce(frb.velocity * -2f , ForceMode2D.Impulse);
                    //pc.SetVelocity(pc.Velocity * -.5f);
                    //electricTimer = .5f;
                    StunTheFriend(Fwiend);

                }
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // if (ThisHazard == HazardType.Electro) 
        // {
        //     if (collision.gameObject.CompareTag("Player"))
        //     {
        //         //Vector2 impulse = rb.velocity * -20;
        //         Vector2 impulse = new (-5, 2);
        //         rb.AddForce(impulse, ForceMode2D.Impulse);
        //     }
        //
        //     //I'm not convinced this is actually working...
        //     //need to figure out a better way to test it
        //     if (collision.gameObject.CompareTag("Friend"))
        //     {
        //         Rigidbody2D frb = collision.gameObject.GetComponent<Rigidbody2D>();
        //         //Vector2 impulse = frb.velocity * -20;
        //         Vector2 impulse = new (-5, 2);
        //         frb.AddForce(impulse, ForceMode2D.Impulse);
        //     }
        // }
    }

    //function to respawn the player at the saved respawn point
    //and make its velocity zero so it doesn't go flying off lol
    private void FriendLava(Vector3 Respawn) 
    {
        Fwiend.transform.position = Respawn;
        Fwiend.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (ThisHazard == HazardType.Electro)
        {
            if (adjustGrav)
            {
                Vector3 v = transform.position - other.gameObject.transform.position;
                Debug.Log(v);
                rb.AddForce(-v, ForceMode2D.Impulse);
            }
            else if (other.gameObject == Fwiend)
            {
                Vector3 v = transform.position - other.gameObject.transform.position;
                Debug.Log(v);
                frb.AddForce(-v, ForceMode2D.Impulse);
            }
        }
    }

    private bool adjustGrav = false;
    private void LateUpdate()
    {
        if (electricTimer > 0)
        {
            electricTimer -= Time.deltaTime;
        }

        if (adjustGrav)
        {
            if (rb.gravityScale > 0)
            {
                rb.gravityScale -= Time.deltaTime * 10;

            }
            else
            {
                rb.gravityScale = 0;
                adjustGrav = false;
                rb.velocity = Vector3.zero;
                pc.enabled = true;
                

                StunThePlayer(false);

                //endStun Animation

            } 
        }
        

        
    }

    private void StunPlayer()
    {
        pc.SetMoveSpeed(0,0);
        //pc.SetVelocity(Vector3.zero);
        pc.enabled = false;
        rb.gravityScale = 10;
        adjustGrav = true;

        StunThePlayer(true);
        //activate stunned animation





        //pc.SetVelocity(Vector3.zero);
        //Debug.Log("enabled");
    }

    private void AddForceZappers(Vector3 colliderPos, Vector3 velocity, Rigidbody2D rb)
    {
        Vector2 vel = velocity;
        
        //calculate if we flip x or y
        Vector2 temp = colliderPos - this.transform.position;

        Debug.Log(vel);

        if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
        {
            Debug.Log("flip y");
            vel.y *= -1;
            vel.x += 2;
        }
        else
        {
            Debug.Log("flip x");
            vel.y += 2;

            vel.x *= -1;
        }
        // if (temp.y > 0)
        // {
        //     if ((temp.x) > (temp.y))
        //     {
        //     
        //         vel.x *= -1;
        //     }
        //     else
        //     {
        //         vel.y *= -1;
        //     }
        // }
        // else
        // {
        //     if ((temp.x) > (temp.y))
        //     {
        //     
        //         vel.y *= -1;
        //     }
        //     else
        //     {
        //         vel.x *= -1;
        //     }
        // }
        


        rb.AddForce(vel * -1.5f , ForceMode2D.Impulse);
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
