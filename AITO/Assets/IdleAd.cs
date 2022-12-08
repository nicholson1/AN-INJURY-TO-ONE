using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAd : MonoBehaviour
{
    private void LateUpdate()
    {
        this.transform.localPosition = new Vector3(0,Mathf.PingPong(Time.time * 10, 20));
    }
}
