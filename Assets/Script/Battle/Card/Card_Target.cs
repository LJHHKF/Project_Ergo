using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Target : Card_Base
{
    protected LivingEntity livTarget;

    protected override void OnEnable()
    {
        livTarget = null;
        base.OnEnable();
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
                for(int i = 0; i < m_sprRs.Length; i++)
                    m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, 0);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public override void Use(int diceValue)
    {
        if(livTarget != null)
        {
            int dmg = fixP + Mathf.RoundToInt(diceValue * flucPRate);
            livTarget.OnDamage(dmg);
        }
        base.Use(diceValue);
    }
}
