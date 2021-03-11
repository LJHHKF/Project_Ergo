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
                if(battleUIManager == null)
                {
                    FindBattleUIManger();
                }
                battleUIManager.OnDiceSysetm(gameObject.transform.position);
                m_sprR.color = new Color(m_sprR.color.r, m_sprR.color.g, m_sprR.color.b, 0);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
