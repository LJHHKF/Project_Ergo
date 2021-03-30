using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


//<레트로 유니티 게임 프로그래밍 에센스> 2권 참고하여 기본 제작 후 기능 추가
public class LivingEntity : MonoBehaviour, IDamageable
{
    //public int startingHealth { get; protected set; }
    public int fullHealth { get { return r_fullHealth; } protected set { r_fullHealth = value; } }
    protected int r_fullHealth = 100;
    public int health { get; protected set; }
    public int regenGuardPoint { get; protected set; }
    public int GuardPoint { get; protected set; }
    //[Header("Stat Setting")]
    protected int fix_endu = 1;
    protected int fix_stren = 1;
    protected int fix_sol = 1;
    protected int fix_int = 1;
    public int fluc_endurance { get; set; }
    public int fluc_strength { get; set; }
    public int fluc_solid { get; set; }
    public int fluc_intel { get; set; }
    public int endurance { get; protected set; }
    public int strength { get; protected set; }
    public int solid { get; protected set; }
    public int intel { get; protected set; }

    public bool dead { get; protected set; }
    public event Action onDeath;

    [Header("Ref Setting")]
    [SerializeField] protected UnitUI myUI;
    [SerializeField] protected AbCondition myAbCond;

    protected virtual void OnEnable()
    {
        dead = false;
    }

    protected virtual void Start()
    {
        TurnManager.instance.firstTurn += Event_FirstTurn;
    }

    protected virtual void OnDestroy()
    {
        ReleseTurnAct();
    }

    protected virtual void ReleseTurnAct()
    {
        TurnManager.instance.firstTurn -= Event_FirstTurn;
    }

    protected virtual void Event_FirstTurn(object _o, EventArgs _e)
    {
        HpAndGuardReset();
        FlucStatReset();
        CalculateStat();
    }

    protected virtual void Event_PlayerTurnEnd(object _o, EventArgs _e)
    { }

    protected virtual void Event_TurnEnd(object _o, EventArgs _e)
    { }
    protected virtual void Event_TurnStart(object _o, EventArgs _e)
    {}

    protected virtual void Event_BattleEnd(object _o, EventArgs _e)
    {}

    public virtual void OnDamage(int damage)
    {
        if (GuardPoint > 0)
        {
            GuardPoint -= damage;
            if (GuardPoint < 0)
            {
                damage = Mathf.Abs(GuardPoint);
                GuardPoint = 0;
            }
            else
            {
                damage = 0;
            }
            myUI.GuardUpdate();
        }
        
        if (GuardPoint <= 0)
        {
            health -= damage;
            myUI.HpUpdate();
        }


        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void OnPenDamage(int damage)
    {
        health -= damage;
        myUI.HpUpdate();

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void RestoreHealth(int restoreValue)
    {
        if(dead)
        {
            return;
        }
        
        if(health + restoreValue >= (fullHealth + endurance))
        {
            restoreValue -= (fullHealth + endurance);
            if (restoreValue <= 0)
                return;
            health += restoreValue;
        }
        else
        {
            health += restoreValue;
        }
    }

    public virtual void GetGuardPoint(int GetValue)
    {
        GuardPoint += GetValue;
        myUI.GuardUpdate();
    }

    public int GetFullHealth()
    {
        return fullHealth + endurance;
        //(endurance * 1);
    }

    public void ResetGuardPoint()
    {
        GuardPoint = regenGuardPoint;
        myUI.GuardUpdate();
    }

    public virtual void ChangeCost(int changeV)
    {

    }

    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }

    protected IEnumerator DelayedDestroy(float sec)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
        yield break;
    }

    public void FlucStatReset()
    {
        fluc_endurance = 0;
        fluc_strength = 0;
        fluc_solid = 0;
        fluc_intel = 0;
    }

    public void CalculateStat()
    {
        if(fix_endu + fluc_endurance < 0)
        {
            endurance = 0;
        }
        else
        {
            endurance = fix_endu + fluc_endurance;
        }

        if(fix_stren + fluc_strength < 0)
        {
            strength = 0;
        }
        else
        {
            strength = fix_stren + fluc_strength;
        }

        if(fix_sol + fluc_solid < 0)
        {
            solid = 0;
        }
        else
        {
            solid = fix_sol + fluc_solid;
        }

        if(fix_int + fluc_intel < 0)
        {
            intel = 0;
        }
        else
        {
            intel = fix_int + fluc_intel;
        }
    }

    protected virtual void HpAndGuardReset()
    {
        //health = startingHealth + endurance; // 만약 시작 HP는 스탯 영향 안 받게 하고 싶을 경우는 수정.
        health = GetFullHealth();
        GuardPoint = regenGuardPoint;
        myUI.HpUpdate();
        myUI.GuardUpdate();
    }

}
