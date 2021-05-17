using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item2_HealingPotion : Item_Base
{
    public override void SetTarget(GameObject _target)
    {
        target = GameObject.FindGameObjectWithTag("Player");
        Use(0);
    }
    public override void Use(int _dummy)
    {
        target.GetComponent<Character>().RestoreHealth(itemPower);
        base.Use(_dummy);
    }
}
