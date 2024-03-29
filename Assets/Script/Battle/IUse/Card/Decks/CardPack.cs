﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class CardPack : MonoBehaviour
{
    public static CardPack instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<CardPack>();
            return m_instance;
        }
    }
    private static CardPack m_instance;

    [Serializable]
    struct CardField
    {
        public GameObject card_prefab;
        public int card_weight;
    }

    [Header("Pack Settincg")]
    [SerializeField] private int max_DuplicateValue = 3;
    private int saveID = 0;

    [Header("Card Registration")]
    [SerializeField] private CardField[] cards;
    private int[] cardIDs;
    private int[] card_HadCnt;
    private int[] tempHadCnt;
    private List<CardField> canList = new List<CardField>();

    private StringBuilder key = new StringBuilder();

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        canList.Capacity = cards.Length + 1;
        cardIDs = new int[cards.Length];
        card_HadCnt = new int[cards.Length];
        tempHadCnt = new int[cards.Length];
        TempHadCntReset();
    }

    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
    }

    private void OnEnable()
    {
        GameMaster.instance.gameOver += CardPackClear;
        GameMaster.instance.gameStop += SaveHadCnt;
    }

    private void OnDisable()
    {
        GameMaster.instance.gameOver -= CardPackClear;
        GameMaster.instance.gameStop -= SaveHadCnt;
    }

    public void CardPack_Start()
    {
        key.Clear();
        for (int i = 0; i < cards.Length; i++)
        {
            int _i = i;
            cardIDs[_i] = cards[_i].card_prefab.GetComponent<ICard>().GetID();

            if (_i == 0)
            {
                key.Append($"SaveID({saveID}).CardID({cardIDs[_i]}).HadCnt");
            }
            else
            {
                key.Clear();
                key.Append($"SaveID({saveID}).CardID({cardIDs[_i]}).HadCnt");
            }

            if (!PlayerPrefs.HasKey(key.ToString()))
            {
                card_HadCnt[_i] = 0;
                PlayerPrefs.SetInt(key.ToString(), card_HadCnt[_i]);
            }
            else
            {
                card_HadCnt[_i] = PlayerPrefs.GetInt(key.ToString());
            }
            tempHadCnt[_i] = 0;
        }
    }

    public void CardPack_Init()
    {
        key.Clear();
        for (int i = 0; i < cards.Length; i++)
        {
            int _i = i;
            cardIDs[_i] = cards[_i].card_prefab.GetComponent<ICard>().GetID();

            if (_i == 0)
            {
                key.Append($"SaveID({saveID}).CardID({cardIDs[_i]}).HadCnt");
            }
            else
            {
                key.Clear();
                key.Append($"SaveID({saveID}).CardID({cardIDs[_i]}).HadCnt");
            }

            card_HadCnt[_i] = 0;
            PlayerPrefs.SetInt(key.ToString(), card_HadCnt[_i]);
            tempHadCnt[_i] = 0;
        }
    }

    private void CardPackClear()
    {
        key.Clear();
        for(int i = 0; i < cards.Length; i++)
        {
            int _i = i;
            if (_i == 0)
                key.Append($"SaveID({saveID}).CardID({cardIDs[_i]}).HadCnt");
            else
                key.Replace($"CardID({cardIDs[i - 1]})", $"CardID({cardIDs[i]})");

            card_HadCnt[_i] = 0;
            PlayerPrefs.DeleteKey(key.ToString());
        }
    }

    //public int CardsArrayLength()
    //{
    //    return card_prefabs.Length;
    //}

    public void AddCard_OnlyHadData(int c_id)
    {
        int index = SearchIndexFromID(c_id);

        if(index == -1)
        {
            Debug.LogError("추가할 카드의 아이디와 일치하는 카드를 발견하지 못함");
            return;
        }

        if (card_HadCnt[index] < max_DuplicateValue)
        {
            card_HadCnt[index] += 1;
            SaveHadCnt();
        }
        else
        {
            Debug.Log($"소지 카드수가 이미 {max_DuplicateValue}을 넘었습니다.");
        }
    }

    public void DeleteCard_hadData(int c_id)
    {
        int index = SearchIndexFromID(c_id);

        if (index == -1)
        {
            Debug.LogError("추가할 카드의 아이디와 일치하는 카드를 발견하지 못함");
            return;
        }

        if(card_HadCnt[index] <= 0)
        {
            Debug.Log("삭제하려는 카드를 소지하고 있지 않습니다.");
        }
        else
        {
            card_HadCnt[index] -= 1;
            SaveHadCnt();
        }
    }

    private int SearchIndexFromID(int c_id)
    {
        //아이디가 오름차순(작은 것 우선)으로 정렬되어 있다고 가정함. 다르다면 별도로 만들 것.
        int head = 0;
        int tail = cardIDs.Length - 1;
        int center = Mathf.FloorToInt((head + tail / 2));

        while (cardIDs[center] != c_id)
        {
            if (head > tail) return -1;

            if(cardIDs[center] < c_id)
            {
                head = center + 1;
                center = Mathf.FloorToInt((head + tail) / 2);
            }
            else
            {
                tail = center - 1;
                center = Mathf.FloorToInt((head + tail) / 2);
            }
        }
        return center;
    }

    public void AddCard_Object(int c_id, Transform _p, ref List<GameObject> _out)
    {
        int index = SearchIndexFromID(c_id);

        if (index == -1)
        {
            Debug.LogError("추가할 카드의 아이디와 일치하는 카드를 발견하지 못함");
            return;
        }

        if (card_HadCnt[index] < max_DuplicateValue)
        {
            card_HadCnt[index] += 1;
            SaveHadCnt();
            GameObject go = Instantiate(cards[index].card_prefab, _p);
            go.SetActive(false);
            _out.Add(go);
        }
        else
        {
            Debug.Log($"소지 카드수가 이미 {max_DuplicateValue}을 넘었습니다.");
        }
    }

    public void InstantiateCards(Transform _p, ref List<GameObject> _out)
    {
        for(int i = 0; i < cards.Length; i++)
        {
            int _i = i;
            if(card_HadCnt[_i] > 0)
            {
                for(int j = 0; j < card_HadCnt[_i]; j++)
                {
                    GameObject go = Instantiate(cards[_i].card_prefab, _p);
                    go.SetActive(false);
                    _out.Add(go);
                }
            }
        }
    }

