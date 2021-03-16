using System;
using UnityEngine;


//<레트로 유니티 게임 프로그래밍 에센스> 2권 참고하여 기본 제작 후 기능 추가
public class LivingEntity : MonoBehaviour, IDamageable
{
    public int startingHealth = 100;
    public int fullHealth = 100;
    public int health { get; protected set; }
    public int regenGuardPoint = 0;
    public int GuardPoint { get; protected set; }
    public bool dead { get; protected set; }
    protected TurnManager m_turnM;
    
    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
        GuardPoint = regenGuardPoint;
    }

    protected virtual void Start()
    {
        m_turnM = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
    }

    public virtual void OnDamage(int damage)
    {
        if (GuardPoint > 0)
        {
            GuardPoint -= damage;
            if (GuardPoint < 0)
            {
                damage = Mathf.Abs(GuardPoint);
            }
            else
            {
                damage = 0;
            }
        }
        
        if (GuardPoint <= 0)
        {
            health -= damage;
        }

        if(health <= 0 && !dead)
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
        
        if(health + restoreValue >= fullHealth)
        {
            restoreValue -= fullHealth;
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
    }

    public void ResetGuardPoint()
    {
        GuardPoint = regenGuardPoint;
    }

    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }
}
