using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleStageManager : MonoBehaviour
{
    private static BattleStageManager m_instance;
    public static BattleStageManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<BattleStageManager>();
            return m_instance;
        }
    }

    [Serializable] struct MonsterSetting
    {
        public int m_index;
        public int weight;
    }

    [Serializable] struct StageSetting
    {
        public MonsterSetting[] monsters;
    }

    [SerializeField] private GameObject[] monsterPrefabs;

    [Header("Chapter1 Setting")]
    [SerializeField] private StageSetting[] c1_stages;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
    }

    public GameObject GetMonster()
    {
        int curStage = StageManager.instance.curStage - 1;

        int fullWeight = 0;
        for(int i = 0; i < c1_stages[curStage].monsters.Length; i++)
        {
            fullWeight += c1_stages[curStage].monsters[i].weight;
        }
        int rand = UnityEngine.Random.Range(0, fullWeight - 1);

        fullWeight = 0;
        for(int i = 0; i < c1_stages[curStage].monsters.Length; i++)
        {
            fullWeight += c1_stages[curStage].monsters[i].weight;
            if (rand >= fullWeight - c1_stages[curStage].monsters[i].weight && rand < fullWeight)
            {
                return monsterPrefabs[c1_stages[curStage].monsters[i].m_index];
            }
        }

        Debug.LogError("배틀 스테이지 매니저가 몬스터를 제대로 넘겨주지 못했습니다.");
        return null;
    }

    public GameObject GetMonster(int index)
    {
        return monsterPrefabs[index];
    }
}
