using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAd : MonoBehaviour
{
    public float moveSpeed = 10;
    public float moveDist = 20;

    private float initY;

    private void Start()
    {
        initY = transform.localPosition.y;
    }

    private void LateUpdate()
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x,initY + Mathf.PingPong(Time.time * moveSpeed, moveDist), this.transform.localPosition.z);
    }
}
