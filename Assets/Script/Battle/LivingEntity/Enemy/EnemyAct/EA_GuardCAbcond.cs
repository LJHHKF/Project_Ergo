﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnemyActType;

public class EA_GuardCAbcond : EnemyAct_Base
{
    [Serializable]
    public struct AbCondAct
    {
        public bool isSet;
        public int abcondID;
        public int abcondPower;
        public bool isDelayedAbCond;
    }

    [Header("Guarded Condition AbCond Setting")]
    [SerializeField] protected AbCondAct ifGuarded;
    [SerializeField] protected AbCondAct ifHPDamaged;
    //인스펙터 뷰에서 설정할 때, UI 에 넘길 용도로 돌지 않을 상태이상 타입의 액트를 1개 더 추가할 것.

    public override void Act()
    {
        for(int i = 0; i < acts.Length; i++)
        {
            int _i = i;
            for(int j = 0; j < acts[_i].repeatNum; j++)
            {
                if(acts[_i].affactType == AffectType.Attack)
                {
                    if(target.OnDamage(r_power[_i]))
                        if(ifHPDamaged.isSet)
                            StartCoroutine(delayedAffect(()=>target.OnAddAbCond(ifHPDamaged.abcondID, ifHPDamaged.abcondPower, ifHPDamaged.isDelayedAbCond)));
                    else
                        if(ifGuarded.isSet)
                            StartCoroutine(delayedAffect(()=>target.OnAddAbCond(ifGuarded.abcondID, ifGuarded.abcondPower, ifGuarded.isDelayedAbCond)));
                }
            }
        }
    }

    IEnumerator delayedAffect(Action _action)
    {
        yield return new WaitForSeconds(affectDelay);
        _action.Invoke();
        yield break;
    }
}
