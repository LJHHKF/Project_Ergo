using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CUF_TargetAttack : CUF_Base
{
    //Card Use Function

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        myCard.use += this.Use;
    }

    public override void Use(int diceValue)
    {
        dv = diceValue;
        target = myCard.GetTarget();
        LivingEntity liv_target = target.GetComponent<LivingEntity>();
        if(liv_target == null)
        {
            Debug.LogError("(공격실패) 타겟의 생명체 정보를 얻어오지 못했습니다.");
            return;
        }

        int dmg;
        if (isOnlyFixed)
            dmg = fixP;
        else if (isOnlyDiceValue)
            dmg = Mathf.RoundToInt(diceValue * flucPRate);
        else if (isSecondDmgFormula)
            dmg = Mathf.RoundToInt((fixP + diceValue) * flucPRate);
        else
            dmg = fixP + Mathf.RoundToInt(diceValue * flucPRate);

        StartCoroutine(delayedAffect(() => liv_target.OnDamage(dmg)));
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
