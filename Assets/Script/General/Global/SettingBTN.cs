using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBTN : MonoBehaviour
{
    [SerializeField] private GameObject settingWindow;

    public void BTN_Setting()
    {
        settingWindow.SetActive(true);
    }
}
