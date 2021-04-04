using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemiesManager : MonoBehaviour
{
    //[SerializeField] private int initAmount; // 스테이지 관리자를 만든 후엔 이 부분은 거기서 얻어올 것.

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
    [SerializeField] private float x_interval = 1.0f;
    [SerializeField] private float time_interval = 1.0f;

    [SerializeField] private int[] m_index;
    [SerializeField] private GameObject[] m_array;
    private int monsterMaxCnt = 3;
    private float deleteTime;

    private List<GameObject> monsters = new List<GameObject>();
    private string key;

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
        string key = $"SaveID({GameMaster.instance.GetSaveID()}).LastMonsterNums"; // 몬스터 타입 인덱스도 랜덤이므로, 추후 관련 저장 기능 만들어야 함.
        if (!PlayerPrefs.HasKey(key) || PlayerPrefs.GetInt(key) == -1)
        {
            int rand = UnityEngine.Random.Range(0, 9);
            int curStageCnt;
            if (rand < 3)
                curStageCnt = 1;
            else if (rand < 8)
                curStageCnt = 2;
            else //if (rand < 10)
                curStageCnt = 3;

            for (int i = 0; i < curStageCnt; i++)
            {
                GameObject mon = Instantiate(m_array[m_index[i]], gameObject.transform);
                mon.transform.position = new Vector2(minX + (x_interval * i), 0);
                mon.name = "Enemy_" + m_index[i].ToString("00") + "_" + i.ToString("00");
                Enemy_Base temp = mon.GetComponent<Enemy_Base>();
                temp.monsterFieldIndex = i;
                temp.onDeath += () => RemoveAtMonstersList(mon);
                monsters.Add(mon);
            }
            PlayerPrefs.SetInt(key, monsters.Count);
        }
        else
        {
            for (int i = 0; i < PlayerPrefs.GetInt(key); i++)
            {
                GameObject mon = Instantiate(m_array[m_index[i]], gameObject.transform);
                mon.transform.position = new Vector2(minX + (x_interval * i), 0);
                mon.name = "Enemy_" + m_index[i].ToString("00") + "_" + i.ToString("00");
                Enemy_Base temp = mon.GetComponent<Enemy_Base>();
                temp.monsterFieldIndex = i;
                temp.onDeath += () => RemoveAtMonstersList(mon);
                monsters.Add(mon);
            }
        }
        TurnManager.instance.playerTurnEnd += Event_PlayerTurnEnd;
        GameMaster.instance.gameStop += Event_GameStop;
    }

    private void OnDestroy()
    {
        TurnManager.instance.playerTurnEnd -= Event_PlayerTurnEnd;
        GameMaster.instance.gameStop -= Event_GameStop;
        m_instance = null;
    }

    private void Event_PlayerTurnEnd(object _o, EventArgs _e)
    {
        StartCoroutine(StartMonsterActsControl());
    }

    private void Event_GameStop(object _o, EventArgs _e)
    {
        string key = $"SaveID({GameMaster.instance.GetSaveID()}).LastMonsterNums";
        PlayerPrefs.SetInt(key, monsters.Count);
    }

    void RemoveAtMonstersList(GameObject who)
    {
        deleteTime = Time.time;
        for(int i = 0; i < monsters.Count; i++)
        {
            if (ReferenceEquals(monsters[i], who))
            {
                monsters.RemoveAt(i);
            }
        }
        if (monsters.Count == 0)
        {
            TurnManager.instance.OnBattleEnd();
            string key = $"SaveID({GameMaster.instance.GetSaveID()}).LastMonsterNums";
            PlayerPrefs.SetInt(key, -1);
        }
    }

    public int GetMaxMonsCnt()
    {
        return monsters.Count;
    }

    public void AddMultiRTarget_NotOverlaped(ref List<GameObject> o_list, int max)
    {
        int[] rands = new int[max];
        bool isSuccess;
        for(int i = 0; i < max; i++)
        {
            isSuccess = false;
            while(!isSuccess)
            {
                rands[i] = UnityEngine.Random.Range(0, monsters.Count);
                bool isS2 = true;
                for(int j = 0; j < i+1; j++)
                {
                    if (i != j)
                    {
                        if (rands[i] == rands[j])
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
            o_list.Add(monsters[rands[i]]);
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
            o_list.Add(monsters[i]);
        }
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
            Enemy_Base temp = monsters[i].GetComponent<Enemy_Base>();
            Debug.Log("행동 대기:" + time_interval + "초");
            yield return new WaitForSeconds(time_interval);
            temp.Act();
        }
        TurnManager.instance.OnTurnEnd();
        yield break;
    }
}
