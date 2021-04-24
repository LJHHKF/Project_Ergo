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
    [SerializeField] private float end_DelayTime = 2.0f;
    [SerializeField] private bool isReset = false;
    private bool isDoGameStop = false;

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
        PlayerPrefs.DeleteKey(key);
    }

    private void OnApplicationQuit()
    {
        OnGameStop();
    }

    public int GetSaveID()
    {
        return saveID;
    }

    //public void GameStart(int _saveID)
    //{
    //    saveID = _saveID;
    //    if (!OnInitSaveData())
    //    {
    //        startGame_Awake?.Invoke(this, EventArgs.Empty);
    //        startGame_Start?.Invoke(this, EventArgs.Empty);
    //    }
    //}

    public void GameStart(int _saveID, bool _isNew)
    {
        saveID = _saveID;
        isDoGameStop = false;

        if (_isNew)
        {
            key = $"SaveID({saveID})";
            PlayerPrefs.SetInt(key, 1);
            initSaveData_Awake?.Invoke(this, EventArgs.Empty);
            initSaveData_Start?.Invoke(this, EventArgs.Empty);
        }
        else if (!OnInitSaveData())
        {
            startGame_Awake?.Invoke(this, EventArgs.Empty);
            startGame_Start?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool OnInitSaveData()
    {
        key = $"SaveID({saveID})";

        if (!PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) == 0)
        {
            PlayerPrefs.SetInt(key, 1);
            initSaveData_Awake?.Invoke(this, EventArgs.Empty);
            initSaveData_Start?.Invoke(this, EventArgs.Empty);
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
        if (!isDoGameStop)
        {
            gameStop?.Invoke(this, EventArgs.Empty);
            isDoGameStop = true;
        }
    }

    public void OnBattleStageStart()
    {
        battleStageStart?.Invoke(this, EventArgs.Empty);
    }

    public void OnStageEnd()
    {
        stageEnd?.Invoke(this, EventArgs.Empty);
    }

    public void OnBattleStageEnd()
    {
        battleStageEnd?.Invoke(this, EventArgs.Empty);
        OnStageEnd();
    }


    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(end_DelayTime);
        gameOver?.Invoke(this, EventArgs.Empty);
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
