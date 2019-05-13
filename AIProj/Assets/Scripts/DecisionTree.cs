using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionTree
{
    protected IDecision start;

    protected abstract void Update(AIAgent agent);
    public abstract void RunTree(AIAgent agent);
}

    public class EnemyDecisionTree : DecisionTree
    {
        // decisions
        BooleanDecision defenseInRange; // start
        BooleanDecision defenderAttacking;
        BooleanDecision wallInWay;

        // actions
        Defend defend;
        Attack attack;
        BreakWall breakWall;
        Advance advance;

        public EnemyDecisionTree(Enemy agent)
        {
            advance = new Advance(agent);
            breakWall = new BreakWall(agent);
            attack = new Attack(agent);
            defend = new Defend(agent);

            wallInWay = new BooleanDecision(breakWall, advance);
            defenderAttacking = new BooleanDecision(defend, attack);
            defenseInRange = new BooleanDecision(defenderAttacking, wallInWay);

            start = defenseInRange;
        }

        public override void RunTree(AIAgent agent)
        {
            Update(agent);

            IDecision current = start;
            while (null != current) { current = current.MakeDecision(); }
        }

        protected override void Update(AIAgent agent)
        {
            Enemy enem = (Enemy)agent;

            defenseInRange.Value = null == enem.Target ? false : Vector3.Distance(agent.transform.position, enem.Target.transform.position) < enem.aggroDistance + 1.01f;
            defenderAttacking.Value = false;
            wallInWay.Value = Wall.walls.Contains(enem.Destination);

            advance.Target = enem.Destination;
            breakWall.Target = wallInWay.Value ? enem.Destination : null; // can be simplified after debugging
            attack.Target = enem.Target;
        }
    }

    public class DefenderDecisionTree : DecisionTree
    {
        // decisions
        BooleanDecision tired;
        BooleanDecision enemyInRange;
        BooleanDecision wallBroken;

        // actions
        Rest rest;
        Attack attack;
        Rebuild rebuild;
        Advance patrol;

        public DefenderDecisionTree(Defender agent)
        {
            patrol = new Advance(agent);
            rebuild = new Rebuild(agent);
            attack = new Attack(agent);
            rest = new Rest(agent);

            wallBroken = new BooleanDecision(rebuild, patrol);
            enemyInRange = new BooleanDecision(attack, wallBroken);
            tired = new BooleanDecision(rest, enemyInRange);

            start = tired;
        }

        public override void RunTree(AIAgent agent)
        {
            throw new System.NotImplementedException();
        }

        protected override void Update(AIAgent agent)
        {
            Defender def = (Defender)agent;

            tired.Value = def.NeedsRest;

            // get closest enemy
            attack.Target = null;
            float minDist = def.aggroDistance;
            foreach (Transform t in Enemy.enemies)
            {
                float dist = Vector3.Distance(def.transform.position, t.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    attack.Target = t;
                }
            }
            enemyInRange.Value = null != attack.Target;

            wallBroken.Value = false;
            rest.Target = null;
            rebuild.Target = null;
            patrol.Target = null;
        }
    }

