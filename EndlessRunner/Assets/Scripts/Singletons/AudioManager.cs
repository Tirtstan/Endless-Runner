using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public static event Action OnMusicChanged;
    private AudioHighPassFilter[] audioHighPassFilters;
    private AudioLowPassFilter[] audioLowPassFilters;

    [Header("Components")]
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private AudioSource mainMenuMusicSource;

    [SerializeField]
    private AudioSource[] musicSources;

    [SerializeField]
    private AudioSource otherSource;

    [Header("Music")]
    [SerializeField]
    private AudioClip mainMenuMusic;

    [SerializeField]
    private AudioClip gameplayMusic;

    [SerializeField]
    private AudioClip boss1Music;

    [SerializeField]
    private AudioClip boss2Music;

    [Header("Other")]
    [SerializeField]
    private AudioClip pickupSound;

    [SerializeField]
    private AudioClip[] buttonClickSounds;

    [Header("Configs")]
    [SerializeField]
    [Range(0, 2)]
    private float transitionTime = 1.25f;
    private AudioClip currentGameplayClip;
    private int toggle;
    private double nextStartTime;
    private Coroutine gameplayMusicCoroutine;

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

        audioHighPassFilters = GetComponentsInChildren<AudioHighPassFilter>();
        audioLowPassFilters = GetComponentsInChildren<AudioLowPassFilter>();
        ToggleHighPassFilter(false);
        ToggleLowPassFilter(false);

        mainMenuMusicSource.clip = mainMenuMusic;
        currentGameplayClip = gameplayMusic;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            default:
            case 0:
                if (gameplayMusicCoroutine != null)
                {
                    StopCoroutine(gameplayMusicCoroutine);
                    gameplayMusicCoroutine = null;
                }
                StartCoroutine(FadeInMainMenuMusicOnly());
                break;
            case >= 1:
                ResetMainMenuMusic();
                ChangeCurrentMusic(GameplayMusic.Gameplay);
                gameplayMusicCoroutine ??= StartCoroutine(MusicTransition());
                break;
        }

        ToggleHighPassFilter(false);
    }

    private void Start()
    {
        PlayerHealth.OnPlayerHealthChanged += OnPlayerHealthChanged;

        EventManager.OnPickup1 += PlayPickupSound;
        EventManager.OnPickup2 += PlayPickupSound;
        EventManager.OnPickup3 += PlayPickupSound;
    }

    private IEnumerator MusicTransition()
    {
        nextStartTime = AudioSettings.dspTime;
        while (true)
        {
            // Leonard French (2018) demonstrates...
            if (AudioSettings.dspTime > nextStartTime - 1.0)
            {
                OnMusicChanged?.Invoke();
                AudioClip clipToPlay = currentGameplayClip;

                musicSources[toggle].clip = clipToPlay;
                musicSources[toggle].PlayScheduled(nextStartTime + 0.5f);

                double duration = (double)clipToPlay.samples / clipToPlay.frequency;
                nextStartTime += duration;

                toggle = 1 - toggle;
            }

            yield return null;
        }
    }

    public void ChangeCurrentMusic(GameplayMusic music)
    {
        switch (music)
        {
            case GameplayMusic.Gameplay:
                currentGameplayClip = gameplayMusic;
                break;
            case GameplayMusic.Boss1:
                currentGameplayClip = boss1Music;
                break;
            case GameplayMusic.Boss2:
                currentGameplayClip = boss2Music;
                break;
        }
    }

    private void OnPlayerHealthChanged(int health)
    {
        if (health > 0)
            return;

        StartCoroutine(FadeAllMusicOut());
    }

    private IEnumerator FadeAllMusicOut()
    {
        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainMenuMusicSource.volume = 1 - (elapsedTime / transitionTime);
            for (int i = 0; i < musicSources.Length; i++)
            {
                musicSources[i].volume = 1 - (elapsedTime / transitionTime);
            }
            yield return null;
        }

        ResetAllSources();
    }

    private IEnumerator FadeInMainMenuMusicOnly()
    {
        mainMenuMusicSource.Play();
        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            mainMenuMusicSource.volume = elapsedTime / transitionTime;
            for (int i = 0; i < musicSources.Length; i++)
            {
                musicSources[i].volume = 1 - (elapsedTime / transitionTime);
            }
            yield return null;
        }

        ResetAllGameplayMusic();
    }

    private void ResetAllSources()
    {
        ResetMainMenuMusic();
        ResetAllGameplayMusic();
    }

    private void ResetMainMenuMusic()
    {
        mainMenuMusicSource.Stop();
        mainMenuMusicSource.volume = 1;
    }

    private void ResetAllGameplayMusic()
    {
        foreach (var source in musicSources)
        {
            source.Stop();
            source.volume = 1;
        }
    }

    private void PlayPickupSound()
    {
        otherSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound Effects")[0];
        otherSource.priority = 64;
        otherSource.PlayOneShot(pickupSound);
    }

    public void PlayUISound()
    {
        otherSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("User Interface")[0];
        otherSource.priority = 192;
        otherSource.PlayOneShot(buttonClickSounds[UnityEngine.Random.Range(0, buttonClickSounds.Length)]);
    }

    public void PlayFadeInClip(AudioSource audioSource, float duration)
    {
        StartCoroutine(FadeInClipCouroutine(audioSource, duration));
    }

    // Leonard French (2018) demonstrates...
    private IEnumerator FadeInClipCouroutine(AudioSource audioSource, float duration)
    {
        audioSource.Play();
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, 1, elapsedTime / duration);
            yield return null;
        }
    }

    public void SetVolume(AudioGroup audioGroup, float volume)
    {
        string audioGroupString;
        switch (audioGroup)
        {
            default:
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
        }

        // (see Unity Audio: How to make a UI volume slider (the right way), 2018)
        audioMixer.SetFloat(audioGroupString, Mathf.Log10(volume) * 20);
    }

    public void ToggleHighPassFilter(bool value)
    {
        foreach (var filter in audioHighPassFilters)
            filter.enabled = value;
    }

    public void ToggleLowPassFilter(bool value)
    {
        foreach (var filter in audioLowPassFilters)
            filter.enabled = value;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerHealth.OnPlayerHealthChanged -= OnPlayerHealthChanged;

        EventManager.OnPickup1 -= PlayPickupSound;
        EventManager.OnPickup2 -= PlayPickupSound;
        EventManager.OnPickup3 -= PlayPickupSound;
    }
}

public enum AudioGroup
{
    Master = 0,
    Music = 1,
    SoundEffects = 2,
    UI = 3
}

public enum GameplayMusic
{
    Gameplay = 0,
    Boss1 = 1,
    Boss2 = 2
}

#region References
/*

Leonard French, J. 2018. How to Queue Audio Clips in Unity (the Ultimate Guide to PlayScheduled). John Leonard French, 17 August 2018. [Blog] Available from: https://johnleonardfrench.com/ultimate-guide-to-playscheduled-in-unity/#playscheduled_uses [Accessed 04 June 2024].

Unity Audio: How to make a UI volume slider (the right way). 2018. YouTube video, added by John Leonard French. [Online]. Available at: https://youtu.be/xNHSGMKtlv4 [Accessed 02 June 2024]

*/
#endregion
