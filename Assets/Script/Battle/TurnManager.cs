using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

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
    public UnityEvent firstTurn;
    public UnityEvent turnStart;
    public UnityEvent playerTurnEnd;
    public UnityEvent turnEnd;
    public UnityEvent battleEnd;

    private bool isBattleEnded = false;

    private bool isFirstActived = false;
    private float start_time = 0;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }

        GameMaster.instance.battleStageEnd.AddListener(Event_BattleStageEnd);

        start_time = Time.time;
    }

    private void Event_BattleStageEnd()
    {
        isFirstActived = false;

        //foreach(Delegate d in firstTurn.GetInvocationList())
        //    firstTurn -= (EventHandler)d;
        //foreach (Delegate d in turnStart.GetInvocationList())
        //    turnStart -= (EventHandler)d;
        //foreach (Delegate d in playerTurnEnd.GetInvocationList())
        //    playerTurnEnd -= (EventHandler)d;
        //foreach (Delegate d in turnEnd.GetInvocationList())
        //    turnEnd -= (EventHandler)d;
        //foreach (Delegate d in battleEnd.GetInvocationList())
        //    battleEnd -= (EventHandler)d;
    }

    private void OnDestroy()
    {
        m_instance = null;
        GameMaster.instance.battleStageEnd.RemoveListener(Event_BattleStageEnd);
    }

    public static void OnFirstTurn()
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
