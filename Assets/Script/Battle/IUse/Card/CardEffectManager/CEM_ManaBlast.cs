using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEM_ManaBlast : ICardEffectM
{
    private Character m_char;

    private void OnEnable()
    {
        SetChar();
    }

    private void SetChar()
    {
        m_char = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    public override void OnEffect()
    {
        if (m_char == null)
            SetChar();
        m_char.OnManaBlastEffect();
    }
}
