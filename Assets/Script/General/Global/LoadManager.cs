using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using myBGM;

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
    private int nextStageTypeIndex = -1;

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

    public void LoadNextStage()
    {
        ChkAndOnStageEndEvent();
        switch (nextStageTypeIndex)
        {
            case -1:
                ReturnLobby_private();
                GameMaster.instance.OnGameOver();
                break;
            case 0:
                m_instance.isBattleReady = true;
                StageManager.instance.IncreaseCurrentStageNum();
                LoadingSceneManager.LoadScene("Battle");
                BGMManager.instance.BGMChange(BGMList.battle);
                break;
            case 1:
                StageManager.instance.IncreaseCurrentStageNum();
                LoadingSceneManager.LoadScene("Ev_Shop");
                BGMManager.instance.BGMChange(BGMList.shop);
                break;
            case 2:
                StageManager.instance.IncreaseCurrentStageNum();
                LoadingSceneManager.LoadScene("Ev_Rest");
                BGMManager.instance.BGMChange(BGMList.rest);
                break;
            case 3:
                StageManager.instance.IncreaseCurrentStageNum();
                LoadingSceneManager.LoadScene("Ev_Trap");
                BGMManager.instance.BGMChange(BGMList.battle);
                break;
        }
        GameMaster.instance.isInit = false;
        StageManager.instance.SetCurrentStageTypeIndex(nextStageTypeIndex);
        StageManager.instance.SetNextStage();
    }

    public void LoadFirst(int _stageTypeIndex)
    {
        switch(_stageTypeIndex)
        {
            case 0:
                m_instance.isBattleReady = true;
                StageManager.instance.SetCurrentStageTypeIndex(0);
                LoadingSceneManager.LoadScene("Battle");
                BGMManager.instance.BGMChange(BGMList.battle);
                break;
            case 1:
                LoadingSceneManager.LoadScene("Ev_Shop");
                StageManager.instance.SetCurrentStageTypeIndex(1);
                BGMManager.instance.BGMChange(BGMList.shop);
                break;
            case 2:
                LoadingSceneManager.LoadScene("Ev_Rest");
                StageManager.instance.SetCurrentStageTypeIndex(2);
                BGMManager.instance.BGMChange(BGMList.rest);
                break;
            case 3:
                LoadingSceneManager.LoadScene("Ev_Trap");
                StageManager.instance.SetCurrentStageTypeIndex(3);
                BGMManager.instance.BGMChange(BGMList.battle);
                break;
        }
        StageManager.instance.SetNextStage();
    }

    public void ChangeNextStage(int _index)
    {
        nextStageTypeIndex = _index;
    }

    public void ChangeNextStage_InitStart()
    {
        LoadFirst(nextStageTypeIndex);
    }

    public void LoadFirst_Init_ToSetting()
    {
        LoadingSceneManager.LoadScene("AutoStatusScene");
        BGMManager.instance.BGMChange(BGMList.story);
    }

    public void LoadFirst_Init()
    {
        m_instance.isBattleReady = true;
        StageManager.instance.SetCurrentStageTypeIndex(0);
        LoadingSceneManager.LoadScene("Battle");
        BGMManager.instance.BGMChange(BGMList.battle);
        StageManager.instance.SetNextStage();
    }

    public void LoadStoryScene()
    {
        LoadingSceneManager.LoadScene("StoryScene");
        BGMManager.instance.BGMChange(BGMList.story);
    }

    public void LoadGameOver()
    {
        ChkAndOnStageEndEvent();
        //GameMaster.instance.OnGameOver();
        //StartCoroutine(DelayedLoadScene("GameOver", 2.0f));
        nextStageTypeIndex = -1;
        LoadStoryScene();
    }

    public void ReturnLobby()
    {
        ChkAndOnStageEndEvent();

        GameMaster.instance.OnGameStop();
        ReturnLobby_private();
    }

    public void ReturnLobby_GameOver()
    {
        ReturnLobby_private();
    }

    private void ReturnLobby_private()
    {
        LoadingSceneManager.LoadScene("Entrance");
        BGMManager.instance.BGMChange(BGMList.entrance);
    }

    private void ChkAndOnStageEndEvent()
    {
        if (SceneManager.GetActiveScene().name == "Battle")
            GameMaster.instance.OnBattleStageEnd();
        else
            GameMaster.instance.OnStageEnd();
    }

    public void ChkAndPlayDelayOn()
    {
        if(m_instance.isBattleReady)
        {
            m_instance.isBattleReady = false;
            m_instance.StartCoroutine(m_instance.OnDelayedTurnStart());
        }
    }

    public void SetNextStageTypeIndex(int _index)
    {
        nextStageTypeIndex = _index;
    }

    IEnumerator OnDelayedTurnStart()
    {
        yield return new WaitForSeconds(0.5f);
        TurnManager.instance.OnFirstTurn();
        yield break;
    }

    IEnumerator DelayedLoadScene(string _sceneName, float _time)
    {
        yield return new WaitForSeconds(_time);
        LoadingSceneManager.LoadScene(_sceneName);
        yield break;
    }
}
