using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    // transform of the target for turrent to detect and track
    public Transform detectTarget;
    // if distance between turrent and target lower than this value, it will keep pointing to target
    public float trackTriggerDist = 3f;
    public float rotationModifier = 180f;
    // the rotation speed for turrent to track its target
    public float trackRotateSpeed = 10f;
    // the rotation speed for turrent to rotate when not tracking its target
    public float automateRotateSpeed = 10f;

    private float dist;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // the value range of localEulerAngles in unity2D is 0 to 360
        //Debug.Log(transform.localEulerAngles.z);

        // get the distance between tracking target and turrent
        dist = Vector3.Distance(transform.position, detectTarget.position);

        // if distance closer than that to trigger tracking, keep pointing to the target
        if (dist <= trackTriggerDist)
        {
            Vector3 vectorToTarget = detectTarget.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * trackRotateSpeed);

        }
        // otherwise, rotate between limited range of angles
        // can be modified according to default angle of turrent's sprite, the example used to test this script points to left by default
        else
        {
            // Mathf.PingPong(t, length) returns a value that will increase and decrease between 0 and length
            // used here to return a z-value between 0 and 180
            // "- 90" at the end is used to make the range modifies from "0 to 180" to "-90 to 90"
            transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time * automateRotateSpeed, 180) - 90);
        }
    }
}
