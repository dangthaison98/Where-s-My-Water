using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioDatas audioDatas;

    public AudioSource VFXSounds;
    public AudioSource BGMSounds;

    private void Awake()
    {
        instance = this;
    }

    public void PlayAlligatorColliSound()
    {
        PlayOneShot(audioDatas.alligatorColli);
    }
    public void PlayAlligatorRunSound()
    {
        PlayOneShot(audioDatas.alligatorRun);
    }
    public void PlayFrogJumpSound()
    {
        PlayOneShot(audioDatas.frogJump);
    }



    void PlayOneShot(AudioClip clip, float volumeScale = 0)
    {
        if (clip == null)
            return;
        if (volumeScale != 0)
            VFXSounds.PlayOneShot(clip, volumeScale: volumeScale);
        else
            VFXSounds.PlayOneShot(clip);
    }
    public void ChangeVFXVolume(float volume)
    {
        VFXSounds.volume = volume;
        PlayerPrefs.SetFloat("SFX", volume);
    }
    public void ChangeBGMVolume(float volume)
    {
        BGMSounds.volume = volume / 2;
        PlayerPrefs.SetFloat("BGM", volume / 2);
    }
}
