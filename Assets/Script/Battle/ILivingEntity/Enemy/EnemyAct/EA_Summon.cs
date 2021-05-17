using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_Summon : EnemyAct_Base
{
    [Header("Summon Setting")]
    [SerializeField] protected int monsterID;
    [SerializeField] protected int maxCnt;
    protected override void SummonAct()
    {
        EnemiesManager.instance.SummonMonster(monsterID, maxCnt);
    }
}
