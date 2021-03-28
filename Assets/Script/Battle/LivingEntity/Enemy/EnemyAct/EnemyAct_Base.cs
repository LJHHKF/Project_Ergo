using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAct_Base : MonoBehaviour, IEAct
{
    public enum Type
    {
        Physics,
        Maigc,
        Guard
    }
    [Header("Base Setting")]
    protected Enemy_Base m_Enemy;
    [SerializeField] protected int typeNumber = 0;
    [SerializeField] private Sprite actSprite;
    [SerializeField] private Type type;
    [SerializeField] private int power = 0;
    protected int r_power;
    //[SerializeField] private int abCondID = -1;
    //[SerializeField] private bool isRelatedAbCond = false;
    //[SerializeField] private bool isAllTargeted = false;

    protected virtual void Start()
    {
        m_Enemy = gameObject.GetComponent<Enemy_Base>();
    }

    public virtual void Act()
    {
    }


    public Sprite GetActSprite()
    {
        return actSprite;
    }

    public void GetActInfo(out int _power)
    {
        if(type == Type.Physics)
        {
            r_power = Mathf.RoundToInt(m_Enemy.strength * 0.5f) + power;
        }
        else if(type == Type.Maigc)
        {
            r_power = Mathf.RoundToInt(m_Enemy.intel * 0.5f) + power;
        }
        else if(type == Type.Guard)
        {
            r_power = Mathf.RoundToInt(m_Enemy.solid * 0.5f) + power;
        }
        _power = r_power;
    }

    //public virtual void GetActInfo(out int _power, out bool _isAll)
    //{
    //    _power = power;
    //    _isAll = isAllTargeted;
    //}

    public int GetActTypeNumber()
    {
        return typeNumber;
    }
}
