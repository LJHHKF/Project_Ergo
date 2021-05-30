using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBTN : MonoBehaviour
{
    [SerializeField] private GameObject settingWindow;

    private void Start()
    {
        settingWindow.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            BTN_Setting();
    }

    public void BTN_Setting()
    {
        if (!settingWindow.activeSelf)
        {
            SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
            settingWindow.SetActive(true);
        }
    }

    public SettingWindowM GetSettingWindowManager()
    {
        return settingWindow.GetComponent<SettingWindowM>();
    }
}
