using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEM_ManaStorm : ICardEffectM
{
    private GlobalCardEffectM cardEffectM;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (StageManager.instance.GetCurrentStageTypeIndex() == 0)
            StartCoroutine(DelayedInvoke(SetCardEffectM));
    }
            

    private void SetCardEffectM()
    {
        cardEffectM = GameObject.FindGameObjectWithTag("GlobalCardEffect").GetComponent<GlobalCardEffectM>();
    }

    public override void OnEffect()
    {
        if (cardEffectM == null)
            SetCardEffectM();

        cardEffectM.OnManaStormEffect();
    }
}
