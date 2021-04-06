using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        CStatManager.instance.GetInheritedAbCond(ref myAbCond);

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
        base.Event_FirstTurn(_o, _e);
        myUI.HpUpdate();
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

    protected override void HpAndGuardReset()
    {
        base.HpAndGuardReset();
        //첫 스테이지 진입때만 base로 하고, 기본적으론 health 값은 스탯 매니저에게서 얻어올 것.
    }

    public void OnCardUseAnimation(CardType _type)
    {
        string trigger = $"Attack_{_type}";
        Debug.LogWarning(trigger);
        myAnimator.SetTrigger(trigger);
    }
}
