using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPan : MonoBehaviour
{
    public float speed;
    public Transform checkObject;


    private bool doShift = false;

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position +Vector3.left, speed * Time.deltaTime);

        if (transform.position.x < 0 && doShift)
        {
            checkObject.transform.position += new Vector3(140, 0, 0);
            doShift = false;
        }

        if (this.transform.position.x > 60)
        {
            doShift = true;
        }
        
    }
}
