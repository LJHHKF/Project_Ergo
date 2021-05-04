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
    private List<AbCond_Saved> abcond_list = new List<AbCond_Saved>();

    public int fullHealth_pure { get; set; }
    //public int startingHealth { get; set; }
    public int health { get; private set; }
    public int endurance { get; private set; }
    public int strength { get; private set; }
    public int solid { get; private set; }
    public int intelligent { get; private set; }

    private StringBuilder key = new StringBuilder();

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
        GameMaster.instance.initSaveData_Awake += Event_InitSaveDataAwake;
        GameMaster.instance.startGame_Awake += Event_StartGameAwake;
        GameMaster.instance.stageEnd += Event_StageEnd;
        GameMaster.instance.battleStageEnd += Event_BattleStageEnd;
        GameMaster.instance.gameOver += Event_GameOver;

        abcond_list.Capacity = AbCondInfoManager.instance.GetAbCondListLength() + 1;
    }

    private void OnDestroy()
    {
        m_instance = null;
        GameMaster.instance.initSaveData_Awake -= Event_InitSaveDataAwake;
        GameMaster.instance.startGame_Awake -= Event_StartGameAwake;
        GameMaster.instance.stageEnd -= Event_StageEnd;
        GameMaster.instance.battleStageEnd -= Event_BattleStageEnd;
        GameMaster.instance.gameOver -= Event_GameOver;
    }

    private void Event_InitSaveDataAwake(object _o, EventArgs _e)
    {
        endurance = init_Endurance;
        strength = init_Strength;
        solid = init_Solid;
        intelligent = init_Intelligent;
        fullHealth_pure = init_FullHealth;
        health = GetCalcFullHealth();

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

        key.Replace("Health", "Abcond.Length");
        if(PlayerPrefs.HasKey(key.ToString()))
        {
            int _leng = PlayerPrefs.GetInt(key.ToString());
            abcond_list.Clear();
            for(int i = 0; i < _leng; i++)
            {
                AbCond_Saved _temp;
                int _i = i;
                if(_i == 0)
                {
                    key.Replace("Length", "1.ID");
                }
                else
                {
                    key.Replace($"{abcond_list.Count}.PiledNum", $"{abcond_list.Count + 1}.ID");
                }
                _temp.id = PlayerPrefs.GetInt(key.ToString());
                key.Replace("ID", "PiledNum");
                _temp.piledNum = PlayerPrefs.GetInt(key.ToString());
                abcond_list.Add(_temp);
            }
        }
    }

    private void Event_GameOver(object _o, EventArgs _e)
    {
        int saveID = GameMaster.instance.GetSaveID();
        key.Clear();
        key.Append($"SaveID({saveID}).CStat.Endurance");

        PlayerPrefs.DeleteKey(key.ToString());
        key.Replace("Endurance", "Strength");
        PlayerPrefs.DeleteKey(key.ToString());
        key.Replace("Strength", "Solid");
        PlayerPrefs.DeleteKey(key.ToString());
        key.Replace("Solid", "Intelligent");
        PlayerPrefs.DeleteKey(key.ToString());
        key.Replace("Intelligent", "FullHealth");
        PlayerPrefs.DeleteKey(key.ToString());
        key.Replace("FullHealth", "Health");
        PlayerPrefs.DeleteKey(key.ToString());
        key.Replace("Health", "Abcond.Length");
        PlayerPrefs.DeleteKey(key.ToString());
    }

    private void Event_StageEnd(object _o, EventArgs _e)
    {
        SaveStats();
    }

    private void Event_BattleStageEnd(object _o , EventArgs _e)
    {
        key.Clear();
        key.Append($"SaveID({GameMaster.instance.GetSaveID()}).CStat.Abcond.Length");
        PlayerPrefs.DeleteKey(key.ToString());
        for (int i = 0; i < abcond_list.Count; i++)
        {
            int _i = i;
            key.Replace("Length", $"{_i + 1}.ID");
            PlayerPrefs.DeleteKey(key.ToString());
            key.Replace("ID", "PiledNum");
            PlayerPrefs.DeleteKey(key.ToString());
        }
        abcond_list.Clear();
    }

    public int GetCalcFullHealth()
    {
        return fullHealth_pure + endurance;
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
            //abcond_list.Clear(); //배틀 스테이지 끝날 시 클리어하는게 좋을 듯.
        }
        return;
    }

    public void SetInheriteAbCond(int _id, int _piledNum)
    {
        AbCond_Saved temp;
        temp.id = _id;
        temp.piledNum = _piledNum;
        abcond_list.Add(temp);

        key.Clear();
        key.Append($"SaveID({GameMaster.instance.GetSaveID()}).CStat.Abcond.Length");
        PlayerPrefs.SetInt(key.ToString(), abcond_list.Count);
        key.Replace("Length",$"{abcond_list.Count}.ID");
        PlayerPrefs.SetInt(key.ToString(), _id);
        key.Replace("ID", "PiledNum");
        PlayerPrefs.SetInt(key.ToString(), _piledNum);
    }

    public void SetStatChange(int _endu, int _str, int _solid, int _int)
    {
        endurance += _endu;
        strength += _str;
        solid += _solid;
        intelligent += _int;
    }

    public void SetStatChange_Init(int _endu, int _str, int _solid, int _int)
    {
        SetStatChange(_endu, _str, _solid, _int);
        health = GetCalcFullHealth();
        SaveStats();
    }
}
