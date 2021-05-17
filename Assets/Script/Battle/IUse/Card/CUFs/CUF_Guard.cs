using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CUF_Guard : CUF_Base
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        myCard.use += this.Use;
    }

    public override void Use(int diceValue)
    {
        dv = diceValue;
        target = GameObject.FindGameObjectWithTag("Player").gameObject;
        LivingEntity liv_target = target.GetComponent<LivingEntity>();
        if(liv_target == null)
        {
            Debug.LogError("(방어실패) 타겟의 생명체 정보를 얻어오지 못했습니다.");
            return;
        }

        int gv;
        if (isOnlyFixed)
            gv = fixP;
        else if (isOnlyDiceValue)
            gv = Mathf.RoundToInt(diceValue * flucPRate);
        else if (isSecondDmgFormula)
            gv = Mathf.RoundToInt((fixP + diceValue) * flucPRate);
        else
            gv = fixP + Mathf.RoundToInt(diceValue * flucPRate);

        StartCoroutine(delayedAffect(() => liv_target.AddGuardPoint(gv)));
    }
    public override void ReUse()
    {
        this.Use(dv);
    }

    IEnumerator delayedAffect(Action _action)
    {
        yield return new WaitForSeconds(affectDelay);
        _action.Invoke();
        yield break;
    }
}
