using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitCardSetting : MonoBehaviour
{
    [Serializable]
    struct CardInitInfo
    {
        public int cardID;
        public int cardHadValue;
    }

    [Header("Card Init Set Value")]
    [SerializeField]private CardPack m_cardPack;
    [SerializeField] private CardInitInfo[] c_initInfos;


    private void Start()
    {
        GameMaster.instance.initSaveData_Awake += () => OnInitCardsSetting(GameMaster.instance.GetSaveID());
        GameMaster.instance.startGame_Awake += () => CardPackReset(GameMaster.instance.GetSaveID());
    }

    public void OnInitCardsSetting(int saveID)
    {
        CardPackReset(saveID);
        for (int i = 0; i < c_initInfos.Length; i++)
        {
            if (c_initInfos[i].cardHadValue > 0)
            {
                for (int j = 0; j < c_initInfos[i].cardHadValue; j++)
                {
                    CardPack.instance.AddCard_OnlyHadData(c_initInfos[i].cardID);
                }
            }
        }
    }

    public void CardPackReset(int saveID)
    {
        m_cardPack.SetSaveID(saveID);
        m_cardPack.CardPackInit();
    }
}
