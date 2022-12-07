using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformVerticalAuto : MonoBehaviour
{
    //up and down offsets are the range of the platform
    [SerializeField] private float offsetDown = 0, offsetUp = 0, speed = 1;
    //bools to check and see if we made it to the next position
    [SerializeField] bool hasReachedUp = false, hasReachedDown = false;
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
        if (!hasReachedUp)
        {
            if (transform.position.y < startPosition.y + offsetUp)
            {
                // Move platform to the right
                Move(offsetUp);
            }
            else if (transform.position.y >= startPosition.y + offsetUp)
            {
                hasReachedUp = true;
                hasReachedDown = false;
            }
        }
        else if (!hasReachedDown)
        {
            if (transform.position.y > startPosition.y + offsetDown)
            {
                // Move platform to the left
                Move(offsetDown);
            }
            else if (transform.position.y <= startPosition.y + offsetDown)
            {
                hasReachedUp = false;
                hasReachedDown = true;
            }
        }
    }

    private void Move(float offset)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPosition.x, transform.position.y + offset, transform.position.z), speed * Time.deltaTime);
    }
}
