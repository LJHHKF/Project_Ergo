using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text;
using Card;

namespace Card
{
    public enum CardType
    {
        Sword,
        Magic
    }
}

public class Card_Base : MonoBehaviour, ICard
{
    [Header("Card Base Setting")]
    [SerializeField] protected int cardID = 0;
    [SerializeField] protected string cardName;
    [SerializeField] protected int cost = 1;
    [SerializeField] protected int fixP = 1;
    protected int r_fixP = 0;
    [SerializeField] protected float flucPRate = 1.0f;
    [SerializeField] protected bool isNonTarget = false;
    [SerializeField] protected bool isFixGuard = false;
    [SerializeField] protected CardType type;
    [TextArea]
    [SerializeField] protected string cardText;

    [Header("renderPriority Setting")]
    [SerializeField]protected float rotP = 2f;
    [SerializeField] protected float x_moveP = 1f;
    [SerializeField] protected float y_heightP = 0f;
    protected int renderPriority = 1;
    protected bool ready = true;
    [SerializeField] protected float handHeightPoint = -2.5f;
    [SerializeField] protected float readyAlpha = 0.5f;

    protected GameObject target;
    protected DiceSystemManager diceManager;
    protected BattleUIManager battleUIManager;
    protected bool isThrowed = false;
    protected SpriteRenderer[] m_sprRs = new SpriteRenderer[2];
    protected BoxCollider2D m_Collider;
    protected Canvas textCanvas;
    protected TextMeshProUGUI text_cost;
    protected Image costImg;
    protected TextMeshProUGUI text_plain;
    protected TextMeshProUGUI text_name;
    protected TextMeshProUGUI[] array_text;
    protected BSCManager m_cardM;
    protected CostManager m_costM;
    protected Character m_charM;

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
        textCanvas = gameObject.transform.Find("TextCanvas").GetComponent<Canvas>();
        costImg = textCanvas.gameObject.transform.Find("CostImage").GetComponent<Image>();
        text_cost = costImg.transform.Find("CostText").GetComponent<TextMeshProUGUI>();
        text_plain = textCanvas.gameObject.transform.Find("CardText").GetComponent<TextMeshProUGUI>();
        text_name = textCanvas.gameObject.transform.Find("CardName").GetComponent<TextMeshProUGUI>();
        array_text = textCanvas.gameObject.GetComponents<TextMeshProUGUI>();

        m_Collider = gameObject.GetComponent<BoxCollider2D>();

        m_sprRs[0] = gameObject.GetComponent<SpriteRenderer>();
        m_sprRs[1] = gameObject.transform.Find("CardImage").GetComponent<SpriteRenderer>();

        text_cost.text = cost.ToString();
        StringBuilder sb = new StringBuilder(cardText);
        sb.Replace("()", $"({r_fixP})");
        sb.Replace("(변동치)", flucPRate.ToString());
        text_plain.text = sb.ToString();
        text_name.text = cardName;
        ready = false;

