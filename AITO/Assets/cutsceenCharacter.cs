using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cutsceenCharacter : SceneEndTriggerController
{

    public Transform buttonLocation;

    public Transform LeaveLocation;
    private Vector3 target;
    private Vector3 startPos;

    private bool leave = false;
    public float speed = 2;

    private bool goStart = false;
    
    public static event Action playerDoneTalk1;


    public override void ReturnLog()
    {
        Debug.Log("cutscene Level End Observered");
    }

    public override int ReturnSceneIndex()
    {
        return 1;
    }

    private void Start()
    {
        startPos = transform.position;
        target = buttonLocation.position;

        bosscutsceene.bossDoneTalk1 += talk1;
        bosscutsceene.bossDoneTalk2 += talk2;

    }

    private void OnDestroy()
    {
        bosscutsceene.bossDoneTalk1 -= talk1;
        bosscutsceene.bossDoneTalk2 += talk2;


    }

    private bool changeScene = false;
    private void LateUpdate()
    {
        if (!leave)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < .1f)
            {
                if (goStart)
                {
                    target = buttonLocation.position;
                    goStart = false;
                }
                else
                {
                    target = startPos;
                    goStart = true;
                }

                if (changeScene)
                {
                    ChangeTheScene();
                }
            }
        }
    }

    public GameObject playertalk1;
    public GameObject playertalk2;
    public GameObject playertalk3;


    private void talk1()
    {
        speed = 0;
        target = LeaveLocation.position;
        StartCoroutine(playertalkinit());


    }
    private void talk2()
    {
        speed = 0;
        target = LeaveLocation.position;
        StartCoroutine(playertalkinit2());


    }
    
    private IEnumerator playertalkinit2()
    {
        playertalk3.SetActive(true);
        yield return new WaitForSeconds(5);
        playertalk3.SetActive(false);
        speed = 5;
        changeScene = true;
    }
    
    private IEnumerator playertalkinit()
    {
        playertalk1.SetActive(true);
        yield return new WaitForSeconds(5);
        playertalk1.SetActive(false);
        playertalk2.SetActive(true);
        yield return new WaitForSeconds(5);
        playertalk2.SetActive(false);
        playerDoneTalk1();
    }

    private void ChangeTheScene()
    {
        SceneManager.LoadScene(1);
    }
}
