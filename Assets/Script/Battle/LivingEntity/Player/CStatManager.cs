using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

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

    private struct AbCond_Saved
    {
        public int id;
        public int piledNum;
    }

    [Header("Init Stat Setting")]
    [SerializeField] private int init_FullHealth = 100;
    //public int init_StartingHealth = 100;
    [SerializeField] private int init_Endurance = 1;
    [SerializeField] private int init_Strength = 1;
    [SerializeField] private int init_Solid = 1;
    [SerializeField] private int init_Intelligent = 1;
    private List<AbCond_Saved> abcond_list;

    public int fullHealth_pure { get; set; }
    //public int startingHealth { get; set; }
    public int health { get; private set; }
    public int endurance { get; set; }
    public int strength { get; set; }
    public int solid { get; set; }
    public int intelligent { get; set; }

    private StringBuilder key = new StringBuilder();

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        abcond_list.Capacity = AbCondInfoManager.instance.GetAbCondListLength();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameMaster.instance.initSaveData_Awake += Event_InitSaveDataAwake;
        GameMaster.instance.startGame_Awake += Event_StartGameAwake;
        GameMaster.instance.stageEnd += Event_StageEnd;
    }

    private void OnDestroy()
    {
        m_instance = null;
        GameMaster.instance.initSaveData_Awake -= Event_InitSaveDataAwake;
        GameMaster.instance.startGame_Awake -= Event_StartGameAwake;
        GameMaster.instance.stageEnd -= Event_StageEnd;
    }

    private void Event_InitSaveDataAwake(object _o, EventArgs _e)
    {
        endurance = init_Endurance;
        strength = init_Strength;
        solid = init_Solid;
        intelligent = init_Intelligent;
        fullHealth_pure = init_FullHealth;
        health = CalcResultFullHealth();

        SaveStats();
    }

    private void Event_StartGameAwake(object _o, EventArgs _e)
    {
        int saveID = GameMaster.instance.GetSaveID();
        key.Clear();
        key.Append($"SaveID({saveID}).CStat.Endurance");

        endurance = PlayerPrefs.GetInt(key.ToString());
        key.Replace("Endurance", "Strength");
        strength = PlayerPrefs.GetInt(key.ToString());
        key.Replace("Strength", "Solid");
        solid = PlayerPrefs.GetInt(key.ToString());
        key.Replace("Solid", "Intelligent");
        intelligent = PlayerPrefs.GetInt(key.ToString());
        key.Replace("Intelligent", "FullHealth");
        fullHealth_pure = PlayerPrefs.GetInt(key.ToString());
        key.Replace("FullHealth", "Health");
        health = PlayerPrefs.GetInt(key.ToString());
    }

    private void Event_StageEnd(object _o, EventArgs _e)
    {
        SaveStats();
    }

    private int CalcResultFullHealth()
    {
        return fullHealth_pure + endurance;
        //(endurance * 1);
    }

    private void SaveStats()
    {
        int saveID = GameMaster.instance.GetSaveID();
        key.Clear();
        key.Append($"SaveID({saveID}).CStat.Endurance");

        PlayerPrefs.SetInt(key.ToString(), endurance);
        key.Replace("Endurance", "Strength");
        PlayerPrefs.SetInt(key.ToString(), strength);
        key.Replace("Strength", "Solid");
        PlayerPrefs.SetInt(key.ToString(), solid);
        key.Replace("Solid", "Intelligent");
        PlayerPrefs.SetInt(key.ToString(), intelligent);
        key.Replace("Intelligent", "FullHealth");
        PlayerPrefs.SetInt(key.ToString(), fullHealth_pure);
        key.Replace("FullHealth", "Health");
        PlayerPrefs.SetInt(key.ToString(), health);
    }

    public void HealthPointUpdate(int value)
    {
        health = value;
        if(health <= 0)
        {
            GameMaster.instance.OnGameOver(); // 실패 시임.
            LoadManager.instance.LoadGameOver();
        }
    }

    public void GetInheritedAbCond(ref AbCondition _target)
    {
        if (abcond_list.Count > 0)
        {
            for (int i = 0; i < abcond_list.Count; i++)
            {
                _target.AddImdiateAbCondition(abcond_list[i].id, abcond_list[i].piledNum);
            }
            abcond_list.Clear();
        }
        return;
    }

    public void SetInheriteAbCond(int _id, int _piledNum)
    {
        AbCond_Saved temp;
        temp.id = _id;
        temp.piledNum = _piledNum;
        abcond_list.Add(temp);
    }
}
