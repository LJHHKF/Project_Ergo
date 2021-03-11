using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_NonTarget : Card_Base
{
    protected bool ready = false;


    public override void Dragged(Vector2 mousePos, LineDrawer liner)
    {
        base.Dragged(mousePos, liner);
        //마우스 위치가 손패 위치서 벗어났느냐, 아니냐로 ReadyToUse 및 그 해제화 작업 필요.
    }


    protected virtual void ReadyToUse()
    {
        ready = true;
        //투명화 처리 필요
    }

    public override void SetTarget(GameObject input)
    {
        // 기본적으로 input과는 무관하게 돌릴것. non-Target, 기본 타겟이 있음.
    }
}
