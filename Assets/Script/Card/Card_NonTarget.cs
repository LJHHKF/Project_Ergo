using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_NonTarget : MonoBehaviour, ICard
{
    public int cost = 1;
    public int fixP = 1;
    public float flucPRate = 1.5f;
    public Sprite cardImage;
    [TextArea]
    public string[] cardText;

    [HideInInspector]
    public int posNum = 0; // -1은 묘지, 0은 덱, 1 이상의 수들은 손패
    protected bool ready = false;



    void Start()
    {

    }

    void Update()
    {

    }

    public virtual ICard Selected()
    {
        return this;
    }

    public virtual void Holded()
    {
        //카드 정보 넘겨주기
    }

    public virtual void Dragged(Vector2 mousePos)
    {
        //마우스 위치가 손패 위치서 벗어났느냐, 아니냐로 ReadyToUse 및 그 해제화 작업 필요.
    }

    public virtual void Use()
    {
        //다이스롤 효과를 앞서 받은 다음에 불려져야 함.
        //선택된 타겟에 효과 발동
    }

    public virtual void UnUse()
    {
        //사용 취소하고 리셋. 
    }

    protected void ReadyToUse()
    {
        ready = true;
        //투명화 처리 필요
    }
}
