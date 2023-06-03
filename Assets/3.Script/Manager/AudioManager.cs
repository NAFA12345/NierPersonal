using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;

    [Header("BGM")]
    [SerializeField]
    private Sound[] bgmClips;
    [SerializeField]
    private float bgmVolume;
    private AudioSource bgmPlayer;

    [Header("SFX")]
    [Space(30)]
    [SerializeField]
    private float sfxVolume;
    [Tooltip("오디오 클립 추가하고 Define 스크립트에 enum 추가해야함")]
    [SerializeField]
    private AudioClip[] sfxClips;
    // 카메라 위치(중앙)에서 재생할 효과음 (ex-플레이어 효과음)
    private AudioSource sfxPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (!Instance.Equals(this))
            {
                Destroy(gameObject);
            }
            return;
        }
        Init();
    }
    private void OnEnable()
    {
        // 이벤트 구독
        // UI~~.OnBgmVolumeSetting += SetBgmVolume;
        // UI~~.OnSfxVolumeSetting += SetSfxVolume;
    }
    private void OnDisable()
    {
        // 이벤트 해제
        // UI~~.OnBgmVolumeSetting -= SetBgmVolume;
        // UI~~.OnSfxVolumeSetting -= SetSfxVolume;
    }

    private void Init()
    {
        // BGM Player 생성 및 세팅
        GameObject bgm_obj = new GameObject("BgmPlayer");
        bgm_obj.transform.parent = transform;
        bgmPlayer = bgm_obj.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        SetBgmVolume();

        // SFX Player 생성 및 세팅
        GameObject sfx_obj = new GameObject("SfxPlayer");
        sfx_obj.transform.parent = transform;
        sfxPlayer = sfx_obj.AddComponent<AudioSource>();
        sfxPlayer.playOnAwake = false;
        SetSfxVolume();
    }

    public void PlayBgm(string name)
    {
        // 이미 재생중인 BGM이 있으면 멈춤
        StopBgm();

        // 이름 일치하는 BGM 찾아서 재생
        foreach (Sound s in bgmClips)
        {
            if (s.name.Equals(name))
            {
                bgmPlayer.clip = s.clip;
                bgmPlayer.Play();
                break;
            }
        }
    }
    private void StopBgm()
    {
        if (bgmPlayer.isPlaying)
        {
            bgmPlayer.Stop();
        }
    }
    public void SetBgmVolume(float volume = 1.0f)
    {
        bgmPlayer.volume = volume;
    }

    public void PlaySfx<T>(T sfx) where T : Enum
    {
        sfxPlayer.PlayOneShot(sfxClips[Convert.ToInt32(sfx)]);
    }
    public void SetSfxVolume(float volume = 1.0f)
    {
        sfxPlayer.volume = volume;
    }
}
