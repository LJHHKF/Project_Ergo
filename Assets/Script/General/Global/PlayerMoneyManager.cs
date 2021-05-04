using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerMoneyManager : MonoBehaviour
{
    private static PlayerMoneyManager m_instance;
    public static PlayerMoneyManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<PlayerMoneyManager>();
            return m_instance;
        }
    }

    public int soul { get; private set; }
    public UnityEvent soulChanged;
    private int saveID;
    private string key;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        soul = -1;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        GameMaster.instance.startGame_Start.AddListener(Ev_StartGame_Start);
        GameMaster.instance.initSaveData_Start.AddListener(Ev_InitGame_Start);
        GameMaster.instance.stageEnd.AddListener(Ev_StageEnd);
        GameMaster.instance.gameStop.AddListener(Ev_GameStop);
        GameMaster.instance.gameOver.AddListener(Ev_GameOver);
    }

    private void OnDisable()
    {
        GameMaster.instance.startGame_Start.RemoveListener(Ev_StartGame_Start);
        GameMaster.instance.initSaveData_Start.RemoveListener(Ev_InitGame_Start);
        GameMaster.instance.stageEnd.RemoveListener(Ev_StageEnd);
        GameMaster.instance.gameStop.RemoveListener(Ev_GameStop);
        GameMaster.instance.gameOver.RemoveListener(Ev_GameOver);
    }

    private void Ev_StartGame_Start()
    {
        //saveID = GameMaster.instance.GetSaveID();
        //key = $"SaveID({saveID}).Soul";
        if (!PlayerPrefs.HasKey(key))
        {
            soul = 0;
            SoulSave();
        }
        else
            soul = PlayerPrefs.GetInt(key);
    }

    private void Ev_InitGame_Start()
    {
        soul = 0;
        SoulSave();
    }

    private void Ev_GameStop()
    {
        if (soul >= 0)
            SoulSave();
    }

    private void Ev_GameOver()
    {
        PlayerPrefs.DeleteKey(key);
    }

    private void Ev_StageEnd()
    {
        SoulSave();
    }

    private void SoulSave()
    {
        if(key == null)
        {
            saveID = GameMaster.instance.GetSaveID();
            key = $"SaveID({saveID}).Soul";
        }
        PlayerPrefs.SetInt(key, soul);
    }

    public void AcquiredSoul(int _value)
    {
        soul += _value;
        OnSoulChanged();
    }

    public bool UseSoul(int _value)
    {
        if(soul < _value)
        {
            return false;
        }
        else
        {
            soul -= _value;
            OnSoulChanged();
            return true;
        }
    }

    private void OnSoulChanged()
    {
        soulChanged?.Invoke();
    }
}
