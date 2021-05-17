using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    private static SoundManager m_instance;
    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<SoundManager>();
            return m_instance;
        }
    }

    //private float m_master;
    public float masterVolume
    {
        get
        {
            return masterVolume;
        }
        set
        {
            if(value >= 0.0 && value <= 1.0)
                masterVolume = value;
            if (ev_soundChange != null)
                ev_soundChange.Invoke();
        }
    }

    private string key;
    public event Action ev_soundChange;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }    
    }

    // Start is called before the first frame update
    void Start()
    {
        key = "SoundSetting.MasterVolume";
        if(PlayerPrefs.HasKey(key))
        {
            masterVolume = PlayerPrefs.GetFloat(key);
        }
        else
        {
            masterVolume = 1.0f;
            PlayerPrefs.SetFloat(key, masterVolume);
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(key, masterVolume);
    }
}
