using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : LivingEntity
{
    [Header("Enemy Setting")]
    public int monsterID = 0;
    public EnemyAct_Base[] normalActs;
    public int[] weights;
    public EnemyAct_Base specialAct;
    private EnemyAct_Base readyAct;
    [Header("E_Stat Setting")]
    public int fix_Endurance = 1;
    public int fix_Strength = 1;
    public int fix_Solid = 1;
    public int fix_Inteligent = 1;
    public int maxSpGauge = 3;
    protected int curSpGauge = 0;

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
        base.Start();
        m_turnM.playerTurnEnd += () => ResetGuardPoint();
        m_turnM.playerTurnEnd += () => myAbCond.Affected();
        m_turnM.turnEnd += () => curSpGauge++;

        m_turnM.firstTurn += () => ActSetting();
        m_turnM.turnEnd += () => ActSetting();

        onDeath += () => StartCoroutine(DelayedUnActived(1.0f));
    }

    protected virtual void Update()
    {
        
    }

    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")가 데미지를 입었습니다. :" + damage);
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")의 남은 체력 :" + health);
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

    private void ActSetting()
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

            for (int i = 0; i < weights.Length; i++)
                fullWeight += weights[i];

            rand = Random.Range(0, fullWeight);

            for(int i = 0; i < weights.Length; i++)
            {
                max += weights[i];
                if(rand >= max-weights[i] && rand < max)
                {
                    readyAct = normalActs[i];
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
        Sprite actSpr = readyAct.GetActSprite();
        int power = -1;
        //int methodID = -1;
        //bool isAll = false;
        //int abcondID = -1;

        //if(readyAct.GetIsAbCondAct())
        //{
        //    readyAct.GetActInfo(out power, out abcondID, out isAll);
        //    myUI.SetActInfo(0, actSpr, power, isAll, abcondID);
        //}
        //else
        //{
        readyAct.GetActInfo(out power);
        myUI.SetActInfo(actSpr, power, readyAct.GetActTypeNumber());
            //abcondID);
        //}
    }

    public void Act()
    {
        readyAct.Act();
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")가 행동했습니다.");
    }
}
