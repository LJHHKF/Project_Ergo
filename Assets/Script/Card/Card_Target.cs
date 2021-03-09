using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Target : MonoBehaviour, ICard
{
    public int cost = 1;
    public int fixP = 1;
    public float flucPRate = 1.5f;
    public Sprite cardImage;
    [TextArea]
    public string[] cardText;
    private LivingEntity target;
    private DiceSystemManager diceManager;
    private bool isThrowed = false;

    [HideInInspector]
    public int posNum = 0; // -1은 묘지, 0은 덱, 1 이상의 수들은 손패
    public Vector2 m_Position
    {
        get{
            return m_Position;
        }
        protected set{
            m_Position = gameObject.transform.position;
        }
    }



    protected virtual void Start()
    {
        FindDiceManager();
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

    public virtual void Use()
    {
        if(diceManager == null)
        {
            FindDiceManager();
        }

        if(diceManager != null)
        {
            diceManager.ActiveDice(out isThrowed);
            if(!isThrowed)
            {
                //오류처리
            }
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
        target = input.GetComponent<LivingEntity>(); // 임시 코드. LivingEntity를 상속하는 Enemy 를 받을 것.
        if (target != null)
        {

        }
    }

    private void FindDiceManager()
    {
        diceManager = GameObject.FindGameObjectWithTag("DiceBox").GetComponent<DiceSystemManager>();
    }
}
