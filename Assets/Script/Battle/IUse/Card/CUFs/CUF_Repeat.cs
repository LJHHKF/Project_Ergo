﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUF_Repeat : MonoBehaviour
{
    [SerializeField] private Card_Base myCard;
    [SerializeField] private int maxNum = 2;
    [SerializeField] private CUF_Base repeatTarget;
    [SerializeField] private float timeInterval = 1.0f;
    [SerializeField] private bool isUseFixP = false;
    [SerializeField] private bool isUseFlucP = false;
    [SerializeField] private bool isSecondDmgFormula = false;
    private int fixP = 1;
    private float flucPRate = 1.0f;
    private int dv = 0;

    // Start is called before the first frame update
    public void Start()
    {
        if (isUseFixP || isUseFlucP)
        {
            myCard.GetCardUseInfo(out fixP, out flucPRate);
            myCard.use += GetDVAndUse;
        }
        else
        {
            myCard.sub_use += () => StartCoroutine(DelayedUse(maxNum));
        }
    }

    private void GetDVAndUse(int diceValue)
    {
        dv = diceValue;

        int dmg;
        if (isUseFixP && isUseFlucP && isSecondDmgFormula)
            dmg = Mathf.RoundToInt((fixP + diceValue) * flucPRate);
        else if(isUseFixP && isUseFlucP)
            dmg = fixP + Mathf.RoundToInt(diceValue * flucPRate);
        else if (isUseFixP)
            dmg = fixP;
        else if (isUseFlucP)
            dmg = Mathf.RoundToInt(diceValue * flucPRate);
        else
            dmg = 0;

        StartCoroutine(DelayedUse(dmg));
    }

    IEnumerator DelayedUse(int maxNum)
    {
        int cnt = 1;
        while(cnt < maxNum)
        {
            yield return new WaitForSeconds(timeInterval);
            repeatTarget.ReUse();
            cnt++;
        }
        yield break;
    }
}