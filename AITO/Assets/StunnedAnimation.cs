using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class StunnedAnimation : MonoBehaviour
{
    [SerializeField] private GameObject animation;
    public bool isPlayer = true;
    
    void Start()
    {
        if (isPlayer)
        {
            Hazard.StunThePlayer += ShowAnimation;
        }
        else
        {
            Hazard.StunTheFriend += ShowFriendAnimation;
        }
    }

    private void OnDestroy()
    {
        if (isPlayer)
        {
            Hazard.StunThePlayer -= ShowAnimation;
        }
        else
        {
            Hazard.StunTheFriend -= ShowFriendAnimation;
        }
    }

    private bool electrofriend = false;
    private float timer;
    private void ShowAnimation(bool show)
    {
        
        animation.SetActive(show);
        
        
    }
    private void ShowFriendAnimation(bool show)
    {
        
        animation.SetActive(show);
        
        timer = 2f;
        electrofriend = true;
        
        
    }

    private void FixedUpdate()
    {
        if (!isPlayer && electrofriend)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                electrofriend = false;
                animation.SetActive(false);

            }
            
        }
    }
}
