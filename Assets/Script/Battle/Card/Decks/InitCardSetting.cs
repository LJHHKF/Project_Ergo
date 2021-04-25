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
    [SerializeField] private CardPack m_cardPack;
    [SerializeField] private CardInitInfo[] c_initInfos;


    private void Start()
    {
        GameMaster.instance.initSaveData_Awake += Event_InitSaveDataAwake;
        GameMaster.instance.startGame_Awake += Event_StartGameAwake;
    }

    private void OnDestroy()
    {
        GameMaster.instance.initSaveData_Awake -= Event_InitSaveDataAwake;
        GameMaster.instance.startGame_Awake -= Event_StartGameAwake;
    }

    private void Event_InitSaveDataAwake(object _o, EventArgs _e)
    {
        OnInitCardsSetting();
    }

    private void Event_StartGameAwake(object _o, EventArgs _e)
    {
        m_cardPack.SetSaveID(GameMaster.instance.GetSaveID());
        m_cardPack.CardPack_Start();
    }

    public void OnInitCardsSetting()
    {
        m_cardPack.SetSaveID(GameMaster.instance.GetSaveID());
        m_cardPack.CardPack_Init();
        for (int i = 0; i < c_initInfos.Length; i++)
        {
            int _i = i;
            if (c_initInfos[_i].cardHadValue > 0)
            {
                for (int j = 0; j < c_initInfos[i].cardHadValue; j++)
                {
                    CardPack.instance.AddCard_OnlyHadData(c_initInfos[_i].cardID);
                }
            }
        }
    }
}
