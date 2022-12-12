using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool.Gun
{
    public class NewTurretController : MonoBehaviour
    {
        //object pool stuff
        public BulletPooler bulletPool;
        private float fireTimer;
        public float fireInterval = 2.5f;

        // transform of the target for turrent to detect and track
        public Transform detectTarget;

        // if distance between turrent and target lower than this value, it will keep pointing to target
        public float trackTriggerDist = 3f;

        public float rotationModifier = 180f;

        // the rotation speed for turrent to track its target
        public float trackRotateSpeed = 10f;

        // the rotation speed for turrent to rotate when not tracking its target
        public float automateRotateSpeed = 10f;

        private float dist;

        public LayerMask TargetLayers;
        private Collider2D[] _targets = new Collider2D[]{};

        void Start()
        {
            bulletPool = FindObjectOfType<BulletPooler>();
            if (bulletPool == null)
            {
                Debug.LogError("Need a reference to the object pool");
            }
        }

        private Transform GetTarget()
        {
            if (detectTarget == null)
            {
                _targets = Physics2D.OverlapCircleAll(this.transform.position, trackTriggerDist * 3, TargetLayers);
            }

            if (_targets.Length > 0)
            {
                foreach (var col in _targets)
                {
                    if(!(col.CompareTag("Player") || col.CompareTag("Friend")))
                    {
                        break;
                    }
                    
                    if (Physics2D.Linecast(this.transform.position, col.transform.position))
                    {
                        return col.transform;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            

            return null;
        }

        // Update is called once per frame
        void Update()
        {
            detectTarget = GetTarget();

            if (detectTarget == null)
            {
                transform.localEulerAngles =
                    new Vector3(0, 0, Mathf.PingPong(Time.time * automateRotateSpeed, 180) - 90);
            }
            else
            {

                

                // get the distance between tracking target and turrent
                dist = Vector3.Distance(transform.position, detectTarget.position);

                // if distance closer than that to trigger tracking, keep pointing to the target
                if (dist <= trackTriggerDist)
                {
                    Vector3 vectorToTarget = detectTarget.transform.position - transform.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * trackRotateSpeed);

                    //firing the bullet
                    if (fireTimer >= fireInterval)
                    {
                        Debug.Log("bullet firing engaged");
                        fireTimer = 0f;

                        bulletPool.ShootBullet(vectorToTarget, transform.position);

                    }

                    fireTimer += Time.deltaTime;
                }
                // otherwise, rotate between limited range of angles
                // can be modified according to default angle of turrent's sprite, the example used to test this script points to left by default
                else
                {
                    // Mathf.PingPong(t, length) returns a value that will increase and decrease between 0 and length
                    // used here to return a z-value between 0 and 180
                    // "- 90" at the end is used to make the range modifies from "0 to 180" to "-90 to 90"
                    transform.localEulerAngles =
                        new Vector3(0, 0, Mathf.PingPong(Time.time * automateRotateSpeed, 180) - 90);
                }
            }
        }
    }
}