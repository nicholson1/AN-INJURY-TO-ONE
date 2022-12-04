using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedAnimation : MonoBehaviour
{
    [SerializeField] private GameObject animation;
    void Start()
    {
        Hazard.StunThePlayer += ShowAnimation;
    }

    private void ShowAnimation(bool show)
    {
        
        animation.SetActive(show);
        
    }
}
