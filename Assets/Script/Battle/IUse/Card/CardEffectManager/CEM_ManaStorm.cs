using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEM_ManaStorm : ICardEffectM
{
    private GlobalCardEffectM cardEffectM;

    // Start is called before the first frame update
    void OnEnable()
    {
        cardEffectM = GameObject.FindGameObjectWithTag("GlobalCardEffect").GetComponent<GlobalCardEffectM>();
    }

    public override void OnEffect()
    {
        cardEffectM.OnManaStormEffect();
    }
}
