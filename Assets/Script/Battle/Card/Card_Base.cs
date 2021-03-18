﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Card_Base : MonoBehaviour, ICard
{
    public enum Type
    {
        Sword,
        Magic
    }

    [Header("Card Base Setting")]
    public int cardID = 0;
    public int cost = 1;
    public int fixP = 1;
    public float flucPRate = 1.0f;
    public bool isNonTarget = false;
    public Type type;
    public Sprite cardImage;
    [TextArea]
    public string cardText;

    //[Header("renderPriority Setting")]
    protected int renderPriority = 1;
    protected float rotP;
    protected float moveP;
    protected bool ready = true;
    protected float handHeightPoint = -2.5f;
    protected float readyAlpha = 0.5f;

    protected GameObject target;
    protected DiceSystemManager diceManager;
    protected BattleUIManager battleUIManager;
    protected bool isThrowed = false;
    protected SpriteRenderer[] m_sprRs = new SpriteRenderer[2];
    protected BoxCollider2D m_Collider;
    protected Canvas textCanvas;
    protected TextMeshProUGUI text_cost;
    protected TextMeshProUGUI text_plain;
    protected TextMeshProUGUI text_name;
    protected TextMeshProUGUI[] array_text;
    protected BSCManager m_cardM;
    protected CostManager m_costM;

    public delegate void UseHandler(int dicevalue);
    public event UseHandler use;
    public event Action sub_use;
    //protected int d_Value;

    public Vector2 m_Position
    {
        get
        {
            return m_Position;
        }
        protected set
        {
            m_Position = gameObject.transform.position;
        }
    }

    protected virtual void Awake()
    {
        if (textCanvas == null)
        {
            textCanvas = gameObject.transform.Find("TextCanvas").GetComponent<Canvas>();
            text_cost = textCanvas.gameObject.transform.Find("CostText").GetComponent<TextMeshProUGUI>();
            text_plain = textCanvas.gameObject.transform.Find("CardText").GetComponent<TextMeshProUGUI>();
            text_name = textCanvas.gameObject.transform.Find("CardName").GetComponent<TextMeshProUGUI>();

            array_text = textCanvas.gameObject.GetComponents<TextMeshProUGUI>();
        }

        text_cost.text = cost.ToString();
        string temp = "고정치()";
        string temp2 = "고정치(" + fixP.ToString() + ")";
        string temp3 = "(변동치)";
        string temp4 = flucPRate.ToString();
        cardText = cardText.Replace(temp, temp2);
        cardText = cardText.Replace(temp3, temp4);
        text_plain.text = cardText;
        ready = false;

        FindBSCardManager();
        m_cardM.SetCardValues(out rotP, out moveP, out handHeightPoint, out readyAlpha);
    }

    protected virtual void OnEnable()
    {
        if(m_Collider == null)
        {
            m_Collider = gameObject.GetComponent<BoxCollider2D>();
        }
        m_Collider.enabled = true;

        if (m_sprRs[0] == null)
        {
            m_sprRs[0] = gameObject.GetComponent<SpriteRenderer>();
        }
        if (m_sprRs[1] == null)
        {
            m_sprRs[1] = gameObject.transform.Find("CardImage").GetComponent<SpriteRenderer>();
        }
        if (textCanvas == null)
        {
            textCanvas = gameObject.transform.Find("TextCanvas").GetComponent<Canvas>();
            text_cost = textCanvas.gameObject.transform.Find("CostText").GetComponent<TextMeshProUGUI>();
            text_plain = textCanvas.gameObject.transform.Find("CardText").GetComponent<TextMeshProUGUI>();
            text_name = textCanvas.gameObject.transform.Find("CardName").GetComponent<TextMeshProUGUI>();

            array_text = textCanvas.gameObject.GetComponents<TextMeshProUGUI>();
        }

        UndoTransparency();

    }

    protected virtual void Start()
    {
        FindBattleUIManger();
        FindCostManager();

        m_sprRs[0] = gameObject.GetComponent<SpriteRenderer>();
        m_sprRs[1] = gameObject.transform.Find("CardImage").GetComponent<SpriteRenderer>();

        m_Collider.enabled = true;
    }


    public virtual ICard Selected()
    {
        if (m_costM.cost < cost)
        {
            //실패처리 (붉은색 테두리 처리)
            Debug.LogError("코스트가 (" + (m_costM.cost - cost) + ")만큼 모자랍니다.");
            return null;
        }
        else
        {
            BringUpCard(true);
            return this;
        }
    }

    public virtual void Holded()
    {
        //카드 정보 넘겨주기
    }

    public virtual void Dragged(Vector2 mousePos, LineDrawer liner)
    {
        //선택된 카드 투명화, 카드 위치 -> 타겟 위치 선 연결 준비
        //타겟 위치는 업데이트에서 받아올 것.
        liner.SetLine_Worlds(gameObject.transform, mousePos);

        if (handHeightPoint < mousePos.y && !ready)
        {
            OnCardAlphaAndReady();
        }
        else if (handHeightPoint > mousePos.y && ready)
        {
            OffCardAlphaAndReady();
        }

        //Debug.Log(mousePos); //논타겟 카드의 손패와 필드 영역 벗어남 구분하기 위해 y값 확인
    }

    protected virtual void OnCardAlphaAndReady()
    {
        ready = true;
        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, (m_sprRs[i].color.a * readyAlpha));
        for (int i = 0; i < array_text.Length; i++)
            array_text[i].faceColor = new Color32((byte)array_text[i].color.r, (byte)array_text[i].color.g, (byte)array_text[i].color.b, (byte)(array_text[i].color.a * readyAlpha));
    }

    protected virtual void OffCardAlphaAndReady()
    {
        ready = false;
        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, (m_sprRs[i].color.a / readyAlpha));
        for (int i = 0; i < array_text.Length; i++)
            array_text[i].faceColor = new Color32((byte)array_text[i].color.r, (byte)array_text[i].color.g, (byte)array_text[i].color.b, (byte)(array_text[i].color.a / readyAlpha));
    }

    public virtual void Use(int diceValue)
    {
        if (m_costM == null)
        {
            FindCostManager();
        }
        if (m_costM.cost < cost) // 보험삼아 넣어둔 곳
        {
            return;
        }

        m_costM.cost -= cost;
        use(diceValue);

        if (sub_use != null)
        {
            sub_use();
        }


        if (m_cardM == null)
        {
            FindBSCardManager();
        }
        m_cardM.MoveToGrave(gameObject);

        DoTransparency();
    }

    protected void DrawLine()
    {
        //화살표선 긋기용 함수
    }

    public virtual void SetTarget(GameObject input)
    {
        target = input;

        if(isNonTarget)
        {
            if (target != null)
                return;
        }
        else
        {
            if (target.GetComponent<LivingEntity>() == null)
                return;
        }

        if (battleUIManager == null)
        {
            FindBattleUIManger();
        }
        battleUIManager.OnDiceSysetm();
        if (m_cardM == null)
        {
            FindBSCardManager();
        }
        m_cardM.DoHandsTransparency();
        m_Collider.enabled = false;
    }

    public void BringUpCard(bool isSelect)
    {
        int s_order;
        if (isSelect)
            s_order = renderPriority + 5;
        else
            s_order = renderPriority - 1;

        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].sortingOrder = s_order;
        textCanvas.sortingOrder = s_order;

        if (!isSelect)
        {
            m_Collider.enabled = true;
            if (ready)
            {
                OffCardAlphaAndReady();
            }
            
        }
    }

    public virtual GameObject GetTarget()
    {
        return target;
    }


    public void GetCardUseInfo(out int o_fixP, out float o_flucPRate)
    {
        o_fixP = fixP;
        o_flucPRate = flucPRate;
    }

    public bool GetReady()
    {
        return ready;
    }

    protected void FindBattleUIManger()
    {
        battleUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<BattleUIManager>();
    }

    protected void FindBSCardManager()
    {
        m_cardM = GameObject.FindGameObjectWithTag("CManager").GetComponent<BSCManager>();
    }

    protected void FindCostManager()
    {
        m_costM = GameObject.FindGameObjectWithTag("CostManager").GetComponent<CostManager>();
    }

    public void SortingCard(int usedRP, int cntCards)
    {
        int middle = Mathf.CeilToInt(cntCards / 2.0f);

        if(renderPriority > usedRP && usedRP != 0) // 0은 초기화나 턴 시작 시
        {
            renderPriority -= 1;
        }

        //m_sprRs[0].sortingOrder = renderPriority - 1;
        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].sortingOrder = renderPriority -1;
        textCanvas.sortingOrder = renderPriority - 1;

        Vector2 tempV = new Vector2(((renderPriority - middle) * moveP), 0);
        Quaternion tempQ = Quaternion.Euler(new Vector3(0, 0, (middle - renderPriority) * rotP));

        gameObject.transform.localPosition = tempV;
        gameObject.transform.localRotation = tempQ;
    }

    public bool GetIsNonTarget()
    {
        return isNonTarget;
    }

    public void SetRenderPriority(int value)
    {
        renderPriority = value;
    }

    public int GetRenderPriority()
    {
        return renderPriority;
    }

    public int GetCardID()
    {
        return cardID;
    }

    public void DoTransparency()
    {
        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, 0);
        //for (int i = 0; i < array_text.Length; i++)   // 어쩐지 배열식 접근은 안됨. public 으로 접근해도 막힘. 개별로 할 것.
        //    array_text[i].enabled = false;
        text_cost.color = new Color(text_cost.color.r, text_cost.color.g, text_cost.color.b, 0);
        text_plain.color = new Color(text_plain.color.r, text_plain.color.g, text_plain.color.b, 0);
        text_name.color = new Color(text_name.color.r, text_name.color.g, text_name.color.b, 0);
    }

    public void UndoTransparency()
    {
        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, 1.0f);
        text_cost.color = new Color(text_cost.color.r, text_cost.color.g, text_cost.color.b, 1.0f);
        text_plain.color = new Color(text_plain.color.r, text_plain.color.g, text_plain.color.b, 1.0f);
        text_name.color = new Color(text_name.color.r, text_name.color.g, text_name.color.b, 1.0f);
    }
}
