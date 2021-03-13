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

    [Header("renderPriority Setting")]
    public int renderPriority = 1;
    public int cntCards = 2; // 손패 관리자가 나오면 private로 돌리고 얻어오는 방식으로 할 것.
    public float rotP = 2f;
    public float moveP = 1f;

    protected GameObject target;
    protected DiceSystemManager diceManager;
    protected BattleUIManager battleUIManager;
    protected bool isThrowed = false;
    protected SpriteRenderer[] m_sprRs = new SpriteRenderer[2];
    protected BoxCollider2D m_Collider;
    protected TextMesh costText;
    protected MeshRenderer costMeshR;

    [HideInInspector]
    public int posNum = 0; // -1은 묘지, 0은 덱, 1 이상의 수들은 손패
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
    }

    protected virtual void Start()
    {
        FindBattleUIManger();
        m_sprRs[0] = gameObject.GetComponent<SpriteRenderer>();
        m_sprRs[1] = gameObject.transform.Find("CardImage").GetComponent<SpriteRenderer>();
        costText = gameObject.transform.Find("CostText").GetComponent<TextMesh>();
        costMeshR = costText.gameObject.GetComponent<MeshRenderer>();
        costMeshR.sortingLayerName = m_sprRs[0].sortingLayerName;
        costText.text = cost.ToString();

        InitSorting();
    }

    protected virtual void Update()
    {

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
        cntCards -= 1; // 이 부분도, 나중엔 핸드 관리자에게서 얻어오는 걸로 바꿀 것.
        SortingCard(renderPriority); // 이 호출부는 아예 핸드 관리자에게 넘겨야 함. 단체로 체크해야하므로.

        //이하 구현은 상속된 곳에서.
        //다이스롤 효과를 앞서 받은 다음에 불려져야 함.
        //선택된 타겟에 효과 발동


        gameObject.SetActive(false); // 덱 및 묘지 시스템 완성하면 이를 덱으로 옮기는걸로 바꿀 것.
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
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }


    protected void FindBattleUIManger()
    {
        battleUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<BattleUIManager>();
    }

    public void SortingCard(int usedRP)
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

    protected void InitSorting()
    {
        int middle = Mathf.CeilToInt(cntCards / 2.0f);

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

    public int GetRenderPriority()
    {
        return renderPriority;
    }

    public int GetCardID()
    {
        return cardID;
    }
}
