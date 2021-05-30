using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEM_PoisonShot : ICardEffectM
{
    private Enemy_Base target;
    private Card_Base m_card;
    // Start is called before the first frame update
    void Start()
    {
        m_card = gameObject.GetComponent<Card_Base>();
        m_card.ev_setTarget += SetTarget;
    }

    private void SetTarget(GameObject _t)
    {
        target = _t.GetComponent<Enemy_Base>();
    }

    public override void OnEffect()
    {
        target.OnHit_PoisonShot();
    }
}
