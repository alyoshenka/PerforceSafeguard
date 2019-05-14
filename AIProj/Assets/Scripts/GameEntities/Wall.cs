using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IDamageable {

    public float maxHealth;

    // walls in enemy path
    public static HashSet<Transform> walls;

    float currentHealth;
    Index idx;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}

    public void Set(Index _idx)
    {
        idx = _idx;
    }
	
	public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0) { OnDeath(); }

        Instantiate(MapGenerator.damageEffect, transform.position, Quaternion.identity);
    }

    public void OnDeath()
    {
        walls.Remove(MapGenerator.tiles[idx.y, idx.x].transform);
        transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        GetComponent<Renderer>().material.color = Color.green;
    }
}
