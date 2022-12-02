using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public static event Action Respawn;

    public GameObject Player;

    //Hazard effects
    private Rigidbody2D rb;
    private Rigidbody2D frb;
    //private float defAccel;
    //private float accelBump = 15f;
    //private float defDeccel;
    //private float decelBump = -15f;

    public enum HazardType 
    {
        Lava,
        Oil,
        Electro
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        //getting the rigidbody so we can add force
        rb = Player.GetComponent<Rigidbody2D>();

        //saving the player's default acceleration so we can get back to it after the oil slick
        //defAccel = _acceleration;
        //defDeccel = _deAcceleration;
    }

    private void OnDestroy()
    {
        //unsub from hazards
        //Hazard.PlHazardHit -= HazardReact;
    }

    public HazardType ThisHazard;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            HazardReact(ThisHazard);
        }

        if (collision.gameObject.CompareTag("Friend"))
        {
            frb = collision.gameObject.GetComponent<Rigidbody2D>();
            FriendHazardReact(ThisHazard);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ThisHazard == HazardType.Electro) 
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //rb.velocity *= -2f;
                rb.AddForce(rb.transform.right * -2f, ForceMode2D.Impulse);
            }

            if (collision.gameObject.CompareTag("Friend"))
            {
                //frb.velocity *= -2f;
                frb.AddForce(frb.transform.right * -2f, ForceMode2D.Impulse);
            }
        }
    }

    private void HazardReact(HazardType Haz)
    {
        switch (Haz)
        {
            case HazardType.Lava:
                Debug.Log("player lava");
                //calls to JT's player save system to respawn the player when they fall into the lava
                Respawn?.Invoke();
                break;
            case HazardType.Oil:
                Debug.Log("player oil");
                //StartCoroutine(OilEffect());
                break;
            case HazardType.Electro:
                Debug.Log("player zap");
                //StartCoroutine(ElectroEffect());
                //ElectroEffect();
                break;
        }
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

    /*private void ElectroEffect() 
    {
        //multiplying the velocity by -2 hasn't been working??
        //will continue experimenting
        rb.AddForce(rb.transform.right * -2f, ForceMode2D.Impulse);
        Debug.Log("this happened " + rb.velocity);
    }*/

    private void FriendHazardReact(HazardType Haz)
    {
        switch (Haz)
        {
            case HazardType.Lava:
                Debug.Log("friend lava");
                FriendLava();
                break;
            case HazardType.Oil:
                Debug.Log("friend oil");
                break;
            case HazardType.Electro:
                Debug.Log("friend zap");
                //FriendZap(FriendObj);
                break;
        }
    }

    //I really don't want to mess with the singleton and save point controllers because I don't quite understand how they work
    private void FriendLava() 
    {
        Debug.Log("Unclear how to integrate this with Jiachen's player respawn");
    }

    //oil is turned off for now
    /*private IEnumerator FriendOilEffect()
    {
        //moveSpeed = +speedBoost;
        //Debug.Log("friend speed " + moveSpeed);
        yield return new WaitForSeconds(5f);
        //moveSpeed = defSpeed;
        //Debug.Log("friend speed " + moveSpeed);
    }*/

    /*private void FriendZap(GameObject Friend) 
    {
        frb = Friend.GetComponent<Rigidbody2D>();
        frb.AddForce(frb.transform.right * -2f, ForceMode2D.Impulse);
        Debug.Log("this happened " + frb.velocity);
    }*/

}
