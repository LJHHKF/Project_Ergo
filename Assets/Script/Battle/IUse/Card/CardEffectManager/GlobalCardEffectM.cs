using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCardEffectM : MonoBehaviour
{
    [SerializeField] private GameObject ManaStormEffect;

    void Start()
    {
        ManaStormEffect.SetActive(false);
    }

    public void OnManaStormEffect()
    {
        if (!ManaStormEffect.activeSelf)
        {
            ManaStormEffect.SetActive(true);
            StartCoroutine(DelayedUnActive(ManaStormEffect));
        }
    }

    IEnumerator DelayedUnActive(GameObject go)
    {
        yield return new WaitForSeconds(1.0f);
        ManaStormEffect.SetActive(false);
        yield break;
    }
}
