using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    public BulletNew BulletPrefab;

    private List<BulletNew> activeBullets = new List<BulletNew>();
    private List<BulletNew> nonActiveBullets = new List<BulletNew>();

    public void DisableBullet(BulletNew b)
    {
        b.gameObject.SetActive(false);
        nonActiveBullets.Add(b);
        activeBullets.Remove(b);
    }
    
    public void ActivateBullet(BulletNew b)
    {
        b.gameObject.SetActive(true);
        nonActiveBullets.Remove(b);
        activeBullets.Add(b);
    }

    public BulletNew GetBullet()
    {
        BulletNew b;
        if (nonActiveBullets.Count > 0)
        {
            b = nonActiveBullets[0];
            b.gameObject.SetActive(true);
            nonActiveBullets.Remove(b);


        }
        else
        {
            b = Instantiate(BulletPrefab, this.transform);
            b.pooler = this;
           
            
        }
        activeBullets.Add(b);
        return b;
        
    }
    

    public void ShootBullet(Vector2 targetAngle,Vector3 startLocation )
    {
        BulletNew b = GetBullet();
        b.MoveDirection = targetAngle;
        b.transform.position = startLocation;
        b.transform.eulerAngles =
            new Vector3(0, 0,
                Vector2.Angle(targetAngle,
                    Vector2.up) + 90); //(new Vector3(b.transform.localPosition.x + targetAngle.x, b.transform.localPosition.y + targetAngle.y, 0));
    }
    
}
