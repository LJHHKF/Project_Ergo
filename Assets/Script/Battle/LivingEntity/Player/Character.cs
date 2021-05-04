using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Card;

public class Character : LivingEntity
{
    [Header("Character Setting")]
    [SerializeField] private int init_maxCost = 3;
    [SerializeField] private float card_anim_delayTime = 1.0f;
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

        // 일부 UI (Top UI등)이 게임 시작 후 첫 참조 때 제대로 값을 못 얻어가는 문제가 발생해서 firstTurnEvent에서 꺼냄.
        FlucStatReset();
        CalculateStat();

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

    protected override void Event_FirstTurn()
    {
        CStatManager.instance.GetInheritedAbCond(ref myAbCond);
        myUI.HpUpdate();
        ResetGuardPoint();
        InitMaxCostSetting();
    }

    protected override void Event_TurnStart()
    {
        ResetGuardPoint();
        myAbCond.Affected();
    }

    protected override void Event_BattleEnd()
    {
        CStatManager.instance.HealthPointUpdate(health);
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
        StartCoroutine(DelayAttackAnim(_type));
    }

    IEnumerator DelayAttackAnim(CardType _type)
    {
        yield return new WaitForSeconds(card_anim_delayTime);
        string trigger = $"Attack_{_type}";
        myAnimator.SetTrigger(trigger);
        yield break;
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

    public void AddActionPopUp(string _name)
    {
        myUI.AddPopUpText_ActionName(_name);
    }
}
