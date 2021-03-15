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

            if (m_cardM == null)
            {
                FindBSCardManager();
            }
            m_cardM.DoHandsTransparency();
            m_Collider.enabled = false;
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
