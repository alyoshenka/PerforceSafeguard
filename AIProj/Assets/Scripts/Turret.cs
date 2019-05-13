
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IDamageable
{
    public float shotTime;
    public int range;
    public int maxHealth;

    int currentHealth;
    float elapsedTime;
    Vector3 pos;
    BulletManager bulletManager;

    public static HashSet<Transform> turrets;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        elapsedTime = 0;
        bulletManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletManager>();
        pos = transform.position;
        pos.y = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // sort enemies
        float minDist = range;
        Transform target = null;
        foreach(Transform t in Enemy.enemies)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                target = t;
            }
        }
        if(null != target) { Shoot(target); }
    }

    void Shoot(Transform target)
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= shotTime)
        {
            elapsedTime = 0f;
            bulletManager.AllocateBullet(transform.position, target, BulletManager.BulletType.turret);
        }
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) { OnDeath(); }

        Instantiate(MapGenerator.damageEffect, transform.position, Quaternion.identity);
    }

    public void OnDeath()
    {
        turrets.Remove(transform);
        gameObject.SetActive(false);
    }
}
