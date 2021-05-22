using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CUF_AbCondition : CUF_Base
{
    [Header("Abnormal Condition Setting")]
    [SerializeField] protected int ab_ID = 0;
    [SerializeField] protected bool isImidiateAbActive = false;
    [SerializeField] protected bool isOnlyUseSecondFixP = false;
    [SerializeField] protected bool isSelfTarget = false;
    [SerializeField] protected int sec_fixP = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        myCard.use += this.Use;
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

        if (isImidiateAbActive)
            StartCoroutine(delayedAffect(() => ab_target.AddImdiateAbCondition(ab_ID, dmg)));
        else
            StartCoroutine(delayedAffect(() => ab_target.AddDelayedCondition(ab_ID, dmg)));

        
    }

    public override void ReUse()
    {
        this.Use(dv);
    }
}
