using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_NonTarget : Card_Base
{
    [Header("NonTarget Card Setting")]
    public float handHeightPoint = -2.5f;
    public float readyAlpha = 0.5f;

    protected bool ready = false;

    protected override void Awake()
    {
        base.Awake();
        isNonTarget = true;
    }

    public override void Dragged(Vector2 mousePos, LineDrawer liner)
    {
        base.Dragged(mousePos, liner);

        if(handHeightPoint < mousePos.y && !ready)
        {
            ReadyToUse();
        }
        else if(handHeightPoint > mousePos.y && ready)
        {
            UnReadyToUse();
        }

        //마우스 위치가 손패 위치서 벗어났느냐, 아니냐로 ReadyToUse 및 그 해제화 작업 필요.
    }

    protected virtual void ReadyToUse()
    {
        ready = true;
        for(int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, (m_sprRs[i].color.a * readyAlpha));
        for (int i = 0; i < array_text.Length; i++)
            array_text[i].faceColor = new Color32((byte)array_text[i].color.r, (byte)array_text[i].color.g, (byte)array_text[i].color.b, (byte)(array_text[i].color.a * readyAlpha));
    }

    protected virtual void UnReadyToUse()
    {
        ready = false;
        for(int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, (m_sprRs[i].color.a / readyAlpha));
        for (int i = 0; i < array_text.Length; i++)
            array_text[i].faceColor = new Color32((byte)array_text[i].color.r, (byte)array_text[i].color.g, (byte)array_text[i].color.b, (byte)(array_text[i].color.a / readyAlpha));
    }
}
