using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public TurnManager instance
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
    public static event Action firstTurn;
    public static event Action turnStart;
    public static event Action playerTurnEnd;
    public static event Action turnEnd;
    public static event Action battleEnd;

    private bool isFirstActived = false;
    private float start_time = 0;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }

        battleEnd += () => isFirstActived = false;
        battleEnd += () => GameMaster.OnStageEnd();
        //GameObject.FindGameObjectWithTag("CDeck").GetComponent<DeckManager>().SetTurnManager(this);

        start_time = Time.time;
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
        if (firstTurn != null)
        {
            firstTurn();
        }
        m_instance.isFirstActived = true;
    }

    public static void OnTurnStart()
    {
        if (turnStart != null)
        {
            turnStart();
        }
    }

    public static void OnPlayerTurnEnd() // UI Btn에 연결되어 있음. 참조 0이라도 상관x.
    {
        if (playerTurnEnd != null)
        {
            playerTurnEnd();
        }
    }

    public static void OnTurnEnd()
    {
        if (turnEnd != null)
        {
            turnEnd();
        }
        OnTurnStart();
    }

    public static void OnBattleEnd()
    {
        if (battleEnd != null)
        {
            battleEnd();
        }
    }

    public static bool GetIsFirstActivated()
    {
        return m_instance.isFirstActived;
    }

    //IEnumerator DelayedFirstTurn()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    OnFirstTurn();
    //    yield break;
    //}
}
