using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] private Transform camera;

    [SerializeField] private float relativeMovement;
    void Update()
    {
        transform.position = new Vector2(camera.position.x * relativeMovement, camera.position.y * relativeMovement);
    }
}
