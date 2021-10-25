using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemInfoManager : MonoBehaviour
{
    private static ItemInfoManager m_instance;
    public static ItemInfoManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<ItemInfoManager>();
            return m_instance;
        }
    }

    [Serializable]
    public struct itemPrefab
    {
        public int id;
        public GameObject prefab;
        public int dropRate;
    }

    [SerializeField] private itemPrefab[] items;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
    }

    public GameObject GetItem(int _index)
    {
        return items[_index].prefab;
    }

    public GameObject GetItem_Random()
    {
        int sum_rate = 0;
        for (int i = 0; i < items.Length; i++)
        {
            int _i = i;
            sum_rate += items[_i].dropRate;
        }
        int rand = UnityEngine.Random.Range(0, sum_rate);
        GameObject temp = null;
        sum_rate = 0;
        for (int i = 0; i < items.Length; i++)
        {
            int _i = i;
            sum_rate += items[_i].dropRate;
            if (rand >= sum_rate - items[_i].dropRate && rand < sum_rate)
            {
                temp = items[_i].prefab;
                break;
            }
        }

        if (temp == null)
        {
            Debug.Log("아이템 랜덤 값을 제대로 얻지 못했습니다.");
            return items[0].prefab;
        }
        else
            return temp;
    }
}
