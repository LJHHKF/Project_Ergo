using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_Base : LivingEntity
{
    [Serializable]
    public struct Acts
    {
        public EnemyAct_Base act;
        public int weight;
    }

    public enum MonsterRank
    {
        Common,
        Elite,
        Boss
    }

    [Header("Enemy Setting")]
    [SerializeField] protected int monsterID = 0;
    [SerializeField] protected MonsterRank m_rank;
    [SerializeField] protected Acts[] normalActs;
    //[SerializeField] protected int[] weights;
    [SerializeField] protected EnemyAct_Base specialAct;
    protected EnemyAct_Base readyAct;
    [Header("E_Stat Setting")]
    [SerializeField] protected int _fullHealth = 100;
   // [SerializeField] protected int _startingHealth = 100;
    [SerializeField] protected int _regenGuardPoint = 0;
    [SerializeField] protected int fix_Endurance = 1;
    [SerializeField] protected int fix_Strength = 1;
    [SerializeField] protected int fix_Solid = 1;
    [SerializeField] protected int fix_Inteligent = 1;
    [SerializeField] protected int maxSpGauge = 3;
    protected int curSpGauge = 0;

    public int m_ID { get { return monsterID; } }

    private int m_FieldIndex = 0;
    public int monsterFieldIndex  // 디버그용 임시코드
    {
        get
        {
            return m_FieldIndex;
        }
        set
        {
            m_FieldIndex = value;
        }
    }

    protected override void Start()
    {
        fix_endu = fix_Endurance;
        fix_stren = fix_Strength;
        fix_sol = fix_Solid;
        fix_int = fix_Inteligent;
        fullHealth = _fullHealth;
        //startingHealth = _startingHealth;
        regenGuardPoint = _regenGuardPoint;
        base.Start();
        TurnManager.instance.playerTurnEnd += Event_PlayerTurnEnd;
        TurnManager.instance.turnEnd += Event_TurnEnd;
        TurnManager.instance.firstTurn += Event_FirstTurn;

        onDeath += () => StartCoroutine(DelayedDestroy(1.0f));
    }

    protected override void ReleseTurnAct()
    {
        base.ReleseTurnAct();
        TurnManager.instance.playerTurnEnd -= Event_PlayerTurnEnd;
        TurnManager.instance.turnEnd -= Event_TurnEnd;
        TurnManager.instance.firstTurn -= Event_FirstTurn;
    }

    protected override void Event_PlayerTurnEnd(object _o, EventArgs _e)
    {
        base.Event_PlayerTurnEnd(_o, _e);
        ResetGuardPoint();
        myAbCond.Affected();
    }

    protected override void Event_TurnEnd(object _o, EventArgs _e)
    {
        base.Event_TurnEnd(_o, _e);
        curSpGauge++;
        ActSetting();
    }

    protected override void Event_FirstTurn(object _o, EventArgs _e)
    {
        base.Event_FirstTurn(_o, _e);
        ActSetting();
    }

    protected virtual void Update()
    {
        
    }

    public override bool OnDamage(int damage)
    {
        bool res = base.OnDamage(damage);
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")가 데미지를 입었습니다. :" + damage);
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")의 남은 체력 :" + health);
        return res;
    }

    public override void ChangeCost(int changeV)
    {
        if(curSpGauge + changeV < 0)
        {
            changeV = -curSpGauge;
        }
        else if(curSpGauge + changeV > maxSpGauge)
        {
            changeV = maxSpGauge - curSpGauge;
        }
        curSpGauge += changeV;
    }

    protected virtual void ActSetting()
    {
        bool isSuccess = false;
        if (curSpGauge == maxSpGauge)
        {
            readyAct = specialAct;
            isSuccess = true;
            curSpGauge = 0;
        }
        else
        {
            int fullWeight = -1; // 20+40+40 으로 100일경우, 랜덤값은 0~99 얻어야 함.
            int rand;
            int max = 0;

            for (int i = 0; i <  normalActs.Length; i++)
                fullWeight += normalActs[i].weight;

            rand = UnityEngine.Random.Range(0, fullWeight);

            for(int i = 0; i < normalActs.Length; i++)
            {
                max += normalActs[i].weight;
                if(rand >= max-normalActs[i].weight && rand < max)
                {
                    readyAct = normalActs[i].act;
                    isSuccess = true;
                    break;
                }
            }
        }
        if (!isSuccess)
            Debug.LogError("행동 설정에 실패한 몬스터가 있습니다.");
        else
            GetReadyActInfo();
    }

    public void GetReadyActInfo()
    {
        int[] powers;
        int[] repeat;
        EnemyActType.AffectType[] types;
        int typeVariationNum;

        readyAct.GetActInfo(out powers, out types,out repeat ,out typeVariationNum);
        myUI.SetActInfo(readyAct.GetActSprite(), powers, types,repeat,typeVariationNum);
    }

    public void Act()
    {
        readyAct.Act();
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")가 행동했습니다.");
    }
}
