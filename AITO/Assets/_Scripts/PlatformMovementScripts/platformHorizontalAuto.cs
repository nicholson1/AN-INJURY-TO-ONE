using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformHorizontalAuto : MonoBehaviour
{
    //left and right offsets are the range of the platform
    [SerializeField] private float offsetLeft = 0, offsetRight = 0, speed = 1;
    //bools to check and see if we made it to the next position
    [SerializeField] bool hasReachedRight = false, hasReachedLeft = false;
    //saving the position we are starting at
    Vector3 startPosition = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hasReachedRight)
        {
            if (transform.position.x < startPosition.x + offsetRight)
            {
                // Move platform to the right
                Move(offsetRight);
            }
            else if (transform.position.x >= startPosition.x + offsetRight)
            {
                hasReachedRight = true;
                hasReachedLeft = false;
            }
        }
        else if (!hasReachedLeft)
        {
            if (transform.position.x > startPosition.x + offsetLeft)
            {
                // Move platform to the left
                Move(offsetLeft);
            }
            else if (transform.position.x <= startPosition.x + offsetLeft)
            {
                hasReachedRight = false;
                hasReachedLeft = true;
            }
        }
    }

    private void Move(float offset)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPosition.x + offset, transform.position.y, transform.position.z), speed * Time.deltaTime);
    }
}
