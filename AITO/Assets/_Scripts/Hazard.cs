using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    //for now hazards only affect the player because the friends have no colliders when they are following

    //public static event Action<HazardType, Transform> PlHazardHit;

    public static event Action<HazardType, Transform, GameObject> FrHazardHit;

    public static event Action Respawn;

    public Rigidbody2D Player;

    //Hazard effects
    /*private Rigidbody2D rb;
    private float defAccel;
    private float accelBump = 15f;
    private float defDeccel;
    private float decelBump = -15f;*/

    public enum HazardType 
    {
        Lava,
        Oil,
        Electro
    }

    private void Start()
    {
        //getting the rigidbody so we can add force
        //rb = Player.GetComponent<Rigidbody2D>();

        //saving the player's default acceleration so we can get back to it after the oil slick
        //defAccel = _acceleration;
        //defDeccel = _deAcceleration;
    }

    public HazardType ThisHazard;

    //I haven't yet begun restructuring the friends' reactions to hazards, still figuring that out
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            //string tag = gameObject.tag;
            //PlHazardHit?.Invoke(ThisHazard, RespawnPoints[CurrentRespawn].transform);
            //Debug.Log(RespawnPoints[CurrentRespawn].transform.position);
            HazardReact(ThisHazard);
        }

        if (collision.gameObject.CompareTag("Friend"))
        {
            //FrHazardHit?.Invoke(ThisHazard, RespawnPoints[CurrentRespawn].transform, collision.gameObject);
        }
    }


    private void OnDestroy()
    {
        //unsub from hazards
        //Hazard.PlHazardHit -= HazardReact;
    }

    //function that sees what hazard was hit and what should happen because of it
    //these all now work, but they need some tuning
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
                StartCoroutine(OilEffect());
                break;
            case HazardType.Electro:
                Debug.Log("player zap");
                //StartCoroutine(ElectroEffect());
                ElectroEffect();
                break;
        }
    }

    //when the player hits the oil slick, their acceleration goes up and deceleration goes down
    //and then after a couple non-oily seconds it goes back down and decel goes back up
    private IEnumerator OilEffect()
    {
        //_acceleration += accelBump;
        //_deAcceleration += decelBump;
        yield return new WaitForSeconds(5f);
        //_acceleration = defAccel;
        //_deAcceleration = defDeccel;
    }

    private void ElectroEffect() 
    {
        //multiplying the velocity by -2 hasn't been working??
        //will continue experimenting
        Player.AddForce(Player.transform.right * -2f, ForceMode2D.Impulse);
        Debug.Log("this happened " + Player.velocity);
    }
}
