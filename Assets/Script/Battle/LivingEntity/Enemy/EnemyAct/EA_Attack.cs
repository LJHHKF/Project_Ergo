using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_Attack : EnemyAct_Base
{
    private Character target;

    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    public override void Act()
    {
        target.OnDamage(r_power);
    }
}
