using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] private Transform cam;

    [SerializeField] private float relativeMovement;
    void Update()
    {
        transform.position = new Vector2(cam.position.x * relativeMovement, cam.position.y * relativeMovement);
    }
}
