﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStatManager : MonoBehaviour
{
    public static CStatManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<CStatManager>();
            }
            return m_instance;
        }
    }
    private static CStatManager m_instance;

    public int health { get; set; }
    public int endurance = 1;
    public int strength = 1;
    public int solid = 1;
    public int intelligent = 1;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetStats(out int endu, out int str, out int sol, out int _int)
    {
        endu = endurance;
        str = strength;
        sol = solid;
        _int = intelligent;
    }

    public void SetStats(int endu, int str, int sol, int _int)
    {
        endurance = endu;
        strength = str;
        solid = sol;
        intelligent = _int;
    }
}