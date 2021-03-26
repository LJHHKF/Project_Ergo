using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class InitCardSetting : MonoBehaviour
{
    [Header("Temp Set Value")]
    public int saveID = 0;
    
    [Header("Card Init Set Value")]
    public CardPack m_cardPack;
    public int[] cardIDs;
    public int[] cardHadValue;

    private void Awake()
    {
        //추후엔 이 부분을 통째로 함수화한 후 세이브 매니저에서 불러올 것.
        m_cardPack.SetSaveID(saveID);
        m_cardPack.CardPackInit();

        StringBuilder Key2 = new StringBuilder();
        Key2.Append("SaveID(");
        Key2.Append(saveID.ToString());
        Key2.Append(")");

        if (PlayerPrefs.GetInt(Key2.ToString()) == 0 || PlayerPrefs.HasKey(Key2.ToString()) == false)
        {
            PlayerPrefs.SetInt(Key2.ToString(), 1);
            OnInitCardsSetting();
        }
    }

    private void OnInitCardsSetting()
    {
        for(int i = 0; i < cardHadValue.Length; i++)
        {
            if (cardHadValue[i] > 0)
            {
                for (int j = 0; j < cardHadValue[i]; j++)
                {
                    m_cardPack.AddCard_OnlyHadData(cardIDs[i]);
                }
            }
        }
    }
}
