using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalCardEffectM : MonoBehaviour
{
    [Header("Set Effects")]
    [SerializeField] private GameObject ManaStormEffect;
    [SerializeField] private GameObject ManaBombEffect_prefab;

    [Header("Effects value set")]
    [SerializeField] private float manabomb_y = 1.0f;

    private List<GameObject> list_manaBomb = new List<GameObject>();

    void Start()
    {
        ManaStormEffect.SetActive(false);
    }

    public void OnManaStormEffect()
    {
        if (!ManaStormEffect.activeSelf)
        {
            ManaStormEffect.SetActive(true);
            ManaStormEffect.transform.position = new Vector2(EnemiesManager.instance.GetMonterListMiddle_x(), ManaStormEffect.transform.position.y);
            StartCoroutine(DelayedUnActive(ManaStormEffect));
        }
    }

    public void OnManaBombEffect()
    {
        if (list_manaBomb.Count == 0)
            Create();
        else
        {
            for(int i = 0; i < list_manaBomb.Count; i++)
            {
                int _i = i;
                if(!list_manaBomb[_i].activeSelf)
                {
                    Active(list_manaBomb[_i]);
                    return;
                }
            }
            Create();
        }

        void Create()
        {
            GameObject temp = Instantiate(ManaBombEffect_prefab, transform);
            list_manaBomb.Add(temp);
            Active(temp);
        }

        void Active(GameObject _t)
        {
            _t.SetActive(true);
            _t.transform.position = new Vector2(EnemiesManager.instance.GetMonterListMiddle_x(), manabomb_y);
            StartCoroutine(DelayedUnActive(_t));
        }
    }

    IEnumerator DelayedUnActive(GameObject go)
    {
        yield return new WaitForSeconds(1.0f);
        go.SetActive(false);
        yield break;
    }
}
