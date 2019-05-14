using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : HostileAgent {

    public float restTime;
    public float actionTime;

    Transform[] patrolPoints;
    int patrolIdx;

    float restElapsed;
    float actionElapsed;

    public bool CanAct { get { return restElapsed >= restTime; } }
    public bool NeedsRest { get { return actionElapsed >= actionTime; } }

	// Use this for initialization
	new void Start () {
        base.Start();
        stateMachine = new DefenderDecisionTree(this);
        restElapsed = 0f;
        actionElapsed = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Shoot(Transform target)
    {
        throw new System.NotImplementedException();
    }
}
