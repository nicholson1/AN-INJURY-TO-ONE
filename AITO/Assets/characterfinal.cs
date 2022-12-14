using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterfinal : MonoBehaviour
{

    public SpriteRenderer[] srs;
    public Sprite[] hearteyes;
    
    public float speed = 0;
    public Transform target1;
    public Transform target2;

    public Transform target;

    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;

    public GameObject endcanvas;

    private bossfinal bf;
    private void Start()
    {
        bossfinal.bossDoneTalk += talk1;
        bf = FindObjectOfType<bossfinal>();
        target = target1;
    }
    private void OnDestroy()
    {
        bossfinal.bossDoneTalk -= talk1;
    }

    private void talk1()
    {
        StartCoroutine(talkone());
    }
    
    private IEnumerator talkone()
    {
        canvas1.SetActive(true);
        yield return new WaitForSeconds(5);
        speed = 5;
        target = target2;
        triggerOnce = false;
        canvas1.SetActive(false);
        canvas2.SetActive(true);
        yield return new WaitForSeconds(3);
        canvas2.SetActive(false);
        swapeyes();
        canvas3.SetActive(true);
        yield return new WaitForSeconds(3);
        endcanvas.SetActive(true);


        //trigger fade out, load menu scene


    }

    private void swapeyes()
    {
        for(int i = 0; i < srs.Length; i++)
        {
            srs[i].sprite = hearteyes[i];
        }
    }

    private bool triggerOnce = false;

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < .1f)
        {
            speed = 0;

            if (target == target1)
            {
                if (!triggerOnce)
                {
                    bf.finalTalk();
                    Debug.Log("bosstalk");
                    triggerOnce = true;
                }
                
            }
            else
            {
                if (!triggerOnce)
                {
                    bf.GetHit();
                    triggerOnce = true;
                }

            }
            // trigger heart eyes
            //activate 3rd canvas

            
        }
    }
}
