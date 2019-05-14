using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// an enemy entity
public class Enemy : HostileAgent
{
    public static HashSet<Transform> enemies;

    List<Index> path;
    int pathIdx;
    BulletManager bulletManager;

    float shotElapsed;

    public Transform Target { get; private set; }
    public Transform Wall { get; private set; }
    public Transform Destination { get; private set; }

    // Use this for initialization
    new void Start() 
    {
        base.Start();
        stateMachine = new EnemyDecisionTree(this);
        transform.GetChild(0).localScale = new Vector3(aggroDistance * 2, aggroDistance * 2, aggroDistance * 2);
        bulletManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletManager>();
    }

    public void Set(List<Index> _path)
    {
        path = _path;
        pathIdx = path.Count - 1;
        Destination = MapGenerator.tiles[path[pathIdx].y, path[pathIdx--].x].transform;
        Vector3 adjust = Destination.position;
        adjust.y = 1;
        Destination.position = adjust;
        Target = null;
    }

    // Update is called once per frame
    void Update()
    {
        // find closest in turrets    
        float minDist = aggroDistance + 1.01f;
        Target = null;
        foreach (Transform t in Turret.turrets)
        {
            float dist = Vector3.Distance(transform.position, t.position); // make more effcient
            if (dist < minDist)
            {
                Target = t;
                dist = minDist;
            }
        }

        if (Vector3.Distance(transform.position, Destination.position) <= 0.2f)
        {
            if (pathIdx <= 0) { Target = MapGenerator.tiles[path[0].y, path[0].x].transform; } // shoot at objective
            else
            {
                Destination = MapGenerator.tiles[path[pathIdx].y, path[pathIdx--].x].transform;
                Vector3 adjust = Destination.position;
                adjust.y = 1;
                Destination.position = adjust;
            } // next in path
        }
       
        stateMachine.RunTree(this);
    }

    public override void Shoot(Transform target)
    {
        if(null == target) { return; }
        shotElapsed += Time.deltaTime;
        if (shotElapsed > shotTimer)
        {
            shotElapsed = 0f;
            bulletManager.AllocateBullet(transform.position, target, BulletManager.BulletType.enemy);
        }

    }
}


