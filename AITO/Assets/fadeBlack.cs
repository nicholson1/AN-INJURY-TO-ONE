using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeBlack : MonoBehaviour
{
    private Image i;
    private void Start()
    {
        i = GetComponent<Image>();
    }

    private void Update()
    {
        Color c = i.color;
        c += new Color(0, 0, 0, .5f) * Time.deltaTime;
        i.color = c;
    }
}
