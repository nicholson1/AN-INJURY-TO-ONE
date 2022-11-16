using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    public Transform FollowTarget;
    private bool isFollowing;

    [SerializeField] private float followDistance;
    [SerializeField] private float moveSpeed;

    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        PlayerFriendControl.CollectFriend += CollectedTrigger;
        PlayerFriendControl.ThrowFriend += Throw;

        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        animator.SetBool("Grounded", true);
        animator.SetFloat("IdleSpeed", 1);
    }

    private void OnDestroy()
    {
        PlayerFriendControl.CollectFriend -= CollectedTrigger;
        PlayerFriendControl.ThrowFriend -= Throw;


    }

    private void Update()
    {
        if (isFollowing)
        {
            DoFollow();
        }
    }
    
    private void DoFollow()
    {
        if (Vector2.Distance(FollowTarget.position, transform.position) > followDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, FollowTarget.position,
                moveSpeed * Time.deltaTime);
        }
       
    }

    private void CollectedTrigger(Friend friend, Transform follow)
    {
        if (friend == this)
        {
            GetComponent<Collider2D>().enabled = false;
            rb.Sleep();

            FollowTarget = follow;
            isFollowing = true;
        }
    }

    private void Throw(Friend f, float power, Vector2 dir)
    {
        
        if (f == this)
        {
            // I am being yeeted
            GetComponent<Collider2D>().enabled = true;
            rb.WakeUp();
            isFollowing = false;
            FollowTarget = null;

            
            rb.AddForce(dir * power, ForceMode2D.Impulse);
            StartCoroutine(ThrowAnimation());

        }
        else
        {
            if (isFollowing)
            {
                StartCoroutine(ThrowAnimation());
            }
        }
        

        
    }

    public IEnumerator ThrowAnimation()
    {
        animator.SetTrigger("Jump");
        //yield return new WaitForSeconds(1);
        animator.SetBool("Grounded", true);
        yield return null;
          
      

    }


}
