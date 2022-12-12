using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNew : MonoBehaviour
{
    public Vector2 MoveDirection;

    public float Speed;
    public BulletPooler pooler;
    //public fl

    private Transform t;
    private void Start()
    {
        t = this.GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector2 target = new Vector2(t.localPosition.x + MoveDirection.x, t.localPosition.y + MoveDirection.y);
        t.localPosition = Vector2.MoveTowards(transform.localPosition, target, Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            pooler.DisableBullet(this);
        }
    }

    
}
