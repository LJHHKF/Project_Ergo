using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : LivingEntity
{

    public int monsterIndex = 0;

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



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_turnM.firstTurn += () => ResetGuardPoint();
        m_turnM.playerTurnEnd += () => ResetGuardPoint();

        onDeath += () => StartCoroutine(DelayedUnActived(1.0f));
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")가 데미지를 입었습니다. :" + damage);
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")의 남은 체력 :" + health);
    }

    public void DummyAct()
    {
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")가 행동했습니다.");
    }
}
