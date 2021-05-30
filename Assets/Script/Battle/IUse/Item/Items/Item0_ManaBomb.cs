using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item0_ManaBomb : Item_Base
{
    private List<GameObject> multiTarget = new List<GameObject>();
    private GlobalCardEffectM cardEffectM;

    public override void SetTarget(GameObject _target)
    {
        EnemiesManager.instance.AddAllTargeted(ref multiTarget);
        Use(0);
    }

    public override void Use(int _dummy)
    {
        if(cardEffectM == null)
            cardEffectM = GameObject.FindGameObjectWithTag("GlobalCardEffect").GetComponent<GlobalCardEffectM>();
        for(int i = 0; i < multiTarget.Count; i++)
        {
            int _i = i;
            multiTarget[_i].GetComponent<Enemy_Base>().OnDamage(itemPower);
        }
        cardEffectM.OnManaBombEffect();
        base.Use(_dummy);
    }
}
