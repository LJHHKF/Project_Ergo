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
    [SerializeField] private CardInitInfo[] c_initInfos;


    private void Start()
    {
        GameMaster.instance.initSaveData_Awake += OnInitCardsSetting;
        GameMaster.instance.startGame_Awake += Event_StartGameAwake;
    }

    private void OnDestroy()
    {
        GameMaster.instance.initSaveData_Awake -= OnInitCardsSetting;
        GameMaster.instance.startGame_Awake -= Event_StartGameAwake;
    }

    //private void Event_InitSaveDataAwake()
    //{
    //    OnInitCardsSetting();
    //}

    private void Event_StartGameAwake()
    {
        CardPack.instance.SetSaveID(GameMaster.instance.GetSaveID());
        CardPack.instance.CardPack_Start();
    }

    public void OnInitCardsSetting()
    {
        CardPack.instance.SetSaveID(GameMaster.instance.GetSaveID());
        CardPack.instance.CardPack_Init();
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
