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

    public event EventHandler startGame_Awake;
    public event EventHandler startGame_Start;
    public event EventHandler initSaveData_Awake;
    public event EventHandler initSaveData_Start;
    public event EventHandler gameOver;
    public event EventHandler gameStop;
    public event EventHandler battleStageStart;
    public event EventHandler stageEnd;
    public event EventHandler battleStageEnd;

    protected void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOver += Event_GameOver;

        if (isReset)
            PlayerPrefs.DeleteAll();
    }

    private void Event_GameOver(object _o, EventArgs _e)
    {
        key = $"SaveID({saveID})";
        PlayerPrefs.SetInt(key, 0);
    }

    private void OnApplicationQuit()
    {
        OnGameStop();
    }

    public int GetSaveID()
    {
        return saveID;
    }

    public void GameStart(int _saveID)
    {
        saveID = _saveID;
        if (!OnInitSaveData())
        {
            if (startGame_Awake != null)
            {
                startGame_Awake.Invoke(this, EventArgs.Empty);
            }
            //StartCoroutine(DelayedEvent(startGame_Start));
            if(startGame_Start != null)
            {
                startGame_Start.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool OnInitSaveData()
    {
        key = $"SaveID({saveID})";

        if (PlayerPrefs.HasKey(key) == false || PlayerPrefs.GetInt(key) == 0)
        {
            PlayerPrefs.SetInt(key, 1);
            if (initSaveData_Awake != null)
            {
                initSaveData_Awake.Invoke(this, EventArgs.Empty);
            }
            //StartCoroutine(DelayedEvent(initSaveData_Start));
            if(initSaveData_Start != null)
            {
                initSaveData_Start.Invoke(this, EventArgs.Empty);
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
        m_instance.StartCoroutine(DelayedGameOver());
    }

    public void OnGameStop()
    {
        if(gameStop != null)
        {
            gameStop.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnBattleStageStart()
    {
        if(battleStageStart != null)
        {
            battleStageStart.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnStageEnd()
    {
        if(stageEnd != null)
        {
            stageEnd.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnBattleStageEnd()
    {
        if(battleStageEnd != null)
        {
            battleStageEnd.Invoke(this, EventArgs.Empty);
        }
        OnStageEnd();
    }


    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(end_DelayTime);
        if (gameOver != null)
        {
            gameOver.Invoke(this, EventArgs.Empty);
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
