using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioDatas audioDatas;

    public AudioSource VFXSounds;
    public AudioSource BGMSounds;

    public Slider sfxSlider;
    public Slider bgmSlider;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        BGMSounds = BGM.instance.GetComponent<AudioSource>();

        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1);
        VFXSounds.volume = PlayerPrefs.GetFloat("SFX", 1);
        bgmSlider.value = PlayerPrefs.GetFloat("BGM", 1) * 2;
        BGMSounds.volume = PlayerPrefs.GetFloat("BGM", 1);
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
