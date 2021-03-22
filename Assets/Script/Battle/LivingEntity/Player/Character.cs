using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : LivingEntity
{
    public int init_maxCost = 3;
    //private int _i_maxCost;
    private CostManager m_costM;
    private CStatManager m_statM;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        m_statM = GameObject.FindGameObjectWithTag("InfoM").GetComponent<CStatManager>();
        m_statM.GetStats(out fix_endu, out fix_stren, out fix_sol, out fix_int);

        m_turnM.turnStart += () => ResetGuardPoint();
        m_turnM.firstTurn += () => InitMaxCostSetting();
        m_turnM.turnStart += () => myAbCond.Affected();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnDestroy()
    {
        m_statM.SetStats(fix_endu, fix_stren, fix_sol, fix_int);
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
