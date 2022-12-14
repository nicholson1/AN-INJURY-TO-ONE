using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerFriendControl : MonoBehaviour
{
    public static event Action<Friend,Transform> CollectFriend;
    public static event Action<Friend, float, Vector2> ThrowFriend;


    private Stack<Friend> friends = new Stack<Friend>();

    private List<Friend> collectedFriends = new List<Friend>();


    [SerializeField] private GameObject pointer;

    private void Start()
    {
        PuzzleTransitions.TransitionRight += GetFriends;
        Hazard.ReturnFriends += GetFriends;

    }
    private void OnDestroy()
    {
        PuzzleTransitions.TransitionRight -= GetFriends;
        Hazard.ReturnFriends -= GetFriends;


    }

    private void GetFriends(bool i = true)
    {
        foreach (Friend f in collectedFriends)
        {
            if (f.FollowTarget == null)
            {
                CollectFriend(f, GetFollowTarget());
                if (friends.Count == 0)
                {
                    f.followDistance = 1.5f;
                }
                else
                {
                    f.followDistance = 1;
                }

                friends.Push(f);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Friend"))
        {

            this.GetComponent<Rigidbody2D>().velocity = Vector3.zero; 
            Friend f = collision.gameObject.GetComponent<Friend>();

            if (f.FollowTarget == null)
            {
                //figure out which gameObject to follow
                CollectFriend(f, GetFollowTarget());
                if (friends.Count == 0)
                {
                    f.followDistance = 1.5f;
                }
                else
                {
                    f.followDistance = 1;
                }

                friends.Push(f);
                collectedFriends.Add(f);
            }
        }
    }

    private Transform GetFollowTarget()
    {
       
        if (friends.Count == 0)
        {
            return this.transform;
        }
        else
        {
            return friends.Peek().transform;
        }
    }

    private float power = 0;
    private bool mouseDown = false;

    private float bounceTimer = 0;

    private void Update()
    {
        if (mouseDown)
        {
            if (power < 4)
            {
                power += Time.deltaTime * 3f;

                //if (power is an integer)
                
            
            }
            pointer.transform.position = friends.Peek().transform.position;

            bounceTimer -= Time.deltaTime;
            if (bounceTimer <= 0)
            {
                StartCoroutine(friends.Peek().ThrowAnimation());
                bounceTimer = 1.5f;
            }

            var _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint((friends.Peek().transform.position));
            var _angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
            pointer.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            
            pointer.transform.localScale = new Vector3(power, power/2 , 1);
        }
        
        if (friends.Count > 0 && Input.GetMouseButtonDown(0))
        {

            
            mouseDown = true;
            pointer.transform.localScale = Vector3.zero;
            pointer.SetActive(true);
        }

        if (mouseDown && Input.GetMouseButtonUp(0))
        {
            ThrowMyFriend(power);

            //.Log(power);
            power = 0;
            mouseDown = false;
            pointer.SetActive(false);




        }
    }

    private void ThrowMyFriend(float p)
    {
        var _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint((friends.Peek().transform.position));
        Friend f = friends.Pop();
        

        ThrowFriend(f, p/25, _mousePos);
    }

    
}
