using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : LivingEntity
{
    [Header("Enemy Setting")]
    [SerializeField] protected int monsterID = 0;
    [SerializeField] protected EnemyAct_Base[] normalActs;
    [SerializeField] protected int[] weights;
    [SerializeField] protected EnemyAct_Base specialAct;
    private EnemyAct_Base readyAct;
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
        TurnManager.instance.playerTurnEnd += () => ResetGuardPoint();
        TurnManager.instance.playerTurnEnd += () => myAbCond.Affected();
        TurnManager.instance.turnEnd += () => curSpGauge++;

        TurnManager.instance.firstTurn += () => ActSetting();
        TurnManager.instance.turnEnd += () => ActSetting();

        onDeath += () => StartCoroutine(DelayedDestroy(1.0f));
    }

    protected override void ReleseTurnAct()
    {
        base.ReleseTurnAct();
        TurnManager.instance.playerTurnEnd -= () => ResetGuardPoint();
        TurnManager.instance.playerTurnEnd -= () => myAbCond.Affected();
        TurnManager.instance.turnEnd -= () => curSpGauge++;

        TurnManager.instance.firstTurn -= () => ActSetting();
        TurnManager.instance.turnEnd -= () => ActSetting();
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

        readyAct.GetActInfo(out power);
        myUI.SetActInfo(actSpr, power, readyAct.GetActTypeNumber());
    }

    public void Act()
    {
        readyAct.Act();
        Debug.Log("몬스터(인덱스:" + monsterFieldIndex + ")가 행동했습니다.");
    }
}
