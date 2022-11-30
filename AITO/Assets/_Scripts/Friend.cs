using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    public Transform FollowTarget;
    private bool isFollowing;

    public float followDistance;
    public bool HasBeenCollected = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float interactRange;

    private Animator animator;
    private Rigidbody2D rb;

    [SerializeField] private LayerMask interactLayers;
    
    

    //remembering the default speed of the friend for hazard effects
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

    private void FixedUpdate()
    {
        if (CanFollow())
        {
            DoFollow();
            
        }
        else if (CanSeeInteract())
        {
            DoInteract();
        }
        
    }

    private bool CanSeeInteract()
    {
        return true;
    }

    private bool CanFollow()
    {
        return isFollowing;
    }
    
    private void DoFollow()
    {
        if (Vector2.Distance(FollowTarget.position, transform.position) > followDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, FollowTarget.position,
                moveSpeed * Time.deltaTime);
        }
       
    }

    private void DoInteract()
    {
        Collider2D[] _interactables =Physics2D.OverlapCircleAll(this.transform.position, interactRange, interactLayers);

        Transform target = CheckInteractables(_interactables);
        if (target != null)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed / 2);
        }

        // if checkinteractables isnt null
        // move to position


    }

    private Transform CheckInteractables(Collider2D[] interactables)
    {
        //check if in sight
        //check if being interacted with
        foreach (var col in interactables)
        {
            
            if (!col.GetComponent<Interactable>().isInteractable)
            {
                break;
            }
            else
            {
                //return first one that is
                return col.transform;
            }
        }
        
        return null;

    }

    private void CollectedTrigger(Friend friend, Transform follow)
    {
        if (friend == this)
        {
            GetComponent<Collider2D>().enabled = false;
            rb.Sleep();

            FollowTarget = follow;
            isFollowing = true;
            HasBeenCollected = true;
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


    //friend hazard behaviors
    private float speedBoost = 50f;
    private void HazardReact(Hazard.HazardType Haz, Transform respawn, GameObject FriendObj)
    {
        if (!isFollowing) 
        {
            if (FriendObj == this.gameObject) 
            {
                switch (Haz)
                {
                    case Hazard.HazardType.Lava:
                        Debug.Log("friend lava");
                        StartCoroutine(LavaRespawn(respawn));
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
    }

    //oil is very hard to test without any AI or movement on the friends
    private IEnumerator OilEffect()
    {
        moveSpeed =+ speedBoost;
        Debug.Log("friend speed " + moveSpeed);
        yield return new WaitForSeconds(5f);
        moveSpeed = defSpeed;
        Debug.Log("friend speed " + moveSpeed);
    }

    //this seems to work? But is hard to test without friend movement
    private IEnumerator ElectroEffect()
    {
        rb.AddForce(new Vector2(-30, 0), ForceMode2D.Impulse);
        Debug.Log("friend velocity " + rb.velocity);
        yield return new WaitForSeconds(2f);
        rb.velocity = new Vector2(0,0);
        Debug.Log("friend velocity " + rb.velocity);
    }

    //this works
    private IEnumerator LavaRespawn(Transform rPoint)
    {
        Debug.Log(rPoint.position);
        transform.position = Vector2.Lerp(transform.position, rPoint.position, 30f * Time.deltaTime);
        yield return new WaitForSeconds(5f);
    }

}
