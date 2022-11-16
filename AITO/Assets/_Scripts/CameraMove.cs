using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    private Vector3 targetPos;
    private bool isMoving = false;
    [SerializeField] private float MoveSpeed;

    private void Start()
    {
        GameManager.MoveCamera += MoveCamera;
        
    }
    private void OnDestroy()
    {
        GameManager.MoveCamera -= MoveCamera;

    }

    private void MoveCamera(Vector3 p)
    {
        targetPos = p;
        isMoving = true;

    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, targetPos) < .01f)
            {
                isMoving = false;
            }
        }
    }
}
