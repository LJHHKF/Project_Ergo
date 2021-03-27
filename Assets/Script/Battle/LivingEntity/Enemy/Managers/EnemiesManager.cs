using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    //public int initAmount; // 스테이지 관리자를 만든 후엔 이 부분은 거기서 얻어올 것.
    public TurnManager turnManager;

    public float minX;
    public float x_interval = 1.0f;
    public float time_interval = 1.0f;

    public int[] m_index;
    public GameObject[] m_array;

    private List<GameObject> monsters = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < m_index.Length; i++)
        {
            GameObject mon = Instantiate(m_array[m_index[i]], gameObject.transform);
            mon.transform.position = new Vector2(minX + (x_interval * i), 0);
            mon.name = "Enemy_" + m_index[i].ToString("00") + "_" + i.ToString("00");
            Enemy_Base temp = mon.GetComponent<Enemy_Base>();
            temp.monsterFieldIndex = i;
            temp.onDeath += () => SortMonstersList(mon);
            monsters.Add(mon);
        }

        turnManager.playerTurnEnd += () => StartCoroutine(StartMonsterActsControl());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SortMonstersList(GameObject who)
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            if (ReferenceEquals(monsters[i], who))
            {
                monsters.RemoveAt(i);
            }
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
                rands[i] = Random.Range(0, monsters.Count);
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
            int rand = Random.Range(0, monsters.Count);
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
        for(int i = 0; i < monsters.Count; i++)
        {
            Enemy_Base temp = monsters[i].GetComponent<Enemy_Base>();
            Debug.Log("행동 대기:" + time_interval + "초");
            yield return new WaitForSeconds(time_interval);
            temp.Act();
        }
        turnManager.OnTurnEnd();
        yield break;
    }
}
