using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUF_RandomAbCond : CUF_Base
{
    [Header("Abnormal Condition Setting")]
    public int ab_ID = 0;
    public bool isImidiateAbActive = false;
    public bool isOnlyUseSecondFixP = false;
    public bool isSelfTarget = false;
    public int sec_fixP = 1;

    [Header("Random Condition Setting")]
    public int min_AbID = 0;
    public int max_AbID = 8;
    private int[] randRes;
    private bool isRepeat = false;
    private int callCnt = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    public override void Use(int diceValue)
    {
        dv = diceValue;
        AbCondition ab_target;
        if (isSelfTarget)
        {
            ab_target = GameObject.FindGameObjectWithTag("Player").GetComponent<AbCondition>();
        }
        else
        {
            target = myCard.GetTarget();
            ab_target = target.GetComponent<AbCondition>();
        }
        if (ab_target == null)
        {
            Debug.LogError("(상태이상 실패) 타겟의 상태이상 관리자 정보를 얻어오지 못했습니다.");
            return;
        }

        int dmg;
        if (isOnlyUseSecondFixP)
            dmg = sec_fixP;
        else if (isOnlyFixed)
            dmg = fixP;
        else if (isOnlyDiceValue)
            dmg = Mathf.RoundToInt(diceValue * flucPRate);
        else if (isSecondDmgFormula)
            dmg = Mathf.RoundToInt((fixP + diceValue) * flucPRate);
        else
            dmg = fixP + Mathf.RoundToInt(diceValue * flucPRate);

        int rand = Random.Range(min_AbID, max_AbID);

        if (isRepeat)
        {
            randRes[callCnt] = rand;
            bool isSucess = false;
            if (callCnt > 0)
            {
                while (!isSucess)
                {
                    bool isS2 = true;
                    for (int i = 0; i < callCnt; i++)
                    {
                        if (randRes[i] == rand)
                            isS2 = false;
                    }
                    if (isS2)
                        isSucess = true;
                    else
                        rand = Random.Range(min_AbID, max_AbID);
                }
            }
            callCnt++;
        }

        if (isImidiateAbActive)
            ab_target.AddImdiateAbCondition(rand, dmg);
        else
            ab_target.AddAbCondition(rand, dmg);
    }

    public override void ReUse()
    {
        Use(dv);
    }

    public void SetRandRes(int max)
    {
        isRepeat = true;
        randRes = new int[max];
        for (int i = 0; i < max; i++)
            randRes[i] = -1;
    }
}
