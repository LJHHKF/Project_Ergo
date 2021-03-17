using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : LivingEntity
{
    public int init_maxCost = 3;
    //private int _i_maxCost;
    private CostManager m_costM;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_turnM.turnStart += () => ResetGuardPoint();
        m_turnM.firstTurn += () => InitMaxCostSetting();
        m_turnM.turnStart += () => myAbCond.Affected();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
