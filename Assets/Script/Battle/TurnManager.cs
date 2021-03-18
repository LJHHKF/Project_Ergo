﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    // On 함수들의 참조 개수에 주의. 참조가 1개가 아니면 문제.
    public event Action firstTurn;  
    public event Action turnStart;
    public event Action playerTurnEnd;
    //public event Action turnEnd;
    public event Action battleEnd;

    private bool isFirstActived = false;
    private DeckManager m_deckM;

    private void Awake()
    {
        battleEnd += () => isFirstActived = true;
    }

    private void Start()
    {
        GameObject.FindGameObjectWithTag("CDeck").GetComponent<DeckManager>().SetTurnManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isFirstActived)
        {
            if (firstTurn != null)
            {
                firstTurn();
            }
            isFirstActived = true;
        }
    }

    public void OnTurnStart()
    {
        if (turnStart != null)
        {
            turnStart();
        }
    }

    public void OnPlayerTurnEnd() // UI Btn에 연결되어 있음. 참조 0이라도 상관x.
    {
        if (playerTurnEnd != null)
        {
            playerTurnEnd();
        }
    }

    //public void OnTurnEnd()
    //{
    //    turnEnd();
    //}

    public void OnBattleEnd()
    {
        if (battleEnd != null)
        {
            battleEnd();
        }
    }

    public bool GetIsFirstActivated()
    {
        return isFirstActived;
    }
}