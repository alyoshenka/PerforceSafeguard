using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAgent : MonoBehaviour {

    public DecisionTree stateMachine; // won't show in editor
    [Range(5, 25)]
    public float speed; // big speeds break it

    // public abstract void Shoot(Transform target);
    public void Advance(Transform target)
    {
        transform.LookAt(target);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}

public abstract class HostileAgent : AIAgent, IDamageable
{
    public int maxHealth;
    public float aggroDistance;
    public float shotTimer;
    // public UnityEngine.UI.Image healthBar;

    protected int currentHealth;   

    public abstract void Shoot(Transform target);

    protected void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        // healthBar.fillAmount = currentHealth * 1f / maxHealth;
        if (currentHealth <= 0) { OnDeath(); }

        Instantiate(MapGenerator.damageEffect, transform.position, Quaternion.identity);
    }

    public void OnDeath()
    {
        gameObject.SetActive(false);
    }
}
