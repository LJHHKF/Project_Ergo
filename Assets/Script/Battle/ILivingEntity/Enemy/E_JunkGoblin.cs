using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_JunkGoblin : Enemy_Base
{
    [Header("Junk Goblin Setting")]
    [SerializeField] private EnemyAct_Base anotherSpecialAct;

    protected override void ActSetting()
    {
        if (curSpGauge >= r_maxSpGauge && EnemiesManager.instance.SearchHadMonster(6))
        {
            readyAct = anotherSpecialAct;
            curSpGauge = 0;
            GetReadyActInfo();
        }
        else
        {
            base.ActSetting();
        }
    }
}
