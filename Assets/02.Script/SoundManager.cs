using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using static GameManager;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonMono<SoundManager>
{
    [SerializeField]
    private AudioSource bgmSource;
    private Dictionary<string, AudioClip> soundEffectPool;
    private Dictionary<string, AudioClip> bgmPool;
    private ObjectProperty<EachSound> soundPool;
    [SerializeField]
    private EachSound eachSoundPrefab;

    private HashSet<string> currentFrameEfxList = new HashSet<string>();

    [Header("BGM")]
    [SerializeField]
    private List<AudioClip> stageBgms;

    [SerializeField]
    private AudioClip bossBgm;

    [SerializeField]
    private AudioClip bonusDefenseBgm;

    [SerializeField]
    private AudioClip infinityBgm;

    private void SetBgmDefaultOption()
    {
        bgmSource.loop = true;
    }


    private new void Awake()
    {
        base.Awake();
    }


    private void Start()
    {
        Subscribe();
        MakeSoundPool();
        LoadSounds();
        SetBgmDefaultOption();
    }

    private void Subscribe()
    {
        SettingData.bgmVolume.Subscribe(e => { bgmSource.volume = e; }).AddTo(this);

        GameManager.Instance.whenSceneChanged.AsObservable().Subscribe(e =>
        {
            bool sameBgm = false;
            AudioClip clip = null;

            ContentsType currentContents = GameManager.Instance.contentsType;

            switch (currentContents)
            {
                case ContentsType.NormalField:
                  //  clip = stageBgms[GameManager.Instance.CurrentStageData.Mapthema];
                    break;
                case ContentsType.FireFly:
                    clip = bonusDefenseBgm;
                    break;
                case ContentsType.Boss:
                    clip = bossBgm;
                    break;
                case ContentsType.InfiniteTower:
                    clip = infinityBgm;
                    break;
            }

            if (bgmSource.clip != clip)
            {
                bgmSource.clip = clip;
                bgmSource.Play();
            }

        }
        ).AddTo(this);
    }

    private void MakeSoundPool()
    {
        soundPool = new ObjectProperty<EachSound>(eachSoundPrefab, this.transform, 10);
    }
    private void LoadSounds()
    {

        LoadSoundEffect();
    }

    public AudioClip GetClip(string name)
    {
        if (soundEffectPool == null) return null;
        if (soundEffectPool.Count == 0) return null;
        if (soundEffectPool.ContainsKey(name) == false) return null;

        return soundEffectPool[name];
    }


    private void LoadSoundEffect()
    {
        soundEffectPool = new Dictionary<string, AudioClip>();

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Effect_sound");

        for (int i = 0; i < clips.Length; i++)
        {
            soundEffectPool.Add(clips[i].name, clips[i]);
        }
    }

    public void PlayBgm(string soundName)
    {
        //if (IsBgmMute==true) return;
        if (bgmSource == null || bgmPool == null) return;
        if (bgmPool.ContainsKey(soundName) == false) return;

        bgmSource.clip = bgmPool[soundName];
        bgmSource.Play();
    }

    public void SetBGMPitch(float value)
    {
        if (bgmSource == null) return;
        bgmSource.pitch = value;
    }

    public void PlayOnlyOneSound(string soundName)
    {                                                               //0 = false
        //중복재생 방지
        if (currentFrameEfxList.Contains(soundName)) return;
        currentFrameEfxList.Add(soundName);

        PlaySound(soundName);
    }

    public void PlaySound(string soundName, bool canCollapsed = false)
    {
        float volume = SettingData.efxVolume.Value;
        if (volume == 0f) return;

        if (soundEffectPool == null) return;

        if (soundEffectPool.ContainsKey(soundName) == false) return;

        if (soundPool == null) return;

        if (canCollapsed && currentPlayingSounds.Contains(soundName)) return;

        EachSound eachSound = soundPool.GetItem();

        if (eachSound != null)
        {
            eachSound.Initialize(soundEffectPool[soundName], volume);

            if (canCollapsed)
            {
                StartCoroutine(soundDelayRoutine(soundName));
            }
        }
    }

    private HashSet<string> currentPlayingSounds = new HashSet<string>();
    private WaitForSeconds soundDelayWs = new WaitForSeconds(0.15f);
    private IEnumerator soundDelayRoutine(string key)
    {
        currentPlayingSounds.Add(key);
        yield return soundDelayWs;
        currentPlayingSounds.Remove(key);
    }

    private void LateUpdate()
    {
        if (currentFrameEfxList.Count != 0)
        {
            currentFrameEfxList.Clear();
        }
    }

    private static string buttonSoundName = "Button";
    public void PlayButtonSound()
    {
        PlaySound(buttonSoundName);
    }

    private static string possitiveSoundKey = "Button";
    public void PlayPossitiveButtonSound()
    {
        PlaySound(possitiveSoundKey);
    }
}
