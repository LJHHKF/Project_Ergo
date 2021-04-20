using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyActType;
using System;

namespace EnemyActType
{
    public enum ActType
    {
        NormalAttack,
        SpecialAttack,
        Guard,
    }

    public enum AffectType
    {
        Attack,
        Guard,
        Abcond,
        CondAttack_Info,
        CondAbcond_Info,
        Summon
    }
}

public class EnemyAct_Base : MonoBehaviour
{
    [Serializable]
    public struct ActSet
    {
        public ActType type;
        public AffectType affactType;
        public float powerRate;
        public int repeatNum;
        public int abcondID;
        public int fixedPower;
        public bool isDelayedAbCond;
    }

    [Header("Base Setting")]
    protected Enemy_Base m_Enemy;
    [SerializeField] protected string actName;
    [SerializeField] protected int actID;
    [SerializeField] protected ActSet[] acts;
    [SerializeField] protected int typeVariationNum = 1;
    [SerializeField] protected Sprite actSprite;
    [TextArea] public string actExplain;
    protected int[] r_power;
    //[SerializeField] private int abCondID = -1;
    //[SerializeField] private bool isRelatedAbCond = false;
    //[SerializeField] private bool isAllTargeted = false;

    protected Character target;

    protected virtual void Start()
    {
        m_Enemy = gameObject.GetComponent<Enemy_Base>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();

        r_power = new int[acts.Length];
    }

    public virtual void Act()
    {
        for (int i = 0; i < acts.Length; i++)
        {
            for (int j = 0; j < acts[i].repeatNum; j++)
            {
                if (acts[i].affactType == AffectType.Attack)
                {
                    target.OnDamage(r_power[i]);
                }
                else if (acts[i].affactType == AffectType.Guard)
                {
                    m_Enemy.GetGuardPoint(r_power[i]);
                }
                else if (acts[i].affactType == AffectType.Abcond)
                {
                    target.OnAddAbCond(acts[i].abcondID, acts[i].fixedPower, acts[i].isDelayedAbCond);
                }
                else if (acts[i].affactType == AffectType.Summon)
                {
                    SummonAct();
                }
            }
        }
    }

    protected virtual void SummonAct()
    {

    }


    public Sprite GetActSprite()
    {
        return actSprite;
    }

    public void GetActInfo(out int[] powers, out AffectType[] types,out int[] _repeatNum,out int typeVariNum)
    {
        AffectType[] _out = new AffectType[acts.Length];
        int[] _out2 = new int[acts.Length];
        for (int i = 0; i < acts.Length; i++)
        {
            _out[i] = acts[i].affactType;
            _out2[i] = acts[i].repeatNum;

            if (acts[i].affactType == AffectType.Abcond || acts[i].affactType == AffectType.CondAbcond_Info || acts[i].affactType == AffectType.Summon)
            {
                r_power[i] = acts[i].fixedPower;
            }
            else
            {
                if (acts[i].type == ActType.NormalAttack)
                {
                    r_power[i] = Mathf.RoundToInt(m_Enemy.strength * acts[i].powerRate);
                }
                else if (acts[i].type == ActType.SpecialAttack)
                {
                    r_power[i] = Mathf.RoundToInt(m_Enemy.intel * acts[i].powerRate);
                }
                else if (acts[i].type == ActType.Guard)
                {
                    r_power[i] = Mathf.RoundToInt(m_Enemy.solid * acts[i].powerRate);
                }
            }
        }

        powers = r_power;
        types = _out;
        typeVariNum = typeVariationNum;
        _repeatNum = _out2;
    }

    //public virtual void GetActInfo(out int _power, out bool _isAll)
    //{
    //    _power = power;
    //    _isAll = isAllTargeted;
    //}
}
