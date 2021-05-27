using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEM_Guard : ICardEffectM
{
    private Character m_char;

    private void OnEnable()
    {
        if (StageManager.instance.GetCurrentStageTypeIndex() == 0)
            StartCoroutine(DelayedInvoke(SetChar));
    }

    private void SetChar()
    {
        m_char = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    public override void OnEffect()
    {
        if (m_char == null)
            SetChar();
        m_char.OnGuardCardEffect();
    }
}
