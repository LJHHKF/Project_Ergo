using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

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

    [Header("Init Stat Setting")]
    [SerializeField] private int init_FullHealth = 100;
    //public int init_StartingHealth = 100;
    [SerializeField] private int init_Endurance = 1;
    [SerializeField] private int init_Strength = 1;
    [SerializeField] private int init_Solid = 1;
    [SerializeField] private int init_Intelligent = 1;

    public static int fullHealth_pure { get; set; }
    //public int startingHealth { get; set; }
    public static int health { get; private set; }
    public static int endurance { get; set; }
    public static int strength { get; set; }
    public static int solid { get; set; }
    public static int intelligent { get; set; }



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
        GameMaster.initSaveData_Awake += () => InitStatSetting();
        GameMaster.startGame_Awake += () => StartStatSetting();
        GameMaster.stageEnd += () => SaveStats();
    }

    private void InitStatSetting()
    {
        endurance = init_Endurance;
        strength = init_Strength;
        solid = init_Solid;
        intelligent = init_Intelligent;
        fullHealth_pure = init_FullHealth;
        health = CalcResultFullHealth();

        SaveStats();
    }

    private int CalcResultFullHealth()
    {
        return fullHealth_pure + endurance;
        //(endurance * 1);
    }

    private void StartStatSetting()
    {
        int saveID = GameMaster.GetSaveID();
        key.Clear();
        key.Append("SaveID(");
        key.Append(saveID.ToString());
        key.Append(").CStat.Endurance");

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

    private void SaveStats()
    {
        int saveID = GameMaster.GetSaveID();
        key.Clear();
        key.Append("SaveID(");
        key.Append(saveID.ToString());
        key.Append(").CStat.Endurance");

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

    public static void HealthPointUpdate(int value)
    {
        CStatManager.health = value;
        if(health <= 0)
        {
            GameMaster.OnGameOver();
        }
    }
}
