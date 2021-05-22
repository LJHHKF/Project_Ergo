using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


//<레트로 유니티 게임 프로그래밍 에센스> 2권 참고하여 기본 제작 후 기능 추가
public class LivingEntity : MonoBehaviour, IDamageable
{
    //public int startingHealth { get; protected set; }
    public int fullHealth { get; protected set; }
    public int health { get; protected set; }
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
    public event Action<int> onHPDamage;

    [Header("Ref Setting")]
    [SerializeField] protected UnitUI myUI;
    [SerializeField] protected AbCondition myAbCond;
    [SerializeField] protected Animator myAnimator;

    protected virtual void OnEnable()
    {
        dead = false;
    }

    protected virtual void Start()
    {
        //TurnManager.instance.firstTurn += Event_FirstTurn;
    }

    protected virtual void OnDestroy()
    {
        ReleseTurnAct();
    }

    protected virtual void ReleseTurnAct()
    {
        //TurnManager.instance.firstTurn -= Event_FirstTurn;
    }

    protected virtual void Event_FirstTurn()
    {
        //HpAndGuardReset();
        //FlucStatReset();
        //CalculateStat();
    }

    protected virtual void Event_PlayerTurnEnd()
    { }

    protected virtual void Event_TurnEnd()
    { }
    protected virtual void Event_TurnStart()
    {}

    protected virtual void Event_BattleEnd()
    {}

    public virtual bool OnDamage(int damage)
    {
        bool isDamaged;
        if (GuardPoint > 0)
        {
            int prevGuardPoint = GuardPoint;
            GuardPoint -= damage;
            if (GuardPoint < 0)
            {
                myUI.AddPopUpText_GuardedDamage(prevGuardPoint);
                myUI.GuardBreakAnim();
                damage = Mathf.Abs(GuardPoint);
                GuardPoint = 0;
            }
            else
            {
                myUI.AddPopUpText_GuardedDamage(damage);
                damage = 0;
            }
            myUI.GuardUpdate();
        }

        if (GuardPoint <= 0)
        {
            health -= damage;
            myUI.AddPopUpText_Damage(damage);
            myUI.HpUpdate();

            isDamaged = true;
            onHPDamage?.Invoke(damage);
        }
        else
            isDamaged = false;


        if (health <= 0 && !dead)
        {
            Die();
        }
        return isDamaged;
    }

    public virtual void OnPenDamage(int damage)
    {
        health -= damage;
        myUI.AddPopUpText_Damage(damage);
        myUI.HpUpdate();
        onHPDamage?.Invoke(damage);

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void OnAddAbCond(int id, int pilledNum, bool isDelayed)
    {
        if (isDelayed)
            myAbCond.AddDelayedCondition(id, pilledNum);
        else
            myAbCond.AddImdiateAbCondition(id, pilledNum);
    }

    public virtual void RestoreHealth(int restoreValue)
    {
        if(dead)
        {
            return;
        }

        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.heal);

        if(health + restoreValue >= (fullHealth + endurance))
        {
            restoreValue = (fullHealth + endurance) - health;
            if (restoreValue <= 0)
                return;
            health += restoreValue;
            myUI.AddPopUpText_RestoreHealth(restoreValue);
        }
        else
        {
            health += restoreValue;
            myUI.AddPopUpText_RestoreHealth(restoreValue);
        }
    }

    public virtual void AddGuardPoint(int AddValue)
    {
        GuardPoint += AddValue;
        myUI.AddPopUpText_GetGuardPoint(AddValue);
        myUI.GuardGainAnim();
        myUI.GuardUpdate();
    }

    public virtual void DestroyGuard()
    {
        myUI.AddPopUpText_GuardedDamage(GuardPoint);
        GuardPoint = 0;
        myUI.GuardBreakAnim();
        myUI.GuardUpdate();
    }

    public int GetFullHealth()
    {
        return (fullHealth + endurance);
        //(endurance * 1);
    }

    public void ResetGuardPoint()
    {
        GuardPoint = 0;
        myUI.UnActiveGuardImg();
    }

    public virtual void ChangeCost(int changeV)
    {

    }

    public virtual void Die()
    {
        onDeath?.Invoke();
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

    protected virtual void ResetHP()
    {
        health = GetFullHealth();
        myUI.HpUpdate();
    }

    public void SetAnimTrigger(string _name)
    {
        if (_name.Length > 1)
            myAnimator.SetTrigger(_name);
    }
}
