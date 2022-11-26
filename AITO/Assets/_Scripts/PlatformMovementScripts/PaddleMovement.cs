using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float PaddleSpeed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && transform.position.y < 10)
        {
            transform.Translate(new Vector3(0, PaddleSpeed / 10, 0));
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y > -10)
        {
            transform.Translate(new Vector3(0, -PaddleSpeed / 10, 0));
        }
    }
}
