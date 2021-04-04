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

    private int m_curChapter;
    //public int curChapter { get { return m_curChapter; } }
    private int m_curStage;
    public int curStage { get { return m_curStage; } }
    private int nextStageTypeIndex;
    private StringBuilder key;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameMaster.instance.initSaveData_Start += Event_InitGameData;
        GameMaster.instance.startGame_Start += Event_GameStart;
    }

    private void OnDestroy()
    {
        GameMaster.instance.initSaveData_Start -= Event_InitGameData;
        GameMaster.instance.startGame_Start -= Event_GameStart;
    }

    private void Event_InitGameData(object _sender, EventArgs _e)
    {
        m_curChapter = 1;
        m_curStage = 0;

        LoadManager.instance.SetNextStageTypeIndex(0);
        LoadManager.instance.LoadNextStage();

        key.Clear();
        key.Append($"SaveID({GameMaster.instance.GetSaveID()}).CurrentStage");

        PlayerPrefs.SetInt(key.ToString(), 1);

        key.Replace("NextStageIndex", "CurrentChapter");
        PlayerPrefs.SetInt(key.ToString(), m_curChapter);
    }

    private void Event_GameStart(object _sender, EventArgs _e)
    {
        key.Clear();
        key.Append($"SaveID({GameMaster.instance.GetSaveID()}).CurrentStage");

        m_curStage = PlayerPrefs.GetInt(key.ToString());
        LoadManager.instance.SetNextStageTypeIndex(m_curStage);
        LoadManager.instance.LoadNextStage();

        key.Replace("NextStageIndex", "CurrentChapter");
        m_curChapter = PlayerPrefs.GetInt(key.ToString());
    }

    private void Event_GameStop(object _sender, EventArgs _e)
    {

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
                int fullWeight = chapter1Weight[m_curStage - 1].battleWeight + chapter1Weight[m_curStage - 1].evWeight;
                int rand = UnityEngine.Random.Range(0, fullWeight - 1);

                if (rand < chapter1Weight[m_curStage - 1].battleWeight)
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
}
