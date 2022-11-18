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

    private float defSpeed;
    private void Start()
    {
        PlayerFriendControl.CollectFriend += CollectedTrigger;
        PlayerFriendControl.ThrowFriend += Throw;

        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        animator.SetBool("Grounded", true);
        animator.SetFloat("IdleSpeed", 1);

        //subscribe to hazards
        Hazard.FrHazardHit += HazardReact;
        //hang onto the default speed of the friends
        defSpeed = moveSpeed;
    }

    private void OnDestroy()
    {
        PlayerFriendControl.CollectFriend -= CollectedTrigger;
        PlayerFriendControl.ThrowFriend -= Throw;

        //unsubscribe from hazards
        Hazard.FrHazardHit -= HazardReact;
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


    //nightmare nightmare nightmare
    //also difficult to test right now without the friends walking back to the player
    //also having one launched friend hit the hazards causes unbelievably fucked up things to happen to the friends still following the player
    //wondering if this whole concept needs to be reworked for the friends because of their varying collider statuses...
    private void HazardReact(Hazard.HazardType Haz, Transform respawn)
    {
        if (!isFollowing) 
        {

            switch (Haz)
            {
                case Hazard.HazardType.Lava:
                    Debug.Log("friend lava");
                    LavaEffect(respawn);
                    break;
                case Hazard.HazardType.Oil:
                    Debug.Log("friend oil");
                    StartCoroutine(OilEffect());
                    break;
                case Hazard.HazardType.Electro:
                    Debug.Log("friend zap");
                    StartCoroutine(ElectroEffect());
                    break;
            }
        }
    }

    //neither oil nor electricity work the way I want them to on the friends
    //maybe this is just a tuning issue?
    private IEnumerator OilEffect()
    {
        //_acceleration += accelBump;
        moveSpeed += 20f;
        Debug.Log("friend speed " + moveSpeed);
        yield return new WaitForSeconds(5f);
        //_acceleration = defAccel;
        moveSpeed = defSpeed;
        Debug.Log("friend speed " + moveSpeed);
    }

    private IEnumerator ElectroEffect()
    {
        rb.AddForce(new Vector2(-10, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);
        rb.velocity = new Vector2(0,0);
    }

    private void LavaEffect(Transform rPoint)
    {
        //transform.position = rPoint.position;
        Debug.Log("right now lava punts you into the infinite");
    }
}
