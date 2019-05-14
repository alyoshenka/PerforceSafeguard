using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective : MonoBehaviour, IDamageable
{
    public int maxHealth;

    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0) { OnDeath(); }

        Instantiate(MapGenerator.damageEffect, transform.position, Quaternion.identity);
    }

    public void OnDeath()
    {
        SceneManager.LoadScene("Lose");
    }
}
