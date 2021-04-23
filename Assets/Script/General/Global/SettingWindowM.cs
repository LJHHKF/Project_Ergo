using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindowM : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = SoundManager.instance.masterVolume;
    }

    public void SliderChange()
    {
        SoundManager.instance.masterVolume = volumeSlider.value;
    }

    public void BTN_GameEnd()
    {
        GameMaster.instance.OnGameStop();
        Application.Quit();
    }

    public void BTN_ToTitle()
    {
        LoadManager.instance.ReturnLobby();
    }

    public void BTN_Close()
    {
        gameObject.SetActive(false);
    }
}
