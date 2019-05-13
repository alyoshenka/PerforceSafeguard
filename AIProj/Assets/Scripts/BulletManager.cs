using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// object pool for Bullets
public class BulletManager : MonoBehaviour
{
    public int maxBullets;
    public GameObject enemyBullet;
    public GameObject turretBullet;

    public enum BulletType { enemy, turret };

    Bullet[] enemyBullets;
    Bullet[] turretBullets;

    Bullet b;

    // Start is called before the first frame update
    void Start()
    {
        enemyBullets = new Bullet[maxBullets];
        turretBullets = new Bullet[maxBullets];

        GameObject g;

        for(int i = 0; i < maxBullets; i++)
        {
            enemyBullets[i] = InstantiateBullet(enemyBullet);
            turretBullets[i] = InstantiateBullet(turretBullet);
        }
    }

    Bullet InstantiateBullet(GameObject bullet)
    {
        GameObject obj = Instantiate(bullet, Vector3.zero, Quaternion.identity);
        Bullet b = gameObject.GetComponent<Bullet>();
        obj.gameObject.SetActive(false);
        return b;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AllocateBullet(Vector3 pos, Transform lookAt, BulletType type)
    {
        Bullet[] bullets;
        GameObject bul;

        switch (type)
        {
            case BulletType.enemy:
                bullets = enemyBullets;
                bul = enemyBullet;
                break;
            case BulletType.turret:
                bullets = turretBullets;
                bul = turretBullet;
                break;
            default:
                return;
        }

        b = null;
        for(int i = 0; i < maxBullets; i++)
        {
            if(null == bullets[i])
            {
                GameObject g = Instantiate(bul, Vector3.zero, Quaternion.identity);
                b = g.GetComponent<Bullet>();
                g.SetActive(true);
                bullets[i] = b;
                break;
            }
            if (!bullets[i].gameObject.activeSelf)
            {
                bullets[i].gameObject.SetActive(true);
                b = bullets[i];
                break;
            }
        }

        if(null == b) { return; } // no room
        b.transform.position = pos;
        Vector3 newLook = lookAt.position;
        newLook.y = 1;
        lookAt.position = newLook;
        b.transform.LookAt(lookAt);       
    }
}
