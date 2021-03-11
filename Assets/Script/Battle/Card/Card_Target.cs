using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Target : Card_Base
{
    protected LivingEntity livTarget;

    public virtual void OnEnable()
    {
        livTarget = null;
    }

    public override void SetTarget(GameObject input)
    {
        target = input;// 임시 코드. LivingEntity를 상속하는 Enemy 를 받을 것.
        if (target != null)
        {
            livTarget = target.GetComponent<LivingEntity>();
            if (livTarget != null)
            {
                battleUIManager.OnDiceSysetm();
            }
        }
    }
}
