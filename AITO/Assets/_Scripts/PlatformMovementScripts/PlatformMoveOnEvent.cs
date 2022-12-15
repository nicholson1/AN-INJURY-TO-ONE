using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveOnEvent : MonoBehaviour
{
    //needs: array of waypoints, current destination
    public Transform[] Waypoints;
    public Transform c;
    private int finalIndex;
    public int curIndex = 0;
    private float minDist = 0.1f;

    public float speed = 0.4f;
    float step;
    public Vector3 startPos;

    public bool MoveToWaypoint = false;

    // Start is called before the first frame update
    void Start()
    {
        //this.enabled = false;
        step = speed;
        c = Waypoints[0];
        finalIndex = Waypoints.Length - 1;
        startPos = new Vector3(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (MoveToWaypoint)
        {
            //Debug.Log(Vector3.Distance(transform.position, c.position));
            if (Vector3.Distance(transform.position, c.position) <= minDist)
            {
                //Debug.Log("this happend");

                //if they are close enough, the destination iterates to the next one
                curIndex++;

                //checking to see if the current index is bigger than the number of waypoints
                if (curIndex > finalIndex)
                {
                    //and stopping the platform if it is
                    speed = 0;

                    //hopefully turning off this script??
                    
                }
                else
                {
                    //setting the destination to the current index
                    c = Waypoints[curIndex];
                }
                //moving to the destination
                //Debug.Log("moving");
                
            }
            transform.position = Vector2.MoveTowards(transform.position, c.position, step * Time.deltaTime);
        }
        else
        {
            //Debug.Log("reversing");

            if (Vector3.Distance(transform.position, startPos) >= minDist)
            {
                transform.position = Vector2.MoveTowards(transform.position, startPos, step  * Time.deltaTime);
            }
        }
        
    }
}
