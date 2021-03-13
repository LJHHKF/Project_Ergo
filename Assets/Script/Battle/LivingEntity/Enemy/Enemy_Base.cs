using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : LivingEntity
{
    public int monsterIndex = 0;
    public int monsterFieldIndex { get; set; }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        monsterFieldIndex = 0; //제작 중 디버그용 임시코드.
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
}
