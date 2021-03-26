using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CardPack : MonoBehaviour
{
    [Header("Pack Settincg")]
    public int max_DuplicateValue = 3;
    private int saveID = 0;

    [Header("Card Registration")]
    public GameObject[] card_prefabs;
    public float[] card_weight;
    private int[] cardIDs;
    private int[] card_HadCnt;

    private void OnApplicationQuit()
    {
        SaveHadCnt();
    }

    public void CardPackInit()
    {
        cardIDs = new int[card_prefabs.Length];
        card_HadCnt = new int[card_prefabs.Length];

        for (int i = 0; i < card_prefabs.Length; i++)
        {
            cardIDs[i] = card_prefabs[i].GetComponent<ICard>().GetCardID();

            StringBuilder key = new StringBuilder();
            key.Append("saveID(");
            key.Append(saveID.ToString());
            key.Append(").cardID(");
            key.Append(cardIDs[i].ToString());
            key.Append(").hadCnt");

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

    //public int CardsArrayLength()
    //{
    //    return card_prefabs.Length;
    //}

    public void AddCard_OnlyHadData(int c_id)
    {
        int index = -1;
        for(int i = 0; i< cardIDs.Length;i++)
        {
            if(cardIDs[i] == c_id)
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

        if (card_HadCnt[index] < 3)
        {
            card_HadCnt[index] += 1;
        }
        else
        {
            Debug.Log("소지 카드수가 이미 3을 넘었습니다.");
        }
    }

    public void AddCard_Object(int c_id, Transform _p, ref List<GameObject> _out)
    {
        int index = -1;
        for (int i = 0; i < cardIDs.Length; i++)
        {
            if (cardIDs[i] == c_id)
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

        if (card_HadCnt[index] < 3)
        {
            card_HadCnt[index] += 1;
            GameObject go = Instantiate(card_prefabs[index], _p);
            _out.Add(go);
        }
        else
        {
            Debug.Log("소지 카드수가 이미 3을 넘었습니다.");
        }
    }

    public void InstantiateCards(Transform _p, ref List<GameObject> _out)
    {
        for(int i = 0; i < card_prefabs.Length; i++)
        {
            if(card_HadCnt[i] > 0)
            {
                for(int j = 0; j < card_HadCnt[i]; j++)
                {
                    GameObject go = Instantiate(card_prefabs[i], _p);
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
        for(int i = 0; i < card_HadCnt.Length; i++)
        {
            StringBuilder key = new StringBuilder();
            key.Append("saveID(");
            key.Append(saveID.ToString());
            key.Append(").cardID(");
            key.Append(cardIDs[i].ToString());
            key.Append(").hadCnt");
            PlayerPrefs.SetInt(key.ToString(), card_HadCnt[i]);
        }
    }

    public void DeleteSavedCardData(int s_ID)
    {
        for (int i = 0; i < card_HadCnt.Length; i++)
        {
            StringBuilder key = new StringBuilder();
            key.Append("saveID(");
            key.Append(saveID.ToString());
            key.Append(").cardID(");
            key.Append(cardIDs[i].ToString());
            key.Append(").hadCnt");
            PlayerPrefs.DeleteKey(key.ToString());
        }

        //이하 코드는 세이브 매니저 만들면 옮길 것
        StringBuilder Key2 = new StringBuilder();
        Key2.Append("SaveID(");
        Key2.Append(saveID.ToString());
        Key2.Append(")");
        PlayerPrefs.SetInt(Key2.ToString(), 0); //false
    }
}
