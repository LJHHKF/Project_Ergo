using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class StageManager : MonoBehaviour
{
    public static StageManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<StageManager>();
            return m_instance;
        }
    }

    private static StageManager m_instance;

    [Serializable]struct EventStageInfo
    {
        public string name;
        public string sceneName;
        public int weight;
    }

    [Serializable]struct StageWeight
    {
        public int battleWeight;
        public int evWeight;
    }

    [SerializeField] private EventStageInfo[] evStages;
    [SerializeField] private StageWeight[] chapter1Weight;

    private int m_curChapter = 1;
    //public int curChapter { get { return m_curChapter; } }
    private int m_curStage;
    private int curStageTypeIndex;
    public int curStage { get { return m_curStage+1; } }
    private int nextStageTypeIndex;
    private StringBuilder key = new StringBuilder();

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameMaster.instance.initSaveData_Awake += Event_InitGameData;
        GameMaster.instance.startGame_Awake += Event_GameStart;
        GameMaster.instance.gameStop += Event_GameStop;
        GameMaster.instance.gameOver += Event_GameOver;
    }

    private void OnDestroy()
    {
        GameMaster.instance.initSaveData_Awake -= Event_InitGameData;
        GameMaster.instance.startGame_Awake -= Event_GameStart;
        GameMaster.instance.gameStop -= Event_GameStop;
        GameMaster.instance.gameOver -= Event_GameOver;
    }

    private void Event_InitGameData()
    {
        m_curChapter = 1;
        m_curStage = 0;
        curStageTypeIndex = 0;

        key.Clear();
        key.Append($"SaveID({GameMaster.instance.GetSaveID()}).CurrentStage");
        PlayerPrefs.SetInt(key.ToString(), m_curStage);

        key.Replace("Stage", "Chapter");
        PlayerPrefs.SetInt(key.ToString(), m_curChapter);

        key.Replace("Chapter", "StageTypeIndex");
        PlayerPrefs.SetInt(key.ToString(), curStageTypeIndex);

        LoadManager.instance.LoadFirst_Init_ToSetting();
    }

    private void Event_GameStart()
    {
        key.Clear();
        key.Append($"SaveID({GameMaster.instance.GetSaveID()}).CurrentStage");
        m_curStage = PlayerPrefs.GetInt(key.ToString());

        key.Replace("Stage", "StageTypeIndex");
        curStageTypeIndex = PlayerPrefs.GetInt(key.ToString());

        key.Replace("StageTypeIndex", "Chapter");
        m_curChapter = PlayerPrefs.GetInt(key.ToString());

        LoadManager.instance.LoadFirst(curStageTypeIndex);
    }

    private void Event_GameStop()
    {
        key.Clear();
        key.Append($"SaveID({GameMaster.instance.GetSaveID()}).CurrentStage");

        PlayerPrefs.SetInt(key.ToString(), m_curStage);

        key.Replace("Stage", "Chapter");
        PlayerPrefs.SetInt(key.ToString(), m_curChapter);

        key.Replace("Chapter", "StageTypeIndex");
        PlayerPrefs.SetInt(key.ToString(), curStageTypeIndex);
    }

    private void Event_GameOver()
    {
        key.Clear();
        key.Append($"SaveID({GameMaster.instance.GetSaveID()}).CurrentStage");

        PlayerPrefs.DeleteKey(key.ToString());

        key.Replace("Stage", "Chapter");
        PlayerPrefs.DeleteKey(key.ToString());

        key.Replace("Chapter", "StageTypeIndex");
        PlayerPrefs.DeleteKey(key.ToString());
    }

    public void IncreaseCurrentStageNum()
    {
        m_curStage++;
    }

    public int GetCurStageMax()
    {
        int result = 0;
        switch(m_curChapter)
        {
            case 1:
                result = chapter1Weight.Length;
                break;
        }
        return result;
    }

    public void SetNextStage()
    {
        nextStageTypeIndex = -1;
        if(m_curChapter == 1)
        {
            if (m_curStage < chapter1Weight.Length)
            {
                int fullWeight = chapter1Weight[m_curStage+1].battleWeight + chapter1Weight[m_curStage+1].evWeight; //
                int rand = UnityEngine.Random.Range(0, fullWeight - 1);

                if (rand < chapter1Weight[m_curStage].battleWeight)
                    nextStageTypeIndex = 0;
                else
                {
                    fullWeight = 0;
                    for (int i = 0; i < evStages.Length; i++)
                        fullWeight += evStages[i].weight;
                    rand = UnityEngine.Random.Range(0, fullWeight - 1);
                    fullWeight = 0;
                    for (int i = 0; i < evStages.Length; i++)
                    {
                        fullWeight += evStages[i].weight;
                        if (rand >= fullWeight - evStages[i].weight && rand < fullWeight)
                        {
                            nextStageTypeIndex = i + 1;
                            break;
                        }
                    }
                }
            }
            else
                nextStageTypeIndex = -1;
        }

        LoadManager.instance.SetNextStageTypeIndex(nextStageTypeIndex);
    }

    public void SetCurrentStageTypeIndex(int _input)
    {
        curStageTypeIndex = _input;
    }

    public int GetCurrentStageTypeIndex()
    {
        return curStageTypeIndex;
    }
}
