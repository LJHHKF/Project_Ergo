using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffectManager : MonoBehaviour
{
    [Header("Card Hit Effect Setting")]
    [SerializeField] protected GameObject Hit_HitAndRun;
    [SerializeField] protected GameObject Hit_HalfSwording;
    [SerializeField] protected GameObject Hit_PoisonShot;
    [SerializeField] protected GameObject Hit_BlowShot;
    [SerializeField] protected GameObject BulkUpEffect;


    // Start is called before the first frame update
    void Start()
    {
        Hit_HitAndRun.SetActive(false);
        Hit_HalfSwording.SetActive(false);
        Hit_PoisonShot.SetActive(false);
        Hit_BlowShot.SetActive(false);
        BulkUpEffect.SetActive(false);
    }

    public void OnHit_HitAndRun()
    {
        if (!Hit_HitAndRun.activeSelf)
        {
            Hit_HitAndRun.SetActive(true);
            StartCoroutine(DeleyedUnActive(Hit_HitAndRun));
        }
    }

    public void OnHit_HalfSwording()
    {
        if (!Hit_HalfSwording.activeSelf)
        {
            Hit_HalfSwording.SetActive(true);
            StartCoroutine(DeleyedUnActive(Hit_HalfSwording));
        }
    }

    public void OnHit_PoisonShot()
    {
        if (!Hit_PoisonShot.activeSelf)
        {
            Hit_PoisonShot.SetActive(true);
            StartCoroutine(DeleyedUnActive(Hit_PoisonShot));
        }
    }

    public void OnHit_BlowShot()
    {
        if(!Hit_BlowShot.activeSelf)
        {
            Hit_BlowShot.SetActive(true);
            StartCoroutine(DeleyedUnActive(Hit_BlowShot));
        }
    }

    public void OnBulkUpEffect()
    {
        if(!BulkUpEffect.activeSelf)
        {
            BulkUpEffect.SetActive(true);
            StartCoroutine(DeleyedUnActive(BulkUpEffect));
        }
    }

    private IEnumerator DeleyedUnActive(GameObject go)
    {
        yield return new WaitForSeconds(1.0f);
        go.SetActive(false);
        yield break;
    }
}
