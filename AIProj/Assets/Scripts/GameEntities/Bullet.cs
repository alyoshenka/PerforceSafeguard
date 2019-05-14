using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float maxDist;
    public int damage;

    Vector3 startPos;
    IDamageable canDamage;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        maxDist *= MapGenerator.mapScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if(Vector3.Distance(startPos, transform.position) > maxDist) { Explode(); }
    }

    void OnTriggerEnter(Collider other)
    {       
        canDamage = null;
        canDamage = other.gameObject.GetComponent<IDamageable>();
        if(null != canDamage) { canDamage.ApplyDamage(damage); }

        Explode();
    }

    void Explode()
    {
        gameObject.SetActive(false);
    }
}
