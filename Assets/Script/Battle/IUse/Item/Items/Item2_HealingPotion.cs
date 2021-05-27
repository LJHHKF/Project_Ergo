using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item2_HealingPotion : Item_Base
{
    private Character m_char;
    public override void SetTarget(GameObject _target)
    {
        target = GameObject.FindGameObjectWithTag("Player");
        m_char = target.GetComponent<Character>();
        Use(0);
    }
    public override void Use(int _dummy)
    {
        m_char.RestoreHealth(itemPower);
        m_char.OnHealEffect();
        base.Use(_dummy);
    }
}
