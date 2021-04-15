using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyActType;

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
            for (int j = 0; j < acts[i].repeatNum; j++)
            {
                if (acts[i].affactType == AffectType.Attack)
                {
                    if (target.OnDamage(r_power[i]))
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
                                else if (acts[i].type == ActType.SpecialAttack)
                                {
                                    _power = Mathf.RoundToInt(m_Enemy.intel * ifHPDamaged.powerRate);
                                }
                                else if (acts[i].type == ActType.Guard)
                                {
                                    _power = Mathf.RoundToInt(m_Enemy.solid * ifHPDamaged.powerRate);
                                }
                            }

                            for (int k = 0; k < ifHPDamaged.repeatNum; i++)
                            {
                                if (ifHPDamaged.affactType == AffectType.Attack)
                                {
                                    target.OnDamage(_power);
                                }
                                else if (ifHPDamaged.affactType == AffectType.Guard)
                                {
                                    m_Enemy.GetGuardPoint(_power);
                                }
                                else if (ifHPDamaged.affactType == AffectType.Abcond)
                                {
                                    target.OnAddAbCond(ifHPDamaged.abcondID, ifHPDamaged.fixedPower, ifHPDamaged.isDelayedAbCond);
                                }
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
                                else if (acts[i].type == ActType.SpecialAttack)
                                {
                                    _power = Mathf.RoundToInt(m_Enemy.intel * ifGuarded.powerRate);
                                }
                                else if (acts[i].type == ActType.Guard)
                                {
                                    _power = Mathf.RoundToInt(m_Enemy.solid * ifGuarded.powerRate);
                                }
                            }

                            for (int k = 0; k < ifGuarded.repeatNum; i++)
                            {
                                if (ifGuarded.affactType == AffectType.Attack)
                                {
                                    target.OnDamage(_power);
                                }
                                else if (ifGuarded.affactType == AffectType.Guard)
                                {
                                    m_Enemy.GetGuardPoint(_power);
                                }
                                else if (ifGuarded.affactType == AffectType.Abcond)
                                {
                                    target.OnAddAbCond(ifHPDamaged.abcondID, ifHPDamaged.fixedPower, ifHPDamaged.isDelayedAbCond);
                                }
                            }
                        }
                }
            }
        }
    }
}
