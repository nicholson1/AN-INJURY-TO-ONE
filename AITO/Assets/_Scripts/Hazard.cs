using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    //for now hazards only affect the player because the friends have no colliders when they are following

    public static event Action<HazardType> PlHazardHit;

    public enum HazardType 
    {
        Lava,
        Oil,
        Electro
    }

    public HazardType ThisHazard;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            PlHazardHit?.Invoke(ThisHazard);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
