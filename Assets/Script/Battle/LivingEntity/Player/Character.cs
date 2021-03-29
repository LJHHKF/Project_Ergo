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
        fullHealth = CStatManager.fullHealth_pure;
        health = CStatManager.health;
        fix_endu = CStatManager.endurance;
        fix_stren = CStatManager.strength;
        fix_sol = CStatManager.solid;
        fix_int = CStatManager.intelligent;

        TurnManager.instance.firstTurn += () => myUI.HpUpdate();
        TurnManager.instance.firstTurn += () => FlucStatReset();
        TurnManager.instance.firstTurn += () => CalculateStat();
        TurnManager.instance.turnStart += () => ResetGuardPoint();
        TurnManager.instance.firstTurn += () => InitMaxCostSetting();
        TurnManager.instance.turnStart += () => myAbCond.Affected();
        TurnManager.instance.battleEnd += () => CStatManager.instance.HealthPointUpdate(health);

        onDeath += () => CStatManager.instance.HealthPointUpdate(health); // 게임오버 체크는 여기 들어가서 함.
    }

    protected override void ReleseTurnAct()
    {
        TurnManager.instance.firstTurn -= () => myUI.HpUpdate();
        TurnManager.instance.firstTurn -= () => FlucStatReset();
        TurnManager.instance.firstTurn -= () => CalculateStat();
        TurnManager.instance.turnStart -= () => ResetGuardPoint();
        TurnManager.instance.firstTurn -= () => InitMaxCostSetting();
        TurnManager.instance.turnStart -= () => myAbCond.Affected();
        TurnManager.instance.battleEnd -= () => CStatManager.instance.HealthPointUpdate(health);
    }

    public override void GetGuardPoint(int GetValue)
    {
        base.GetGuardPoint(GetValue);
        Debug.Log("플레이어가 가드 포인트를 획득했습니다. 획득치:" + GetValue);
        Debug.Log("플레이어의 현재 가드 포인트치:" + GuardPoint);
    }

    public override void ChangeCost(int changeV)
    {
        if (m_costM == null)
            FindCostManager();

        m_costM.maxCost += changeV;
    }

    private void InitMaxCostSetting()
    {
        if (m_costM == null)
            FindCostManager();
        m_costM.maxCost = init_maxCost;
    }

    private void FindCostManager()
    {
        m_costM = GameObject.FindGameObjectWithTag("CostManager").GetComponent<CostManager>();
    }

    protected override void HpAndGuardReset()
    {
        base.HpAndGuardReset();
        //첫 스테이지 진입때만 base로 하고, 기본적으론 health 값은 스탯 매니저에게서 얻어올 것.
    }
}
