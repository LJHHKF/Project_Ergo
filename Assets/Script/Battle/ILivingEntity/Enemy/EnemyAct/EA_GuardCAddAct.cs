using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyActType;
using System;

public class EA_GuardCAddAct : EnemyAct_Base
{

    [SerializeField] protected bool isSet_ifGuarded;
    [SerializeField] protected ActSet ifGuarded;
    [SerializeField] protected bool isSet_ifHPDamaged;
    [SerializeField] protected ActSet ifHPDamaged;

    public override void Act()
    {
        for (int i = 0; i < acts.Length; i++)
        {
            int _i = i;
            if (acts[_i].affactType == AffectType.Attack)
            {
                if (target.OnDamage(r_power[_i]))
                    if (isSet_ifHPDamaged)
                    {
                        int _power = 0;

                        if (ifHPDamaged.affactType == AffectType.Abcond)
                        {
                            _power = ifHPDamaged.fixedPower;
                        }
                        else
                        {
                            if (ifHPDamaged.type == ActType.NormalAttack)
                            {
                                _power = Mathf.RoundToInt(m_Enemy.strength * ifHPDamaged.powerRate);
                            }
                            else if (acts[_i].type == ActType.SpecialAttack)
                            {
                                _power = Mathf.RoundToInt(m_Enemy.intel * ifHPDamaged.powerRate);
                            }
                            else if (acts[_i].type == ActType.Guard)
                            {
                                _power = Mathf.RoundToInt(m_Enemy.solid * ifHPDamaged.powerRate);
                            }
                        }

                        if (ifHPDamaged.affactType == AffectType.Attack)
                        {
                            StartCoroutine(delayedAffect(()=>target.OnDamage(_power)));
                        }
                        else if (ifHPDamaged.affactType == AffectType.Guard)
                        {
                            StartCoroutine(delayedAffect(()=>m_Enemy.AddGuardPoint(_power)));
                        }
                        else if (ifHPDamaged.affactType == AffectType.Abcond)
                        {
                            StartCoroutine(delayedAffect(()=>target.OnAddAbCond(ifHPDamaged.abcondID, ifHPDamaged.fixedPower, ifHPDamaged.isDelayedAbCond)));
                        }
                    }
                    else
                    if (isSet_ifGuarded)
                    {
                        int _power = 0;

                        if (ifGuarded.affactType == AffectType.Abcond)
                        {
                            _power = ifGuarded.fixedPower;
                        }
                        else
                        {
                            if (ifGuarded.type == ActType.NormalAttack)
                            {
                                _power = Mathf.RoundToInt(m_Enemy.strength * ifGuarded.powerRate);
                            }
                            else if (acts[_i].type == ActType.SpecialAttack)
                            {
                                _power = Mathf.RoundToInt(m_Enemy.intel * ifGuarded.powerRate);
                            }
                            else if (acts[_i].type == ActType.Guard)
                            {
                                _power = Mathf.RoundToInt(m_Enemy.solid * ifGuarded.powerRate);
                            }
                        }

                        if (ifGuarded.affactType == AffectType.Attack)
                        {
                            StartCoroutine(delayedAffect(()=>target.OnDamage(_power)));
                        }
                        else if (ifGuarded.affactType == AffectType.Guard)
                        {
                            StartCoroutine(delayedAffect(()=>m_Enemy.AddGuardPoint(_power)));
                        }
                        else if (ifGuarded.affactType == AffectType.Abcond)
                        {
                            StartCoroutine(delayedAffect(()=>target.OnAddAbCond(ifHPDamaged.abcondID, ifHPDamaged.fixedPower, ifHPDamaged.isDelayedAbCond)));
                        }
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
