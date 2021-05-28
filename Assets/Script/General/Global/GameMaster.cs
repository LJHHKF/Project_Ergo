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

    public event Action startGame_Awake;
    public event Action startGame_Start;
    public event Action initSaveData_Awake;
    public event Action initSaveData_Start;
    public event Action gameOver;
    public event Action gameStop;
    public event Action stageStart;
    public event Action battleStageStart;
    public event Action stageEnd;
    public event Action battleStageEnd;

    public bool isInit { get; set; }
    private bool isBattleEndChk = false;

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

        isInit = false;

        if (isReset)
            PlayerPrefs.DeleteAll();
    }

    private void Event_GameOver()
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
            isInit = true;
            key = $"SaveID({saveID})";
            PlayerPrefs.SetInt(key, 1);
            initSaveData_Awake?.Invoke();
            initSaveData_Start?.Invoke();
        }
        else if (!OnInitSaveData())
        {
            startGame_Awake?.Invoke();
            startGame_Start?.Invoke();
        }
    }

    public bool OnInitSaveData()
    {
        key = $"SaveID({saveID})";
        if (!PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) == 0)
        {
            PlayerPrefs.SetInt(key, 1);
            initSaveData_Awake?.Invoke();
            initSaveData_Start?.Invoke();
            isInit = true;
            return true;
        }
        else
        {
            isInit = false;
            return false;
        }
    }

    public void OnGameOver()
    {
        isInit = false;
        Debug.LogWarning("게임오버가 되었습니다.");
        m_instance.StartCoroutine(DelayedGameOver());
    }

    public void OnGameStop()
    {
        if (!isDoGameStop)
        {
            gameStop?.Invoke();
            isDoGameStop = true;
            isInit = false;
        }
    }

    public void OnStageStart()
    {
        stageStart?.Invoke();
    }

    public void OnBattleStageStart()
    {
        battleStageStart?.Invoke();
        OnStageStart();

        isBattleEndChk = false;
    }

    public void OnStageEnd()
    {
        stageEnd?.Invoke();
    }

    public void OnBattleStageEnd()
    {
        if (!isBattleEndChk)
        {
            isBattleEndChk = false;
            battleStageEnd?.Invoke();
            OnStageEnd();
        }
    }


    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(end_DelayTime);
        gameOver?.Invoke();
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
