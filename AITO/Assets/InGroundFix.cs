using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class InGroundFix : MonoBehaviour
{
    public LayerMask groundLayer;

    public float dist = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.layer + collision.gameObject.name);
        if (collision.gameObject.layer == 6)
        {
            //timer += 1;
            //Debug.Log(timer);

        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, dist, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, Vector2.down);
        if (hit.collider)
        {
            //Debug.Log(hit.collider.name);
            transform.position = Vector3.MoveTowards(transform.position , transform.position + new Vector3(0, .5f, 0), 10 * Time.deltaTime);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        }
        
    }
}
