using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CUF_MultiRTargetAttack : CUF_Base
{
    //R = Random

    [Header("Multi Target Set")]
    [SerializeField] protected bool isAllAttak = false;
    [SerializeField] protected bool isTargetOverlapped = false;
    [SerializeField] protected int maxTarget = 2;

    private List<GameObject> target_list = new List<GameObject>();

    private void OnDisable()
    {
        target_list.Clear();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if(!isUseRepeat)
            myCard.use += this.Use;
    }

    public override void Use(int diceValue)
    {
        dv = diceValue;

        int dmg;
        if (isOnlyFixed)
            dmg = fixP;
        else if (isOnlyDiceValue)
            dmg = Mathf.RoundToInt(diceValue * flucPRate);
        else if (isSecondDmgFormula)
        {
            dmg = Mathf.RoundToInt((fixP + diceValue) * flucPRate);
        }
        else
            dmg = fixP + Mathf.RoundToInt(diceValue * flucPRate);

        if (isAllAttak)
        {
            StartCoroutine(delayedAffect(() => EnemiesManager.instance.AllDamaged(dmg))); // 원래는 타겟 리스트를 가져와서 반복문 돌렸는데, 2회차때부터 오류가 나기 시작해서 수정.
        }
        else if (!isAllAttak)
        {
            int targetNum = maxTarget;
            if (targetNum > EnemiesManager.instance.GetCurMonsCnt())
            {
                targetNum = EnemiesManager.instance.GetCurMonsCnt();
            }

            if (isTargetOverlapped)
            {
                EnemiesManager.instance.AddMultiRTarget_Overlaped(ref target_list, maxTarget);
            }
            else
            {
                EnemiesManager.instance.AddMultiRTarget_NotOverlaped(ref target_list, targetNum);
            }

            for (int i = 0; i < target_list.Count; i++)
            {
                int _i = i;
                StartCoroutine(delayedAffect(() => target_list[_i]?.GetComponent<LivingEntity>().OnDamage(dmg)));
            }
        }
    }

    public override void ReUse()
    {
        Use(dv);
    }
}
