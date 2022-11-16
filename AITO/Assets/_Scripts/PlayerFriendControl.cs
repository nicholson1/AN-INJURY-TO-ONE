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
    

    [SerializeField] private GameObject pointer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Friend"))
        {
            
            Friend f = collision.gameObject.GetComponent<Friend>();

            if (f.FollowTarget == null)
            {
                //figure out which gameObject to follow
                CollectFriend(f, GetFollowTarget());
                friends.Push(f);
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

    private void Update()
    {
        if (mouseDown)
        {
            if (power < 4)
            {
                power += Time.deltaTime * 3f;

                //if (power is an integer)
                if((float) Mathf.RoundToInt(power) == power)
                {
                    friends.Peek().GetComponentInChildren<Animator>().SetTrigger("Jump");
                }
            }
            
            var _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint((transform.position));
            var _angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
            pointer.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            
            pointer.transform.localScale = new Vector3(power, power/2, 1);
        }
        
        if (friends.Count > 0 && Input.GetMouseButtonDown(0))
        {
            

            mouseDown = true;
            pointer.SetActive(true);
        }

        if (mouseDown && Input.GetMouseButtonUp(0))
        {
            ThrowMyFriend(power);

            Debug.Log(power);
            power = 0;
            mouseDown = false;
            pointer.SetActive(false);




        }
    }

    private void ThrowMyFriend(float p)
    {
        var _mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint((transform.position));
        ThrowFriend(friends.Pop(), p/50, _mousePos - transform.position);
    }

    
}
