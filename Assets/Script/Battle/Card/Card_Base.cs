using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Base : MonoBehaviour, ICard
{
    public int cost = 1;
    public int fixP = 1;
    public float flucPRate = 1.5f;
    public Sprite cardImage;
    [TextArea]
    public string[] cardText;
    protected GameObject target;
    protected DiceSystemManager diceManager;
    protected BattleUIManager battleUIManager;
    protected bool isThrowed = false;

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

    protected virtual void Start()
    {
        FindBattleUIManger();
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
        liner.SetLine(gameObject.transform, mousePos);
    }

    public virtual void Use(int diceValue)
    {
        if (battleUIManager == null)
        {
            FindBattleUIManger();
        }


        //다이스롤 효과를 앞서 받은 다음에 불려져야 함.
        //선택된 타겟에 효과 발동
    }

    public virtual void UnUse()
    {
        //사용 취소하고 리셋. 
    }

    protected void DrawLine()
    {
        //화살표선 긋기용 함수
    }

    public virtual void SetTarget(GameObject input)
    {
        target = input;
    }


    protected void FindBattleUIManger()
    {
        battleUIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<BattleUIManager>();
    }
}
