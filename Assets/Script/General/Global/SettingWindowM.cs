using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SettingWindowM : IAbleEvent
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject warningWindow;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = SoundManager.instance.masterVolume;
        warningWindow.SetActive(false);
    }

    //OnEnable이나 OnDisable 추가 시, override 타입으로 해줘야 하고, base는 꼭 실행시킬 것. [IAbleEvent]

    public void SliderChange()
    {
        SoundManager.instance.masterVolume = volumeSlider.value;
    }

    public void BTN_GameEnd()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            if (TurnManager.instance.GetIsBattleEnded())
            {
                warningWindow.SetActive(true);
            }
            else
            {
                GameMaster.instance.OnGameStop();
                Application.Quit();
            }
        }
        else
        {
            GameMaster.instance.OnGameStop();
            Application.Quit();
        }
    }

    public void BTN_ToTitle()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            if (TurnManager.instance.GetIsBattleEnded())
            {
                warningWindow.SetActive(true);
            }
            else
                LoadManager.instance.ReturnLobby(); // OnGameStop이 이 안에 있음.
        }
        else
            LoadManager.instance.ReturnLobby();
    }

    public void BTN_Close()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        gameObject.SetActive(false);
    }

    public void BTN_SubClose()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        warningWindow.SetActive(false);
    }
}
