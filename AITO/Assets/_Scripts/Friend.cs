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

    private Collider2D[] _interactables;

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
         _interactables = Physics2D.OverlapCircleAll(this.transform.position, interactRange, interactLayers);

        if (_interactables.Length > 0)
        {
            //Debug.Log("i can see interactable");
            return true;
            
        }

        return false;

    }

    private bool CanFollow()
    {
        return isFollowing;
    }
    
    private void DoFollow()
    {
        float dist = Vector2.Distance(FollowTarget.position, transform.position);
        if (dist > followDistance)
        {
            if (dist > 40)
            {
                transform.position = Vector2.MoveTowards(transform.position, FollowTarget.position,
                    moveSpeed * dist * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, FollowTarget.position,
                    moveSpeed * Time.deltaTime);
            }
            
        }
       
    }

    private void DoInteract()
    {

        Transform target = CheckInteractables(_interactables);
        //Debug.Log(target);
        
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
        //Debug.Log(interactables.Length);
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
}
