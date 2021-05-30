using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<EnemiesManager>();
            return m_instance;
        }
    }
    private static EnemiesManager m_instance;

    [SerializeField] private float minX;
    [SerializeField] private float y_Init = 0;
    [SerializeField] private float x_interval = 1.0f;
    [SerializeField] private float time_interval = 1.0f;

    private int monsterMaxCnt = 3;
    private float deleteTime;

    private List<GameObject> monsters = new List<GameObject>();
    private int initCnt;
    private string key;
    private StringBuilder m_sb = new StringBuilder(30);

    public int stageSoul { get; private set; }

    private float m_list_min_x = 2000;
    private float m_list_max_x = -2000;

    private void Awake()
    {
        monsters.Capacity = monsterMaxCnt;
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        key = $"SaveID({GameMaster.instance.GetSaveID()}).LastMonsterNums";
        stageSoul = 0;
        if (!PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) <= 0 || GameMaster.instance.isInit)
        {
            int rand = UnityEngine.Random.Range(0, 9);

            if (StageManager.instance.curStage % 5 == 0)
            {
                GameObject mon = Instantiate(BattleStageManager.instance.GetMonster(), gameObject.transform);
                mon.transform.position = new Vector2(minX, y_Init);
                m_list_min_x = minX;
                m_list_max_x = minX;
                mon.name = "Enemy_" + mon.name + "_0";
                Enemy_Base temp = mon.GetComponent<Enemy_Base>();
                temp.monsterFieldIndex = 0;
                stageSoul += temp.DropSoul;
                temp.onDeath += () => RemoveAtMonstersList(mon);
                monsters.Add(mon);

                m_sb.Clear();
                m_sb.Append($"{key}.0");
                PlayerPrefs.SetInt(m_sb.ToString(), temp.m_ID);

                if(StageManager.instance.curStage / 5 < 3) // or != 3
                {
                    StoryTurningManager.instance.SetEliteStage(true);
                    switch(temp.m_ID)
                    {
                        case 1:
                            StoryTurningManager.instance.SetElite_Aries();
                            break;
                        case 4:
                            StoryTurningManager.instance.SetElite_Creta();
                            break;
                        case 7:
                            StoryTurningManager.instance.SetElite_JunkGoblin();
                            break;
                    }
                }
                else
                {
                    StoryTurningManager.instance.SetBossStage(true);
                    switch (temp.m_ID)
                    {
                        case 2:
                            StoryTurningManager.instance.SetBoss_Aries();
                            break;
                        case 5:
                            StoryTurningManager.instance.SetBoss_Creta();
                            break;
                        case 8:
                            StoryTurningManager.instance.SetBoss_JunkGoblin();
                            break;
                    }
                }
            }
            else
            {
                int curStageCnt;

                if (rand < 3)
                    curStageCnt = 1;
                else if (rand < 8)
                    curStageCnt = 2;
                else //if (rand < 10)
                    curStageCnt = 3;


                for (int i = 0; i < curStageCnt; i++)
                {
                    int _i = i; //델리게이트 연관 등서 i값을 제대로 못 받는 경우가 있어서 습관적 추가
                    GameObject mon = Instantiate(BattleStageManager.instance.GetMonster(), gameObject.transform);
                    mon.transform.position = new Vector2(minX + (x_interval * _i), y_Init);
                    if (m_list_min_x > mon.transform.position.x)
                        m_list_min_x = mon.transform.position.x;
                    if (m_list_max_x < mon.transform.position.x)
                        m_list_max_x = mon.transform.position.x;
                    mon.name = "Enemy_" + mon.name + "_" + _i.ToString("00");
                    Enemy_Base temp = mon.GetComponent<Enemy_Base>();
                    temp.monsterFieldIndex = _i;
                    stageSoul += temp.DropSoul;
                    temp.onDeath += () => RemoveAtMonstersList(mon);
                    monsters.Add(mon);

                    m_sb.Clear();
                    m_sb.Append($"{key}.{_i}");
                    PlayerPrefs.SetInt(m_sb.ToString(), temp.m_ID);
                }
            }

            PlayerPrefs.SetInt(key, monsters.Count);
        }
        else
        {
            for (int i = 0; i < PlayerPrefs.GetInt(key); i++)
            {
                int _i = i;
                m_sb.Clear();
                m_sb.Append($"{key}.{_i}");
                //int m_id = PlayerPrefs.GetInt(m_sb.ToString());
                GameObject mon = Instantiate(BattleStageManager.instance.GetMonster(PlayerPrefs.GetInt(m_sb.ToString())), gameObject.transform);
                mon.transform.position = new Vector2(minX + (x_interval * _i), y_Init);
                if (m_list_min_x > mon.transform.position.x)
                    m_list_min_x = mon.transform.position.x;
                if (m_list_max_x < mon.transform.position.x)
                    m_list_max_x = mon.transform.position.x;
                mon.name = "Enemy_" + mon.name + "_" + _i.ToString("00");
                Enemy_Base temp = mon.GetComponent<Enemy_Base>();
                temp.monsterFieldIndex = _i;
                stageSoul += temp.DropSoul;
                temp.onDeath += () => RemoveAtMonstersList(mon);
                monsters.Add(mon);
            }
        }
        initCnt = monsters.Count;
        TurnManager.instance.playerTurnEnd += Event_PlayerTurnEnd;
        GameMaster.instance.gameStop += Event_GameStop;
    }

    private void OnDestroy()
    {
        TurnManager.instance.playerTurnEnd -= Event_PlayerTurnEnd;
        GameMaster.instance.gameStop -= Event_GameStop;
        m_instance = null;
    }

    private void Event_PlayerTurnEnd()
    {
        StartCoroutine(StartMonsterActsControl());
    }

    private void Event_GameStop()
    {
        PlayerPrefs.SetInt(key, initCnt);
    }

    void RemoveAtMonstersList(GameObject who)
    {
        deleteTime = Time.time;
        for(int i = 0; i < monsters.Count; i++)
        {
            int _i = i;
            if (ReferenceEquals(monsters[_i], who))
            {
                monsters.RemoveAt(_i);
            }
        }
        if (monsters.Count == 0)
        {
            StartCoroutine(DelayedExecute());
            PlayerPrefs.DeleteKey(key);
            for(int i = 0; i < initCnt; i++)
            {
                int _i = i;
                m_sb.Clear();
                m_sb.Append($"{key}.{_i}");
                PlayerPrefs.DeleteKey(m_sb.ToString());
            }
        }

        IEnumerator DelayedExecute()
        {
            yield return new WaitForSeconds(2.0f);
            TurnManager.instance.OnBattleEnd();
            yield break;
        }
    }

    public int GetMaxMonsCnt()
    {
        return monsters.Count;
    }

    public int GetInitCnt()
    {
        return initCnt;
    }

    public void AddMultiRTarget_NotOverlaped(ref List<GameObject> o_list, int max)
    {
        int[] rands = new int[max];
        bool isSuccess;
        for(int i = 0; i < max; i++)
        {
            int _i = i;
            isSuccess = false;
            while(!isSuccess)
            {
                rands[_i] = UnityEngine.Random.Range(0, monsters.Count);
                bool isS2 = true;
                for(int j = 0; j < i+1; j++)
                {
                    int _j = j;
                    if (_i != _j)
                    {
                        if (rands[_i] == rands[_j])
                        {
                            isS2 = false;
                        }
                    }
                }
                if(isS2)
                {
                    isSuccess = true;
                }
            }
            o_list.Add(monsters[rands[_i]]);
        }
    }

    public void AddMultiRTarget_Overlaped(ref List<GameObject> o_list, int max)
    {
        for(int i = 0; i < max; i++)
        {
            int rand = UnityEngine.Random.Range(0, monsters.Count);
            o_list.Add(monsters[rand]);
        }
    }

    public void AddAllTargeted(ref List<GameObject> o_list)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            int _i = i;
            o_list.Add(monsters[_i]);
        }
    }

    public void AllDamaged(int dmg)
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            int _i = i;
            monsters[_i].GetComponent<LivingEntity>().OnDamage(dmg);
        }
    }

    public bool SearchHadMonster(int _id)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            int _i = i;
            Enemy_Base temp = monsters[_i].GetComponent<Enemy_Base>();
            if (temp.m_ID == _id)
                return true;
        }
        return false;
    }

    public void SummonMonster(int _id, int _max)
    {
        for(int i = 0; i < _max; i++)
        {
            int _i = i;
            GameObject mon = Instantiate(BattleStageManager.instance.GetMonster(_id), gameObject.transform);
            mon.name = "Enemy_" + mon.name + "_s_" + _i.ToString("00");
            Enemy_Base temp = mon.GetComponent<Enemy_Base>();
            temp.monsterFieldIndex = _i;
            stageSoul += temp.DropSoul;
            temp.onDeath += () => RemoveAtMonstersList(mon);
            monsters.Add(mon);
        }
        ReSortMonsters();
    }

    private void ReSortMonsters()
    {
        int j = monsters.Count - 1;
        m_list_min_x = 2000;
        m_list_max_x = -2000;
        for (int i = 0; i < monsters.Count; i--)
        {
            int _i = i;
            monsters[_i].transform.position = new Vector2(minX + (x_interval * j--), y_Init);
            if (m_list_min_x > monsters[_i].transform.position.x)
                m_list_min_x = monsters[_i].transform.position.x;
            if (m_list_max_x < monsters[_i].transform.position.x)
                m_list_max_x = monsters[_i].transform.position.x;
        }
    }

    public float GetMonterListMiddle_x()
    {
        return (m_list_max_x - m_list_min_x) / 2.0f;
    }

    IEnumerator StartMonsterActsControl()
    {
        float curTime = Time.time;
        if(curTime - deleteTime < 2.0f)
        {
            yield return new WaitForSeconds(2.0f);
        }
        for(int i = 0; i < monsters.Count; i++)
        {
            int _i = i;
            yield return new WaitForSeconds(time_interval);
            monsters[_i].GetComponent<Enemy_Base>().Act();
        }
        yield return new WaitForSeconds(time_interval);
        TurnManager.instance.OnTurnEnd();
        yield break;
    }
}
