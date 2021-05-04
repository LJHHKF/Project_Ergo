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

        myCard.use += this.Use;
    }

    public override void Use(int diceValue)
    {
        dv = diceValue;
        if (isAllAttak)
        {
            EnemiesManager.instance.AddAllTargeted(ref target_list);
        }
        else if (!isAllAttak)
        {
            int targetNum = maxTarget;
            if (targetNum > EnemiesManager.instance.GetMaxMonsCnt())
            {
                targetNum = EnemiesManager.instance.GetMaxMonsCnt();
            }

            if (isTargetOverlapped)
            {
                EnemiesManager.instance.AddMultiRTarget_Overlaped(ref target_list, targetNum);
            }
            else
            {
                EnemiesManager.instance.AddMultiRTarget_NotOverlaped(ref target_list, targetNum);
            }
        }

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

        for (int i = 0; i < target_list.Count; i++)
        {
            int _i = i;
            StartCoroutine(delayedAffect(() => target_list[_i].GetComponent<LivingEntity>().OnDamage(dmg)));
        }
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
