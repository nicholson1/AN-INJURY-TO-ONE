using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossfinal : MonoBehaviour
{

    public static event Action bossDoneTalk;

    public GameObject bosscanvas1;
    public GameObject bosscanvas2;

    
    public void finalTalk()
    {
        StartCoroutine(bosstalk2());
    }
    private IEnumerator bosstalk2()
    {
        bosscanvas1.SetActive(true);
        yield return new WaitForSeconds(5);
        bosscanvas1.SetActive(false);
        
        bossDoneTalk();
    }

    

    

    public void GetHit()
    {
        bosscanvas2.SetActive(true);

        GetComponent<Rigidbody2D>().AddForceAtPosition( new Vector2(5, -3) * 20, transform.position + new Vector3(0,2,0));
    }
}