public GameObject GetRandomCard_isntConfirm()
    {
        int fullWeight = 0;
        int max = -1;
        int rand;
        for (int i = 0; i < canList.Count; i++)
        {
            int _i = i;
            fullWeight += canList[_i].card_weight;
        }
        rand = UnityEngine.Random.Range(0, fullWeight);

        for (int i = 0; i < m_instance.canList.Count; i++)
        {
            int _i = i;
            max += m_instance.canList[_i].card_weight;
            if(rand >= max - canList[_i].card_weight && rand < max)
            {
                int id = m_instance.canList[_i].card_prefab.GetComponent<ICard>().GetID();
                int index = m_instance.SearchIndexFromID(id);
                GameObject temp;
                TempHadCntUpDown_Index(index, true);
                if (m_instance.card_HadCnt[_i] + m_instance.tempHadCnt[_i] >= max_DuplicateValue)
                {
                    temp = canList[_i].card_prefab;
                    canList.RemoveAt(_i);
                    return temp;
                }
                return canList[_i].card_prefab;
            }
        }
        Debug.LogError("랜덤 카드 값을 가져오는데 실패했습니다.");
        return null;
    }

    public void ResetCanList()
    {
        canList.Clear();
        for(int i = 0; i < card_HadCnt.Length; i++)
        {
            int _i = i;
            if(card_HadCnt[_i] + tempHadCnt[_i] < max_DuplicateValue)
            {
                canList.Add(m_instance.cards[_i]);
            }
        }
    }

    public void TempHadCntUpDown(int _id, bool isUp)
    {
        int index = SearchIndexFromID(_id);
        if (isUp)
            tempHadCnt[index] += 1;
        else
            tempHadCnt[index] -= 1;
    }

    public void TempHadCntReset()
    {
        for(int i = 0; i < tempHadCnt.Length; i++)
        {
            int _i = i;
            tempHadCnt[_i] = 0;
        }
    }

    private void TempHadCntUpDown_Index(int _index, bool isUp)
    {
        if (isUp)
            tempHadCnt[_index] += 1;
        else
            tempHadCnt[_index] -= 1;
    }

    public void SetSaveID(int id)
    {
        saveID = id;
    }

    public void SaveHadCnt()
    {
        key.Clear();
        for(int i = 0; i < card_HadCnt.Length; i++)
        {
            int _i = i;
            if (_i == 0)
                key.Append($"SaveID({saveID}).CardID({cardIDs[_i]}).HadCnt");
            else
                key.Replace($"CardID({cardIDs[_i - 1]})", $"CardID({cardIDs[_i]})");
            PlayerPrefs.SetInt(key.ToString(), card_HadCnt[_i]);
        }
    }

    public int GetCardsLength()
    {
        return cards.Length;
    }

    public int GetHadCardsNum()
    {
        int result = 0;
        for (int i = 0; i < card_HadCnt.Length; i++)
        {
            int _i = i;
            result += card_HadCnt[_i];
        }
        return result;
    }
    public int GetCardHadCnt_ID(int c_id, bool _sumTempCnt)
    {
        int index = SearchIndexFromID(c_id);
        int result = card_HadCnt[index];
        if (_sumTempCnt)
            result += tempHadCnt[index];
        return result;
    }

    public int GetMaxDuplicateNum()
    {
        return max_DuplicateValue;
    }
}
