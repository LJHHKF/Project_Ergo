using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : LivingEntity
{
    [Header("Enemy Setting")]
    public int monsterID = 0;
    [Header("E_Stat Setting")]
    public int fix_Endurance = 1;
    public int fix_Strength = 1;
    public int fix_Solid = 1;
    public int fix_Inteligent = 1;
    public int maxSpGauge = 3;
    protected int curSpGauge = 0;

    private int m_FieldIndex = 0;
    public int monsterFieldIndex  // 디버그용 임시코드
    {
        get
        {
            return m_FieldIndex;
        }
        set
        {
            m_FieldIndex = value;
        }
    }

    protected override void Start()
    {
        fix_endu = fix_Endurance;
        fix_stren = fix_Strength;
        fix_sol = fix_Solid;
        fix_int = fix_Inteligent;
        base.Start();
        m_turnM.playerTurnEnd += () => ResetGuardPoint();
        m_turnM.playerTurnEnd += () => curSpGauge++;
        m_turnM.playerTurnEnd += () => myAbCond.Affected();

        onDeath += () => StartCoroutine(DelayedUnActived(1.0f));
    }

    protected virtual void Update()
    {
        
    }

    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")가 데미지를 입었습니다. :" + damage);
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")의 남은 체력 :" + health);
    }

    public override void ChangeCost(int changeV)
    {
        if(curSpGauge + changeV < 0)
        {
            changeV = -curSpGauge;
        }
        else if(curSpGauge + changeV > maxSpGauge)
        {
            changeV = maxSpGauge - curSpGauge;
        }
        curSpGauge += changeV;
    }

    public void DummyAct()
    {
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")가 행동했습니다.");
    }
}
