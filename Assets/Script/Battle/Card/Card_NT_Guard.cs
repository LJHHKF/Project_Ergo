using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_NT_Guard : Card_NonTarget
{
    private Character m_char;
    public override void SetTarget(GameObject input)
    {
        if (ready)
        {
            if (m_char == null)
            {
                m_char = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            }

            if (battleUIManager == null)
            {
                FindBattleUIManger();
            }
            battleUIManager.OnDiceSysetm(gameObject.transform.position);
            for(int i = 0; i < m_sprRs.Length; i++)
                m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, 0);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    public override void Use(int diceValue)
    {
        if (ready)
        {
            ready = false;
            int value = fixP + Mathf.RoundToInt(diceValue * flucPRate);
            m_char.GetGardPoint(value);
            base.Use(diceValue);
        }
    }
}
