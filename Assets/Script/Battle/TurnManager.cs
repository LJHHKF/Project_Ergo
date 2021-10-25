using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TurnManager: MonoBehaviour
{
    public static TurnManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<TurnManager>();
            return m_instance;
        }
    }
    private static TurnManager m_instance;

    // On 함수들의 참조 개수에 주의. 참조가 1개가 아니면 문제.
    public event Action firstTurn;
    public event Action turnStart;
    public event Action playerTurnEnd;
    public event Action turnEnd;
    public event Action battleEnd;

    private bool isBattleEnded = false;

    private bool isFirstActived = false;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }

        GameMaster.instance.battleStageEnd += Event_BattleStageEnd;
    }

    private void Event_BattleStageEnd()
    {
        isFirstActived = false;

        //foreach(Delegate d in firstTurn.GetInvocationList())
        //    firstTurn -= (Action)d;
        //foreach (Delegate d in turnStart.GetInvocationList())
        //    turnStart -= (Action)d;
        //foreach (Delegate d in playerTurnEnd.GetInvocationList())
        //    playerTurnEnd -= (Action)d;
        //foreach (Delegate d in turnEnd.GetInvocationList())
        //    turnEnd -= (Action)d;
        //foreach (Delegate d in battleEnd.GetInvocationList())
        //    battleEnd -= (Action)d;
    }

    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
        GameMaster.instance.battleStageEnd -= Event_BattleStageEnd;
    }

    public void OnFirstTurn()
    {
        m_instance.firstTurn?.Invoke();
        m_instance.isFirstActived = true;
        m_instance.isBattleEnded = false;
    }

    public void OnTurnStart()
    {
        turnStart?.Invoke();
    }

    public void OnPlayerTurnEnd() // UI Btn에 연결되어 있음. 참조 0이라도 상관x.
    {
        playerTurnEnd?.Invoke();
    }

    public void OnTurnEnd()
    {
        turnEnd?.Invoke();
        OnTurnStart();
    }

    public void OnBattleEnd()
    {
        battleEnd?.Invoke();
        m_instance.isBattleEnded = true;
    }

    public bool GetIsFirstActivated()
    {
        return isFirstActived;
    }

    public bool GetIsBattleEnded()
    {
        return isBattleEnded;
    }
}
