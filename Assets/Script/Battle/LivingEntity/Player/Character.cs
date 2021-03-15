using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : LivingEntity
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_turnM.turnStart += () => ResetGardPoint();
        m_turnM.firstTurn += () => ResetGardPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GetGardPoint(int GetValue)
    {
        base.GetGardPoint(GetValue);
        Debug.Log("플레이어가 가드 포인트를 획득했습니다. 획득치:" + GetValue);
        Debug.Log("플레이어의 현재 가드 포인트치:" + gardPoint);
    }
}
