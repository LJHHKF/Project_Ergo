using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item1_SpecialThrowStone : Item_Base
{
    public override void SetTarget(GameObject _target)
    {
        target = _target;
        Use(0);
    }

    public override void Use(int _dummy)
    {
        target.GetComponent<Enemy_Base>().DestroyGuard();
        base.Use(_dummy);
    }
}
