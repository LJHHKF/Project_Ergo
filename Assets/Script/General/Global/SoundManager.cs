using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float masterVolume { get; set; }

    private string key;

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
