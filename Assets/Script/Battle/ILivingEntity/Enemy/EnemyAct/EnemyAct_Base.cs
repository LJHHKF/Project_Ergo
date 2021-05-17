using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyActType;
using System;
using System.Text;

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
    [SerializeField] protected string actAnimTrigger;
    [SerializeField] protected float affectDelay = 1.0f;
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
            int _i = i;
            if (acts[_i].affactType == AffectType.Attack)
            {
                StartCoroutine(delayedAffect(()=>target.OnDamage(r_power[_i])));
            }
            else if (acts[_i].affactType == AffectType.Guard)
            {
                StartCoroutine(delayedAffect(()=>m_Enemy.AddGuardPoint(r_power[_i])));
            }
            else if (acts[_i].affactType == AffectType.Abcond)
            {
                StartCoroutine(delayedAffect(()=>target.OnAddAbCond(acts[_i].abcondID, acts[_i].fixedPower, acts[_i].isDelayedAbCond)));
            }
            else if (acts[_i].affactType == AffectType.Summon)
            {
                StartCoroutine(delayedAffect(()=>SummonAct()));
            }
        }
        m_Enemy.SetAnimTrigger(actAnimTrigger);
    }

    protected virtual void SummonAct()
    {

    }


    public Sprite GetActSprite()
    {
        return actSprite;
    }

    public void GetActInfo(out int[] powers, out AffectType[] types, out int typeVariNum)
    {
        AffectType[] _out = new AffectType[acts.Length];
        int[] _out2 = new int[acts.Length];
        for (int i = 0; i < acts.Length; i++)
        {
            _out[i] = acts[i].affactType;

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
    }

    public string GetActPlainText()
    {
        StringBuilder m_sb = new StringBuilder();
        m_sb.Append(actExplain);
        m_sb.Replace("[힘]", m_Enemy.strength.ToString());
        m_sb.Replace("[내구]", m_Enemy.endurance.ToString());
        m_sb.Replace("[특공]", m_Enemy.intel.ToString());
        return m_sb.ToString();
    }

    public string GetActName()
    {
        return actName;
    }

    //public virtual void GetActInfo(out int _power, out bool _isAll)
    //{
    //    _power = power;
    //    _isAll = isAllTargeted;
    //}

    IEnumerator delayedAffect(Action _action)
    {
        yield return new WaitForSeconds(affectDelay);
        _action.Invoke();
        yield break;
    }
}
