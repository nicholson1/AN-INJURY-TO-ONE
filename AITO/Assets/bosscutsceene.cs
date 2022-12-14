using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bosscutsceene : MonoBehaviour
{

    public Transform buttonLocation;

    public Transform LeaveLocation;
    private Vector3 target;
    private Vector3 startPos;

    private bool leave = false;
    public float speed = 2;

    private bool goStart = false;

    public GameObject bosscanvas1;
    public GameObject bosscanvas2;


    public static event Action bossDoneTalk1;
    public static event Action bossDoneTalk2;

    private void Start()
    {
        startPos = transform.position;
        target = buttonLocation.position;
        cutsceenCharacter.playerDoneTalk1 += talk2;
    }

    private void OnDestroy()
    {
        cutsceenCharacter.playerDoneTalk1 -= talk2;

    }

    private void talk2()
    {
        StartCoroutine(bosstalk2());
    }
    private void LateUpdate()
    {
        if (!leave)
        {
           
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < .1f)
            {
                speed = 0;
                StartCoroutine(bosstalk());
                leave = true;
            }
        }
    }

    private IEnumerator bosstalk()
    {
        bosscanvas1.SetActive(true);
        yield return new WaitForSeconds(5);
        bosscanvas1.SetActive(false);
        bossDoneTalk1();
    }
    private IEnumerator bosstalk2()
    {
        bosscanvas2.SetActive(true);
        yield return new WaitForSeconds(5);
        bosscanvas2.SetActive(false);
        speed = 3;
        target = LeaveLocation.position;
        leave = false;
        bossDoneTalk2();
    }
    
    


}
