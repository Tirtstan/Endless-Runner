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
    private AudioHighPassFilter audioHighPassFilter;
    private AudioLowPassFilter audioLowPassFilter;

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
    private int previousSceneIndex = -1;

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
        audioHighPassFilter = GetComponent<AudioHighPassFilter>();
        audioLowPassFilter = GetComponent<AudioLowPassFilter>();
        audioHighPassFilter.enabled = false;
        audioLowPassFilter.enabled = false;

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
                StartCoroutine(FadeAllAudioOut());
                break;
        }
    }

    private void OnStartTime(int time)
    {
        if (time > 0)
            return;

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex >= 1 && sceneIndex != previousSceneIndex)
        {
            ChangeMusic(gameplayMusic);
            previousSceneIndex = sceneIndex;
        }
    }

    private void ChangeMusic(AudioClip toClip)
    {
        StartCoroutine(MusicTransition(toClip));
    }

    private IEnumerator MusicTransition(AudioClip toClip)
    {
        AudioSource activeSource = audioSource1.isPlaying ? audioSource1 : audioSource2;
        AudioSource newSource = audioSource1.isPlaying ? audioSource2 : audioSource1;

        if (activeSource.clip == toClip)
            yield break;

        newSource.clip = toClip;
        newSource.volume = 0;
        newSource.Play();

        float transition = 0;
        while (transition < transitionTime)
        {
            transition += Time.unscaledDeltaTime;
            activeSource.volume = 1 - (transition / transitionTime);
            newSource.volume = transition / transitionTime;
            yield return null;
        }

        activeSource.Stop();
    }

    private IEnumerator FadeAllAudioOut()
    {
        float transition = 0;
        while (transition < transitionTime)
        {
            transition += Time.unscaledDeltaTime;
            audioSource1.volume = 1 - (transition / transitionTime);
            audioSource2.volume = 1 - (transition / transitionTime);
            yield return null;
        }
    }

    public void PlayBoss1Music()
    {
        ChangeMusic(boss1Music);
    }

    public void PlayBoss2Music()
    {
        ChangeMusic(boss2Music);
    }

    public void SetVolume(AudioGroup audioGroup, float volume)
    {
        string audioGroupString;
        switch (audioGroup)
        {
            case AudioGroup.Master:
                audioGroupString = "MasterVolume";
                break;
            case AudioGroup.Music:
                audioGroupString = "MusicVolume";
                break;
            case AudioGroup.SoundEffects:
                audioGroupString = "SoundEffectsVolume";
                break;
            case AudioGroup.UI:
                audioGroupString = "UserInterfaceVolume";
                break;
            default:
                goto case AudioGroup.Master;
        }

        // ... (see Unity Audio: How to make a UI volume slider (the right way), 2018)
        audioMixer.SetFloat(audioGroupString, Mathf.Log10(volume) * 20);
    }

    public void ToggleHighPassFilter(bool value) => audioHighPassFilter.enabled = value;

    public void ToggleLowPassFilter(bool value) => audioLowPassFilter.enabled = value;

    private void OnDestroy()
    {
        GameManager.OnStartTime -= OnStartTime;
    }
}

public enum AudioGroup
{
    Master = 0,
    Music = 1,
    SoundEffects = 2,
    UI = 3
}

#region References
/*

Unity Audio: How to make a UI volume slider (the right way). 2018. YouTube video, added by John Leonard French. [Online]. Available at: https://youtu.be/xNHSGMKtlv4 [Accessed 02 June 2024]

*/
#endregion
