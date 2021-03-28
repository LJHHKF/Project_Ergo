using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUF_MultiRTargetAttack : CUF_Base
{
    //R = Random

    [Header("Multi Target Set")]
    [SerializeField] protected bool isAllAttak = false;
    [SerializeField] protected bool isTargetOverlapped = false;
    [SerializeField] protected int maxTarget = 2;

    private EnemiesManager e_manager;
    private List<GameObject> target_list = new List<GameObject>();

    private void OnDisable()
    {
        target_list.Clear();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        e_manager = GameObject.FindGameObjectWithTag("EnemiesManager").GetComponent<EnemiesManager>();

        myCard.use += this.Use;
    }

    public override void Use(int diceValue)
    {
        dv = diceValue;
        if (isAllAttak)
        {
            e_manager.AddAllTargeted(ref target_list);
        }
        else if (!isAllAttak)
        {
            int targetNum = maxTarget;
            if (targetNum > e_manager.GetMaxMonsCnt())
            {
                targetNum = e_manager.GetMaxMonsCnt();
            }

            if (isTargetOverlapped)
            {
                e_manager.AddMultiRTarget_Overlaped(ref target_list, targetNum);
            }
            else
            {
                e_manager.AddMultiRTarget_NotOverlaped(ref target_list, targetNum);
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
            target_list[i].GetComponent<LivingEntity>().OnDamage(dmg);
        }
    }

    public override void ReUse()
    {
        if (target != null)
        {
            this.Use(dv);
        }
    }
}
