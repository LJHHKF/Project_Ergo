using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUF_Guard : CUF_Base
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        myCard.use += Use;
    }

    void Use(int diceValue)
    {
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
        else
            gv = fixP + Mathf.RoundToInt(diceValue * flucPRate);


        liv_target.GetGuardPoint(gv);
    }
}
