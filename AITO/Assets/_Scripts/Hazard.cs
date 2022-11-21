using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    //for now hazards only affect the player because the friends have no colliders when they are following

    public static event Action<HazardType, Transform> PlHazardHit;

    public static event Action<HazardType, Transform, GameObject> FrHazardHit;

    public GameObject[] RespawnPoints;

    private int CurrentRespawn = 0;

    public enum HazardType 
    {
        Lava,
        Oil,
        Electro
    }

    private void Start()
    {
        RespawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        PuzzleTransitions.Transition += UpdateRespawn;
    }

    public HazardType ThisHazard;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            PlHazardHit?.Invoke(ThisHazard, RespawnPoints[CurrentRespawn].transform);
            Debug.Log(RespawnPoints[CurrentRespawn].transform.position);
        }

        if (collision.gameObject.CompareTag("Friend"))
        {
            FrHazardHit?.Invoke(ThisHazard, RespawnPoints[CurrentRespawn].transform, collision.gameObject);
        }
    }

    private void UpdateRespawn() 
    {
        CurrentRespawn++;
    }
}
