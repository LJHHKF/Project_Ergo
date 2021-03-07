using System;
using UnityEngine;


//<레트로 유니티 게임 프로그래밍 에센스> 2권 참고
public class LivingEntity : MonoBehaviour, IDamageable
{
    public int startingHealth = 100;
    public int fullHealth = 100;
    public int health { get; protected set; }
    public bool dead { get; protected set; }
    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    public virtual void OnDamage(int damage)
    {
        health -= damage;

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

    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }
}
