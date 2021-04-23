﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    private bool isGetRes = false;
    private bool isRollEnd = false;
    private Rigidbody m_rb;
    private DiceSystemManager m_DsystemManager;
    private float activeTime = 0f;
    private int resNum = 0;
    private Sprite resImg;

    private bool isBottom = false;
    private float rollPower = 0;
    private float rollMinPowerRate = 0;

    private void OnEnable()
    {
        isRollEnd = false;
        isGetRes = false;
        isBottom = false;
        activeTime = Time.time;
    }

    private void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();
        m_DsystemManager = GameObject.FindGameObjectWithTag("DiceBox").GetComponent<DiceSystemManager>();
    }

    private void Update()
    {
        if (!isBottom)
        {
            DiceRolled();
        }
        else if((activeTime + 1.0f <= Time.time) && !isRollEnd)
        {
            if (m_rb.velocity.magnitude == 0)
            {
                m_DsystemManager.SumRollEnd();
                isRollEnd = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("DiceBox_Bottom"))
        {
            isBottom = true;
        }
    }

    public void SetRes(int _num, Sprite _spr)
    {
        if (!isGetRes)
        {
            resNum = _num;
            resImg = _spr;
            isGetRes = true;
        }
    }

    public void SetDiceRollPower(float _rollpower, float r_minPowerRate)
    {
        rollPower = _rollpower;
        rollMinPowerRate = r_minPowerRate;

    }

    private void DiceRolled()
    {
        m_rb.AddTorque(Random.Range(rollPower * rollMinPowerRate, rollPower)
            , Random.Range(rollPower * rollMinPowerRate, rollPower)
            , Random.Range(rollPower * rollMinPowerRate, rollPower));
    }

    public void SetResValue()
    {
        m_DsystemManager.resValue += resNum;
        m_DsystemManager.SetResImg(resImg);
    }
}
