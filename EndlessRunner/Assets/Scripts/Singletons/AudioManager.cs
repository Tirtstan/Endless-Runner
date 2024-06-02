using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource audioSource1;
    private AudioSource audioSource2;

    [Header("Components")]
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private AudioClip mainMenuMusic;

    [SerializeField]
    private AudioClip gameplayMusic;

    [SerializeField]
    private AudioClip boss1Music;

    [SerializeField]
    private AudioClip boss2Music;

    [Header("Configs")]
    [SerializeField]
    [Range(0.1f, 2)]
    private float transitionTime = 1.25f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource1 = GetComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();

        audioSource2.outputAudioMixerGroup = audioSource1.outputAudioMixerGroup;
        audioSource2.priority = audioSource1.priority;

        audioSource1.loop = true;
        audioSource2.loop = true;
        audioSource1.playOnAwake = false;
        audioSource2.playOnAwake = false;
    }

    private void Start()
    {
        ChangeMusic(mainMenuMusic);
        GameManager.OnStartTime += OnStartTime;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            default:
            case 0:
                ChangeMusic(mainMenuMusic);
                break;
            case >= 1:
                ChangeMusic(gameplayMusic);
                break;
        }
    }

    private void OnStartTime(int time)
    {
        if (time > 0)
            return;
    }

    private void ChangeMusic(AudioClip toClip)
    {
        StartCoroutine(MusicTransition(toClip));
    }

    private IEnumerator MusicTransition(AudioClip toClip)
    {
        AudioSource activeSource = audioSource1.isPlaying ? audioSource1 : audioSource2;
        AudioSource newSource = audioSource1.isPlaying ? audioSource2 : audioSource1;

        newSource.clip = toClip;
        newSource.volume = 0;
        newSource.Play();

        float transition = 0;
        while (transition < transitionTime)
        {
            transition += Time.deltaTime;
            activeSource.volume = 1 - (transition / transitionTime);
            newSource.volume = transition / transitionTime;
            yield return null;
        }

        activeSource.Stop();
    }

    public void PlayBoss1Music()
    {
        ChangeMusic(boss1Music);
    }

    public void PlayBoss2Music()
    {
        ChangeMusic(boss2Music);
    }

    public void SetVolume(AudioGroups audioGroup, float volume)
    {
        string audioGroupString;
        switch (audioGroup)
        {
            case AudioGroups.Master:
                audioGroupString = "MasterVolume";
                break;
            case AudioGroups.Music:
                audioGroupString = "MusicVolume";
                break;
            case AudioGroups.SoundEffects:
                audioGroupString = "SoundEffectsVolume";
                break;
            case AudioGroups.UI:
                audioGroupString = "UserInterfaceVolume";
                break;
            default:
                goto case AudioGroups.Master;
        }

        audioMixer.SetFloat(audioGroupString, volume);
    }

    private void OnDestroy()
    {
        GameManager.OnStartTime -= OnStartTime;
    }
}

public enum AudioGroups
{
    Master = 0,
    Music = 1,
    SoundEffects = 2,
    UI = 3
}