        GameMaster.instance.battleStageStart += Event_BattleStageStart;
    }

    private void Event_BattleStageStart(object _o, EventArgs _e)
    {
        ChkAndFindBSCardManager();

        if (m_charM == null)
        {
            m_charM = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();

            if (isFixGuard)
            {
                r_fixP = fixP + Mathf.RoundToInt(m_charM.solid * 0.5f);
            }
            else if (type == CardType.Sword)
            {
                r_fixP = fixP + Mathf.RoundToInt(m_charM.strength * 0.5f);
            }
            else if (type == CardType.Magic)
            {
                r_fixP = fixP + Mathf.RoundToInt(m_charM.intel * 0.5f);
            }
        }


        ChkAndFindBattleUIManger();
        ChkAndFindCostManager();
    }

    protected virtual void OnEnable()
    {
        m_Collider.enabled = true;
        UndoTransparency();
    }


    public virtual ICard Selected()
    {
        ChkAndFindCostManager();

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
        costImg.color = new Color(costImg.color.r, costImg.color.g, costImg.color.b, (costImg.color.a * readyAlpha));
    }

    protected virtual void OffCardAlphaAndReady()
    {
        ready = false;
        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, (m_sprRs[i].color.a / readyAlpha));
        for (int i = 0; i < array_text.Length; i++)
            array_text[i].faceColor = new Color32((byte)array_text[i].color.r, (byte)array_text[i].color.g, (byte)array_text[i].color.b, (byte)(array_text[i].color.a / readyAlpha));
        costImg.color = new Color(costImg.color.r, costImg.color.g, costImg.color.b, (costImg.color.a / readyAlpha));
    }

    public virtual void Use(int diceValue)
    {

        ChkAndFindCostManager();
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

        ChkAndFindCharcter();
        m_charM.OnCardUseAnimation(type);

        ChkAndFindBSCardManager();
        m_cardM.MoveToGrave_Hand(gameObject);

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


        ChkAndFindBattleUIManger();

        battleUIManager.OnDiceSysetm();

        ChkAndFindBSCardManager();

        m_cardM.DoHandsTransparency();
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
        ChkAndFindCharcter();

        if (isFixGuard)
        {
            r_fixP = fixP + Mathf.RoundToInt(m_charM.solid * 0.5f);
        }
        else if (type == CardType.Sword)
        {
            r_fixP = fixP + Mathf.RoundToInt(m_charM.strength * 0.5f);
        }
        else if (type == CardType.Magic)
        {
            r_fixP = fixP + Mathf.RoundToInt(m_charM.intel * 0.5f);
        }

        o_fixP = r_fixP;
        o_flucPRate = flucPRate;
    }

    public bool GetReady()
    {
        return ready;
    }

    protected void ChkAndFindBattleUIManger()
    {
        if (battleUIManager == null)
        {
            battleUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<BattleUIManager>();
        }
    }

    protected void ChkAndFindBSCardManager()
    {
        if (m_cardM == null)
        {
            m_cardM = GameObject.FindGameObjectWithTag("CManager").GetComponent<BSCManager>();
        }
    }

    protected void ChkAndFindCostManager()
    {
        if (m_costM == null)
        {
            m_costM = GameObject.FindGameObjectWithTag("CostManager").GetComponent<CostManager>();
        }
    }

    protected void ChkAndFindCharcter()
    {
        if(m_charM == null)
        {
            m_charM = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        }
    }

    public void SortingCard(int usedRP, int cntCards)
    {
        int middle = Mathf.CeilToInt(cntCards / 2.0f);

        if(renderPriority > usedRP && usedRP != 0) // 0은 초기화나 턴 시작 시
        {
            renderPriority -= 1;
        }

        //m_sprRs[0].sortingOrder = renderPriority - 1;
        //for (int i = 0; i < m_sprRs.Length; i++)
        //    m_sprRs[i].sortingOrder = renderPriority -1;
        textCanvas.sortingOrder = renderPriority;
        m_sprRs[0].sortingOrder = renderPriority;
        m_sprRs[1].sortingOrder = renderPriority - 1;
        Vector2 tempV = new Vector2(((renderPriority - middle) * x_moveP), y_heightP);
        Quaternion tempQ = Quaternion.Euler(new Vector3(0, 0, (middle - renderPriority) * rotP));
        gameObject.transform.localPosition = tempV;
        gameObject.transform.localRotation = tempQ;

        if (m_charM == null)
        {
            m_charM = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        }

        if (isFixGuard)
        {
            r_fixP = fixP + Mathf.RoundToInt(m_charM.solid * 0.5f);
        }
        else if (type == CardType.Sword)
        {
            r_fixP = fixP + Mathf.RoundToInt(m_charM.strength * 0.5f);
        }
        else if (type == CardType.Magic)
        {
            r_fixP = fixP + Mathf.RoundToInt(m_charM.intel * 0.5f);
        }
        StringBuilder sb = new StringBuilder(cardText);
        sb.Replace("()", $"({r_fixP})");
        sb.Replace("(변동치)", flucPRate.ToString());
        text_plain.text = sb.ToString();
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
        costImg.color = new Color(costImg.color.r, costImg.color.g, costImg.color.b, 0);

        m_Collider.enabled = false;
    }

    public void UndoTransparency()
    {
        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, 1.0f);
        text_cost.color = new Color(text_cost.color.r, text_cost.color.g, text_cost.color.b, 1.0f);
        text_plain.color = new Color(text_plain.color.r, text_plain.color.g, text_plain.color.b, 1.0f);
        text_name.color = new Color(text_name.color.r, text_name.color.g, text_name.color.b, 1.0f);
        costImg.color = new Color(costImg.color.r, costImg.color.g, costImg.color.b, 1.0f);

        m_Collider.enabled = true;
    }

    //private int cardID = 0;
    //private int cost = 1;
    //private int fixP = 1;
    //private int r_fixP = 0;
    //private float flucPRate = 1.0f;
    //private Sprite cardImage;
    //private string cardText;

    public void CopyUIInfo(out int _cardID,out string _name ,out int _cost, out int _fixP, out float _flucPRate, out Sprite _cardImage, out Sprite _cardOuterImage ,out string _cardText)
    {
        _cardID = cardID;
        _name = cardName;
        _cost = cost;
        _fixP = fixP;
        _flucPRate = flucPRate;
        _cardText = cardText;
        _cardOuterImage = gameObject.GetComponent<SpriteRenderer>().sprite;
        _cardImage = gameObject.transform.Find("CardImage").GetComponent<SpriteRenderer>().sprite;
    }

}
