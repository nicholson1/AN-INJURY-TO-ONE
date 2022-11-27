using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveOnEvent : MonoBehaviour
{
    //needs: array of waypoints, current destination
    public Transform[] Waypoints;
    private Transform c;
    private int finalIndex;
    private int curIndex = 0;
    private float minDist = 0.1f;

    private float speed = 0.1f;
    float step;

    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        step = speed * Time.deltaTime;
        c = Waypoints[0];
        finalIndex = Waypoints.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(transform.position, c.position));
        if (Vector3.Distance(transform.position, c.position) <= minDist)
        {
            //if they are close enough, the destination iterates to the next one
            curIndex++;

            //checking to see if the current index is bigger than the number of waypoints
            if (curIndex > finalIndex)
            {
                //and stopping the platform if it is
                speed = 0;
                //hopefully turning off this script??
                this.enabled = false;
            }
            else
            {
                //setting the destination to the current index
                c = Waypoints[curIndex];
            }
        }
        //moving to the destination
        transform.position = Vector2.MoveTowards(transform.position, c.position, step);
    }
}