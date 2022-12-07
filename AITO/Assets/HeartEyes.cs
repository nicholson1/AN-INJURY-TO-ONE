using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeartEyes : MonoBehaviour
{
    [SerializeField] private Sprite heartEyeSprite;
    [SerializeField] private Sprite feet;
    [SerializeField] private Sprite move;
    private SpriteRenderer sp;

    private bool idle;
    private float timer;
    private void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();

        PlayerFriendControl.CollectFriend += ShowHeartEyes;

    }

    private void OnDestroy()
    {
        PlayerFriendControl.CollectFriend -= ShowHeartEyes;
    }

    private void ShowHeartEyes(Friend f, Transform t )
    {
        if (f == this.GetComponentInParent<Friend>())
        {
            StartCoroutine(Eyes());
        }

        if (this.transform.parent.parent.CompareTag("Player"))
        {
            StartCoroutine(Eyes());
        }
        
    }

    
    private void LateUpdate()
    {
        Transform t = this.transform;
        if (!idle)
        {
            if (t.rotation.z == 0 && t.localScale.x == 1f)
            {
                timer += Time.deltaTime;

                if (timer > 1.5f)
                {
                    idle = true;
                    sp.sprite = feet;
                }
                
            }
        }
        else
        {
            if (t.rotation.z != 0 || t.localScale.x != 1f)
            {
                idle = false;
                sp.sprite = move;
                timer = 0;

            }
        }
        
    }

    IEnumerator Eyes()
    {
        sp.sprite = heartEyeSprite;
        yield return new WaitForSeconds(2);
        sp.sprite = move;
    }
}
