using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item0_ManaBomb : Item_Base
{
    private List<GameObject> multiTarget = new List<GameObject>();

    public override void SetTarget(GameObject _target)
    {
        EnemiesManager.instance.AddAllTargeted(ref multiTarget);
        Use(0);
    }

    public override void Use(int _dummy)
    {
        for(int i = 0; i < multiTarget.Count; i++)
        {
            int _i = i;
            multiTarget[_i].GetComponent<Enemy_Base>().OnDamage(itemPower);
        }
        base.Use(_dummy);
    }
}
