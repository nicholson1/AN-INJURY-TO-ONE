using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] SpriteRenderer ArrowBase;
    [SerializeField] SpriteRenderer ArrowTip;

    [SerializeField] Color color;

    private void Update()
    {
        Color c = new Color(color.r, color.g, color.b, transform.localScale.x / 8);
        ArrowBase.color = c;
        ArrowTip.color = c;
     
    }

}
 