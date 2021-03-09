using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public bool isGetRes = false;
    private bool isRollEnd = false;
    private Rigidbody m_rb;
    private DiceSystemManager m_DsystemManager;
    private float activeTime = 0f;
    private int result = 0;
    public int resNum {
        get
        {
            if (isGetRes)
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if(!isGetRes)
            {
                result = value;
                isGetRes = true;
            }
        }
    }

    private void OnEnable()
    {
        isRollEnd = false;
        isGetRes = false;
        activeTime = Time.time;
    }

    private void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();
        m_DsystemManager = GameObject.FindGameObjectWithTag("DiceBox").GetComponent<DiceSystemManager>();
    }

    private void Update()
    {
        if (activeTime + 1.0f <= Time.time)
        {
            if (m_rb.velocity.magnitude == 0f && !isRollEnd)
            {
                isRollEnd = true;
                m_DsystemManager.SumRollEnd();
            }
        }
    }

    public void SetResValue()
    {
        m_DsystemManager.resValue += resNum;
    }
}
