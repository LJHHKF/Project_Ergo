using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using mySoundEffect;

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
    [SerializeField] protected SoundEf soundType;
    [SerializeField] protected MonsterRank m_rank;
    [SerializeField] protected int dropSoul = 5;
    [SerializeField] protected bool hadHitAnim = false;
    [SerializeField] protected Acts[] normalActs;
    [SerializeField] protected EnemyAct_Base specialAct;
    protected EnemyAct_Base readyAct;
    [Header("E_Stat Setting")]
    [SerializeField] protected int _fullHealth = 100;
   // [SerializeField] protected int _startingHealth = 100;
    [SerializeField] protected int fix_Endurance = 1;
    [SerializeField] protected int fix_Strength = 1;
    [SerializeField] protected int fix_Solid = 1;
    [SerializeField] protected int fix_Inteligent = 1;
    [SerializeField] protected int maxSpGauge = 3;
    [Header("Effect Setting")]
    [SerializeField] protected MonsterEffectManager effectManager_monster;
    [SerializeField] protected GameObject dieEffect_prefab;
    protected int curSpGauge = 0;
    protected int r_maxSpGauge;

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

    public int DropSoul { get { return dropSoul; } }

    protected override void Start()
    {
        fix_endu = fix_Endurance;
        fix_stren = fix_Strength;
        fix_sol = fix_Solid;
        fix_int = fix_Inteligent;
        fullHealth = _fullHealth;
        //startingHealth = _startingHealth;
        r_maxSpGauge = maxSpGauge;

        TurnManager.instance.playerTurnEnd += Event_PlayerTurnEnd;
        TurnManager.instance.turnEnd += Event_TurnEnd;
        TurnManager.instance.firstTurn += Event_FirstTurn;

        onDeath += () => StartCoroutine(DelayedDestroy(1.0f));
        onDeath += () => Instantiate(dieEffect_prefab, gameObject.transform.position, Quaternion.identity);
    }

    protected override void ReleseTurnAct()
    {
        TurnManager.instance.playerTurnEnd -= Event_PlayerTurnEnd;
        TurnManager.instance.turnEnd -= Event_TurnEnd;
        TurnManager.instance.firstTurn -= Event_FirstTurn;
    }

    protected override void Event_PlayerTurnEnd()
    {
        ResetGuardPoint();
        myAbCond.Affected();
    }

    protected override void Event_TurnEnd()
    {
        if(specialAct != null)
            curSpGauge++;
        ActSetting();
    }

    protected override void Event_FirstTurn()
    {
        ResetHP();
        ResetGuardPoint();
        FlucStatReset();
        CalculateStat();
        ActSetting();
    }

    public override bool OnDamage(int damage)
    {
        SoundEfManager.instance.SetSoundEffect(soundType);
        if(hadHitAnim)
            myAnimator.SetTrigger("Hit");
        bool res = base.OnDamage(damage);
        return res;
    }

    public override void OnPenDamage(int damage)
    {
        SoundEfManager.instance.SetSoundEffect(soundType);
        if(hadHitAnim)
            myAnimator.SetTrigger("Hit");
        base.OnPenDamage(damage);
    }

    public override void ChangeCost(int changeV)
    {
        //if(curSpGauge - changeV < 0)
        //{
        //    changeV = curSpGauge;
        //}
        //else if(curSpGauge - changeV > maxSpGauge)
        //{
        //    changeV = -(maxSpGauge - curSpGauge);
        //}
        //curSpGauge -= changeV;

        //최대치 수정 방식으로 변경. -1이 들어오면 +1식으로 부호를 바꿔야 함.
        //if (maxSpGauge - changeV < 0)
        //    r_maxSpGauge = 0;
        //else
            r_maxSpGauge = maxSpGauge - changeV;
    }

    protected virtual void ActSetting()
    {
        bool isSuccess = false;
        if (curSpGauge >= r_maxSpGauge && specialAct != null)
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
        EnemyActType.AffectType[] types;
        int typeVariationNum;

        readyAct.GetActInfo(out powers, out types,out typeVariationNum);
        myUI.SetActInfo(readyAct.GetActSprite(), powers, types,typeVariationNum);
    }

    public void GetReadyActText(ref Text _head, ref Text _body)
    {
        _head.text = readyAct.GetActName();
        _body.text = readyAct.GetActPlainText();
    }

    public void Act()
    {
        readyAct.Act();
        myUI.AddPopUpText_ActionName(readyAct.GetActName());
    }

    public override void Die()
    {
        SoundEfManager.instance.SetSoundEffect(SoundEf.monsterDead);
        base.Die();
    }

    public void OnHit_HitAndRun()
    {
        effectManager_monster.OnHit_HitAndRun();
    }

    public void OnHit_HalfSwording()
    {
        effectManager_monster.OnHit_HalfSwording();
    }

    public void OnHit_PoisonShot()
    {
        effectManager_monster.OnHit_PoisonShot();
    }

    public void OnHit_BlowShot()
    {
        effectManager_monster.OnHit_BlowShot();
    }

    public void OnBulkUpEffect()
    {
        effectManager_monster.OnBulkUpEffect();
    }
}
