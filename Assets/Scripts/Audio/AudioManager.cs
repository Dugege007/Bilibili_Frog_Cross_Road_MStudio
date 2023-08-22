using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    [Header("Audio Clips")]
    public AudioClip bgmClip;
    public AudioClip jumpClip;
    public AudioClip longJunmpClip;
    public AudioClip deadClip;
    public AudioClip buttonClip;

    [Header("Audio Source")]
    public AudioSource bgmMusic;
    public AudioSource fx;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        bgmMusic.clip = bgmClip;
    }

    private void OnEnable()
    {
        PlayBGMMusic();
        EventHandler.GameOverEvent += OnGameOverEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
    }

    private void OnGameOverEvent()
    {
        fx.clip = deadClip;
        fx.Play();
    }

    /// <summary>
    /// …Ë÷√Ã¯‘æµƒ“Ù∆µ∆¨∂Œ
    /// </summary>
    /// <param name="type">0–°Ã¯£¨1¥ÛÃ¯</param>
    public void SetJumpClip(int type)
    {
        //switch (type)
        //{
        //    case 0:
        //        fx.clip = jumpClip;
        //        break;
        //    case 1:
        //        fx.clip = longJunmpClip;
        //        break;
        //}

        fx.clip = type == 0 ? jumpClip : longJunmpClip;
    }

    public void PlayJumpFX()
    {
        fx.Play();
    }

    public void PlayBGMMusic()
    {
        if (!bgmMusic.isPlaying)
        {
            bgmMusic.Play();
        }
    }

    public void ToggleAudio(bool isOn)
    {
        if (isOn)
        {
            mixer.SetFloat("MasterVolume", 0);
        }
        else
        {
            mixer.SetFloat("MasterVolume", -80);
        }
    }
}
