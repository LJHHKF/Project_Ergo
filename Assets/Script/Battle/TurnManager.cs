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

    private bool isFirstActived = false;
    private float start_time = 0;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }

        battleEnd += () => isFirstActived = false;
        //GameObject.FindGameObjectWithTag("CDeck").GetComponent<DeckManager>().SetTurnManager(this);

        GameMaster.instance.OnBattleStageStart();

        start_time = Time.time;
    }

    private void OnDestroy()
    {
        m_instance = null;
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
        if (m_instance.firstTurn != null)
        {
            m_instance.firstTurn();
        }
        m_instance.isFirstActived = true;
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

    public void OnTurnEnd()
    {
        if (turnEnd != null)
        {
            turnEnd();
        }
        OnTurnStart();
    }

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

    //IEnumerator DelayedFirstTurn()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    OnFirstTurn();
    //    yield break;
    //}
}
