﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<LoadManager>();
            return m_instance;
        }
    }
    private static LoadManager m_instance;

    //[SerializeField] private int saveID = 0;
    private bool isBattleReady = false;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    public void GameStart(int ID)
    {
        GameMaster.instance.GameStart(ID);
        isBattleReady = true;
        LoadingSceneManager.LoadScene("Battle");
    }

    public void LoadNextStage()
    {
        //GameMaster.instance.OnStageEnd();
        GameMaster.instance.OnBattleStageEnd();
        m_instance.isBattleReady = true; // 이 부분은 추후 '전투씬'으로 들어갈 때만으로 한정할 필요가 있음
        LoadingSceneManager.LoadScene("Battle");
    }

    public void ChkAndPlayDelayOn()
    {
        if(m_instance.isBattleReady)
        {
            m_instance.isBattleReady = false;
            m_instance.StartCoroutine(m_instance.OnDelayedTurnStart());
        }
    }

    IEnumerator OnDelayedTurnStart()
    {
        yield return new WaitForSeconds(2.0f);
        TurnManager.OnFirstTurn();
        yield break;

    }
}