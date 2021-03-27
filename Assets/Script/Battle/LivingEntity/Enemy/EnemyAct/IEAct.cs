using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEAct
{
    void Act();

    int GetActTypeNumber(); //이하 ActInfo 구분용도. 기본은 0, 상태이상 포함이면 1
    //bool GetIsAbCondAct();

    Sprite GetActSprite();

    void GetActInfo(out int power);

    //void GetActInfo(out int power, out bool isAll);

    //void GetActInfo(out int power, out int abCoIndex, out bool isAll);
}
