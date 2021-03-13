using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Type type;
    public bool isNonTarget = false;
    public Sprite cardImage;
    [TextArea]
    public string[] cardText;

    //[Header("renderPriority Setting")]
    protected int renderPriority = 1;
    protected float rotP;
    protected float moveP;

    protected GameObject target;
    protected DiceSystemManager diceManager;
    protected BattleUIManager battleUIManager;
    protected bool isThrowed = false;
    protected SpriteRenderer[] m_sprRs = new SpriteRenderer[2];
    protected BoxCollider2D m_Collider;
    protected TextMesh costText;
    protected MeshRenderer costMeshR;
    protected BSCManager m_cardM;
    protected TurnManager m_turnM;

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
        if (costText == null)
        {
            costText = gameObject.transform.Find("CostText").GetComponent<TextMesh>();
        }

        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, 1.0f);
        costText.color = new Color(costText.color.r, costText.color.g, costText.color.b, 1.0f);
    }

    protected virtual void Start()
    {
        FindBattleUIManger();
        FindTurnManager();
        FindBSCardManager();
        costMeshR = costText.gameObject.GetComponent<MeshRenderer>();
        costMeshR.sortingLayerName = m_sprRs[0].sortingLayerName;
        costText.text = cost.ToString();
        m_cardM.SetCardSortingValue(out rotP, out moveP);
    }


    //public virtual ICard Selected()
    //{
    //    return this;
    //}

    public virtual void Holded()
    {
        //카드 정보 넘겨주기
    }

    public virtual void Dragged(Vector2 mousePos, LineDrawer liner)
    {
        //선택된 카드 투명화, 카드 위치 -> 타겟 위치 선 연결 준비
        //타겟 위치는 업데이트에서 받아올 것.
        liner.SetLine_Worlds(gameObject.transform, mousePos);
        
        //Debug.Log(mousePos); //논타겟 카드의 손패와 필드 영역 벗어남 구분하기 위해 y값 확인
    }

    public virtual void Use(int diceValue)
    {
        m_cardM.MoveToGrave(gameObject);
        if(m_turnM == null)
        {
            FindTurnManager();
        }
        m_turnM.OnPlayerTurnEnd();
    }

    protected void DrawLine()
    {
        //화살표선 긋기용 함수
    }

    public virtual void SetTarget(GameObject input)
    {
        target = input;

        if (battleUIManager == null)
        {
            FindBattleUIManger();
        }
        battleUIManager.OnDiceSysetm(gameObject.transform.position);

        for (int i = 0; i < m_sprRs.Length; i++)
            m_sprRs[i].color = new Color(m_sprRs[i].color.r, m_sprRs[i].color.g, m_sprRs[i].color.b, 0);
        costText.color = new Color(costText.color.r, costText.color.g, costText.color.b, 0);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }


    protected void FindBattleUIManger()
    {
        battleUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<BattleUIManager>();
    }

    protected void FindBSCardManager()
    {
        m_cardM = GameObject.FindGameObjectWithTag("CManager").GetComponent<BSCManager>();
    }

    protected void FindTurnManager()
    {
        m_turnM = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
    }

    public void SortingCard(int usedRP, int cntCards)
    {
        int middle = Mathf.CeilToInt(cntCards / 2.0f);

        if(renderPriority > usedRP && usedRP != 0) // 0은 초기화 때만.
        {
            renderPriority -= 1;
        }
        m_sprRs[0].sortingOrder = renderPriority - 1;
        for (int i = 1; i < m_sprRs.Length; i++)
            m_sprRs[i].sortingOrder = renderPriority;
        costMeshR.sortingOrder = renderPriority;
        gameObject.transform.localPosition = new Vector2(((renderPriority - middle) * moveP), 0);
        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (middle - renderPriority) * rotP));
    }

    public bool GetIsNonTarget()
    {
        return isNonTarget;
    }

    public void SetRenderPriority(int value)
    {
        renderPriority = value;
    }

    public int GetCardID()
    {
        return cardID;
    }
}
