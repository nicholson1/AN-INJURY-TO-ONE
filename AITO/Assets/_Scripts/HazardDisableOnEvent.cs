using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDisableOnEvent : MonoBehaviour
{
    public bool turnOff = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (turnOff)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
