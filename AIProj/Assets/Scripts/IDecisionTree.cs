﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Interfaces

public interface IDecision
{
    IDecision MakeDecision();
}

#endregion

#region Decisions

public class BooleanDecision : IDecision
{
    IDecision trueBranch;
    IDecision falseBranch;

    public bool Value { set; get; }

    public BooleanDecision(IDecision _trueBranch, IDecision _falseBranch)
    {
        Debug.Assert(null != _trueBranch && null != _falseBranch) ;
        trueBranch = _trueBranch;
        falseBranch = _falseBranch;
    }

    public IDecision MakeDecision()
    {
        return Value ? trueBranch.MakeDecision() : falseBranch.MakeDecision();
    }
}

#endregion


#region Actions

public abstract class Action : IDecision
{
    public AIAgent agent; // aiagent
    public Transform Target { get; set; }
    private Action() { }
    public Action(AIAgent agent) { this.agent = agent; }
    public abstract IDecision MakeDecision();
}

public class Defend : Action
{
    public Defend(Enemy agent) : base(agent) { }

    public override IDecision MakeDecision()
    {
        Debug.Log("defend");
        return null;
    }
}

public class Attack : Action
{
    public Attack(HostileAgent agent) : base(agent) { }

    // public Transform Target { set; get; }

    public override IDecision MakeDecision()
    {
        if (null != Target) { ((HostileAgent)agent).Shoot(Target); }
        return null;
    }
}

public class BreakWall : Action
{
    public BreakWall(HostileAgent agent) : base(agent) { }

    public override IDecision MakeDecision()
    {
        if (null != Target) { ((HostileAgent)agent).Shoot(Target); }
        return null;
    }
}

public class Advance : Action
{
    public Advance(AIAgent agent) : base(agent) { }

    public override IDecision MakeDecision()
    {
        if (null != Target) { agent.Advance(Target); }
        return null;
    }
}

public class Rest : Action
{
    public Rest(AIAgent agent) : base(agent) { }

    public override IDecision MakeDecision()
    {
        Debug.Log("rest");
        return null;
    }
}

public class Rebuild : Action
{
    public Rebuild(AIAgent agent) : base(agent) { }

    public override IDecision MakeDecision()
    {
        return null;
    }
}

#endregion


