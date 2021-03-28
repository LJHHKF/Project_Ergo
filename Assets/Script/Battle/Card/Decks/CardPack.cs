using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class CardPack : MonoBehaviour
{
    public CardPack instance
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

    private StringBuilder key = new StringBuilder();

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        GameMaster.gameOver += () => CardPackClear();
        GameMaster.gameStop += () => SaveHadCnt();
    }

    public void CardPackInit()
    {
        cardIDs = new int[cards.Length];
        card_HadCnt = new int[cards.Length];

        key.Clear();
        for (int i = 0; i < cards.Length; i++)
        {
            cardIDs[i] = cards[i].card_prefab.GetComponent<ICard>().GetCardID();

            if (i == 0)
                key.Append($"SaveID({saveID}).CardID({cardIDs[i]}).HadCnt");
            else
                key.Replace($"CardID({cardIDs[i - 1]})", $"CardID({cardIDs[i]}");

            if (PlayerPrefs.HasKey(key.ToString()) == false)
            {
                card_HadCnt[i] = 0;
                PlayerPrefs.SetInt(key.ToString(), card_HadCnt[i]);
            }
            else
            {
                card_HadCnt[i] = PlayerPrefs.GetInt(key.ToString());
            }
        }
    }

    private void CardPackClear()
    {
        key.Clear();
        for(int i = 0; i < cards.Length; i++)
        {
            if (i == 0)
                key.Append($"SaveID({saveID}).CardID({cardIDs[i]}).HadCnt");
            else
                key.Replace($"CardID({cardIDs[i - 1]})", $"CardID({cardIDs[i]})");

            card_HadCnt[i] = 0;
            PlayerPrefs.SetInt(key.ToString(), card_HadCnt[i]);
        }
    }

    //public int CardsArrayLength()
    //{
    //    return card_prefabs.Length;
    //}

    public static void AddCard_OnlyHadData(int c_id)
    {
        int index = -1;
        for(int i = 0; i< m_instance.cardIDs.Length;i++)
        {
            if(m_instance.cardIDs[i] == c_id)
            {
                index = c_id;
                break;
            }
        }

        if(index == -1)
        {
            Debug.LogError("추가할 카드의 아이디와 일치하는 카드를 발견하지 못함");
            return;
        }

        if (m_instance.card_HadCnt[index] < 3)
        {
            m_instance.card_HadCnt[index] += 1;
        }
        else
        {
            Debug.Log("소지 카드수가 이미 3을 넘었습니다.");
        }
    }

    public static void AddCard_Object(int c_id, Transform _p, ref List<GameObject> _out)
    {
        int index = -1;
        for (int i = 0; i < m_instance.cardIDs.Length; i++)
        {
            if (m_instance.cardIDs[i] == c_id)
            {
                index = c_id;
                break;
            }
        }

        if (index == -1)
        {
            Debug.LogError("추가할 카드의 아이디와 일치하는 카드를 발견하지 못함");
            return;
        }

        if (m_instance.card_HadCnt[index] < 3)
        {
            m_instance.card_HadCnt[index] += 1;
            GameObject go = Instantiate(m_instance.cards[index].card_prefab, _p);
            go.SetActive(false);
            _out.Add(go);
        }
        else
        {
            Debug.Log("소지 카드수가 이미 3을 넘었습니다.");
        }
    }

    public void InstantiateCards(Transform _p, ref List<GameObject> _out)
    {
        for(int i = 0; i < cards.Length; i++)
        {
            if(card_HadCnt[i] > 0)
            {
                for(int j = 0; j < card_HadCnt[i]; j++)
                {
                    GameObject go = Instantiate(cards[i].card_prefab, _p);
                    go.SetActive(false);
                    _out.Add(go);
                }
            }
        }
    }

    public void SetSaveID(int id)
    {
        saveID = id;
    }

    private void SaveHadCnt()
    {
        key.Clear();
        for(int i = 0; i < card_HadCnt.Length; i++)
        {
            if (i == 0)
                key.Append($"SaveID({saveID}).CardID({cardIDs[i]}).HadCnt");
            else
                key.Replace($"CardID({cardIDs[i - 1]})", $"CardID({cardIDs[i]})");
            PlayerPrefs.SetInt(key.ToString(), card_HadCnt[i]);
        }
    }
}
