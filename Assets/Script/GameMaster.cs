using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GameMaster>();
            return m_instance;
        }
    }
    private static GameMaster m_instance;

    private int saveID = 0; //멀티 세이브 용도보단 ...
    private string key;
    private int cur_stage = 0;
    [SerializeField] private float end_DelayTime = 2.0f;
    [SerializeField] private bool isReset = false;

    public event Action startGame_Awake;
    public event Action startGame_Start;
    public event Action initSaveData_Awake;
    public event Action initSaveData_Start;
    public event Action gameOver;
    public event Action gameStop;
    public event Action battleStageStart;
    public event Action stageEnd;

    protected void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        key = $"SaveID({saveID})";

        gameOver += () => PlayerPrefs.SetInt(key, 0);

        if (isReset)
            PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit()
    {
        OnGameStop();
    }

    public int GetSaveID()
    {
        return m_instance.saveID;
    }

    public void GameStart(int _saveID)
    {
        m_instance.saveID = _saveID;
        if (!m_instance.OnInitSaveData())
        {
            if (startGame_Awake != null)
            {
                startGame_Awake();
            }
            //StartCoroutine(DelayedEvent(startGame_Start));
            if(startGame_Start != null)
            {
                startGame_Start();
            }
        }
    }

    public bool OnInitSaveData()
    {
        key = $"SaveID({saveID})";

        if (PlayerPrefs.GetInt(key) == 0 || PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetInt(key, 1);
            if (initSaveData_Awake != null)
            {
                initSaveData_Awake();
            }
            //StartCoroutine(DelayedEvent(initSaveData_Start));
            if(initSaveData_Start != null)
            {
                initSaveData_Start();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnGameOver()
    {
        Debug.LogWarning("게임오버가 되었습니다.");
        m_instance.StartCoroutine(m_instance.DelayedGameOver());
    }

    public void OnGameStop()
    {
        if(gameStop != null)
        {
            gameStop();
        }
    }

    public void OnBattleStageStart()
    {
        if(battleStageStart != null)
        {
            battleStageStart();
        }
    }

    public void OnStageEnd()
    {
        if(stageEnd != null)
        {
            stageEnd();
        }
    }


    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(end_DelayTime);
        if (gameOver != null)
        {
            gameOver();
        }
        yield break;
    }

    //IEnumerator DelayedEvent(Action _ev)
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    if (_ev != null)
    //    {
    //        _ev();
    //    }
    //    yield break;
    //}
}
