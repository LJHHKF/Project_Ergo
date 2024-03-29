using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBTN : MonoBehaviour
{
    [SerializeField] private GameObject activeTarget;

    private void Start()
    {
        activeTarget.SetActive(false);
    }

    public void ActiveBTNClick()
    {
        SoundEfManager.instance.SetSoundEffect(mySoundEffect.SoundEf.ui_touch);
        if (activeTarget.activeSelf)
        {
            activeTarget.SetActive(false);
        }
        else
        {
            activeTarget.SetActive(true);
        }
    }

}
