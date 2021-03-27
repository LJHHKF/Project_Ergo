using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_Defense : EnemyAct_Base
{
    //[Header("Def Setting")]
    //public Enemy_Base target_self;

    public override void Act()
    {
        //target_self.GetGuardPoint(r_power);
        m_Enemy.GetGuardPoint(r_power);
    }
}
