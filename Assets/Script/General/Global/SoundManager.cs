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

    private float m_master;
    public float masterVolume
    {
        get
        {
            return m_master;
        }
        set
        {
            if(value >= 0.0 && value <= 1.0)
                m_master = value;
            ev_soundChange?.Invoke();
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
        key = "SoundSetting.MasterVolume";
        if (PlayerPrefs.HasKey(key))
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
        if (m_instance == this)
            m_instance = null;

        PlayerPrefs.SetFloat(key, masterVolume);
    }
}
