using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartEyes : MonoBehaviour
{
    [SerializeField] private Sprite heartEyeSprite;
    private Sprite original;
    private SpriteRenderer sp;

    private void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();
        original = sp.sprite;
        
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

    IEnumerator Eyes()
    {
        sp.sprite = heartEyeSprite;
        yield return new WaitForSeconds(2);
        sp.sprite = original;
    }
}
