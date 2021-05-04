using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public event Action soulChanged;
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
        GameMaster.instance.startGame_Start += Ev_StartGame_Start;
        GameMaster.instance.initSaveData_Start += Ev_InitGame_Start;
        GameMaster.instance.stageEnd += Ev_StageEnd;
        GameMaster.instance.gameStop += Ev_GameStop;
        GameMaster.instance.gameOver += Ev_GameOver;
    }

    private void OnDisable()
    {
        GameMaster.instance.startGame_Start -= Ev_StartGame_Start;
        GameMaster.instance.initSaveData_Start -= Ev_InitGame_Start;
        GameMaster.instance.stageEnd -= Ev_StageEnd;
        GameMaster.instance.gameStop -= Ev_GameStop;
        GameMaster.instance.gameOver -= Ev_GameOver;
    }

    private void Ev_StartGame_Start(object _o, EventArgs _e)
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

    private void Ev_InitGame_Start(object _o, EventArgs _e)
    {
        soul = 0;
        SoulSave();
    }

    private void Ev_GameStop(object _o, EventArgs _e)
    {
        if (soul >= 0)
            SoulSave();
    }

    private void Ev_GameOver(object _o, EventArgs _e)
    {
        PlayerPrefs.DeleteKey(key);
    }

    private void Ev_StageEnd(object _o, EventArgs _e)
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
