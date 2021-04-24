using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Card;

public class Character : LivingEntity
{
    [SerializeField] private int init_maxCost = 3;
    //private int _i_maxCost;
    private CostManager m_costM;

    // Start is called before the first frame update
    protected override void Start()
    {
        fullHealth = CStatManager.instance.fullHealth_pure;
        health = CStatManager.instance.health;
        fix_endu = CStatManager.instance.endurance;
        fix_stren = CStatManager.instance.strength;
        fix_sol = CStatManager.instance.solid;
        fix_int = CStatManager.instance.intelligent;

        TurnManager.instance.firstTurn += Event_FirstTurn;
        TurnManager.instance.turnStart += Event_TurnStart;
        TurnManager.instance.battleEnd += Event_BattleEnd;

        onDeath += () => CStatManager.instance.HealthPointUpdate(health); // 게임오버 체크는 여기 들어가서 함.
    }

    protected override void ReleseTurnAct()
    {
        TurnManager.instance.firstTurn -= Event_FirstTurn;
        TurnManager.instance.turnStart -= Event_TurnStart;
        TurnManager.instance.battleEnd -= Event_BattleEnd;
    }

    protected override void Event_FirstTurn(object _o, EventArgs _e)
    {
        //base.Event_FirstTurn(_o, _e);
        FlucStatReset();
        CalculateStat();
        CStatManager.instance.GetInheritedAbCond(ref myAbCond);
        myUI.HpUpdate();
        ResetGuardPoint();
        InitMaxCostSetting();
    }

    protected override void Event_TurnStart(object _o, EventArgs _e)
    {
        base.Event_TurnStart(_o, _e);
        ResetGuardPoint();
        myAbCond.Affected();
    }

    protected override void Event_BattleEnd(object _o, EventArgs _e)
    {
        CStatManager.instance.HealthPointUpdate(health);
        myAbCond.SaveCsCurAbCond();
    }

    public override void GetGuardPoint(int GetValue)
    {
        base.GetGuardPoint(GetValue);
        Debug.Log("플레이어가 가드 포인트를 획득했습니다. 획득치:" + GetValue);
        Debug.Log("플레이어의 현재 가드 포인트치:" + GuardPoint);
    }

    public override void ChangeCost(int changeV)
    {
        ChkAndFindCostManager();
        m_costM.maxCost += changeV;
    }

    private void InitMaxCostSetting()
    {
        ChkAndFindCostManager();
        m_costM.maxCost = init_maxCost;
    }

    private void ChkAndFindCostManager()
    {
        if(m_costM == null)
            m_costM = GameObject.FindGameObjectWithTag("CostManager").GetComponent<CostManager>();
    }

    public void OnCardUseAnimation(CardType _type)
    {
        string trigger = $"Attack_{_type}";
        Debug.LogWarning(trigger);
        myAnimator.SetTrigger(trigger);
    }

    public override bool OnDamage(int damage)
    {
        myAnimator.SetTrigger("Hit");
        return base.OnDamage(damage);
    }

    public override void OnPenDamage(int damage)
    {
        myAnimator.SetTrigger("Hit");
        base.OnPenDamage(damage);
    }
}
