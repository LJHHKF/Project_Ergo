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
    public event EventHandler firstTurn;
    public event EventHandler turnStart;
    public event EventHandler playerTurnEnd;
    public event EventHandler turnEnd;
    public event EventHandler battleEnd;

    private bool isFirstActived = false;
    private float start_time = 0;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }

        GameMaster.instance.battleStageEnd += Event_BattleStageEnd;
        //GameObject.FindGameObjectWithTag("CDeck").GetComponent<DeckManager>().SetTurnManager(this);

        start_time = Time.time;
    }

    private void Event_BattleStageEnd(object sender, EventArgs e)
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
        GameMaster.instance.battleStageEnd -= Event_BattleStageEnd;
    }


    //private void Update()
    //{
    //    if(!isFirstActived)
    //    {
    //        if (Time.time - start_time > 1.0f)
    //        {
    //            isFirstActived = true;
    //            if (firstTurn != null)
    //            {
    //                firstTurn();
    //            }
    //        }
    //    }
    //}

    //public void OnDelayedFirstTurn()
    //{
    //    StartCoroutine(DelayedFirstTurn());
    //}

    public static void OnFirstTurn()
    {
        m_instance.firstTurn?.Invoke(m_instance, EventArgs.Empty);
        m_instance.isFirstActived = true;
    }

    public void OnTurnStart()
    {
        turnStart?.Invoke(m_instance, EventArgs.Empty);
    }

    public void OnPlayerTurnEnd() // UI Btn에 연결되어 있음. 참조 0이라도 상관x.
    {
         playerTurnEnd?.Invoke(m_instance, EventArgs.Empty);
    }

    public void OnTurnEnd()
    {
        turnEnd?.Invoke(m_instance, EventArgs.Empty);
        OnTurnStart();
    }

    public void OnBattleEnd()
    {
        battleEnd?.Invoke(m_instance, EventArgs.Empty);
    }

    public bool GetIsFirstActivated()
    {
        return isFirstActived;
    }
}
