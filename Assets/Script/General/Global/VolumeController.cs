using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeController : MonoBehaviour
{
    private AudioSource m_audio;
    // Start is called before the first frame update
    void Start()
    {
        m_audio = gameObject.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        VolumeChange();
        SoundManager.instance.ev_soundChange += VolumeChange;
    }

    private void nDisable()
    {
        SoundManager.instance.ev_soundChange -= VolumeChange;
    }

    private void VolumeChange()
    {
        m_audio.volume = SoundManager.instance.masterVolume;
    }
}
