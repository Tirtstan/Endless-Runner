using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance { get; private set; }
    public int ResolutionIndex { get; set; }
    public FullScreenMode FullScreenMode { get; set; }
    public float MasterVolume { get; set; } = 1;
    public float MusicVolume { get; set; } = 1;
    public float SoundEffectsVolume { get; set; } = 1;
    public float UserInterfaceVolume { get; set; } = 1;
    private bool IsFirstTime;

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

        QualitySettings.vSyncCount = 1;
    }

    private void Start()
    {
        IsFirstTime = PlayerPrefs.GetInt(nameof(IsFirstTime), 1) == 1;

        if (IsFirstTime)
            ResetToDefault();
        else
            Load();

        Apply();
    }

    public int GetPlayerResolutionIndex()
    {
        Resolution[] resolutions = Screen.resolutions;
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < resolutions.Length; i++)
        {
            int result = resolutions[i]
                .refreshRateRatio.value.CompareTo(currentResolution.refreshRateRatio.value);
            if (
                resolutions[i].width == currentResolution.width
                && resolutions[i].height == currentResolution.height
                && result == 0
            )
            {
                return i;
            }
        }

        return 0;
    }

    public void ResetToDefault()
    {
        ResolutionIndex = GetPlayerResolutionIndex();
        FullScreenMode = FullScreenMode.FullScreenWindow;
        MasterVolume = 0.5f;
        MusicVolume = 1;
        SoundEffectsVolume = 1;
        UserInterfaceVolume = 1;
    }

    public void Apply()
    {
        Resolution resolution = Screen.resolutions[ResolutionIndex];
        Screen.SetResolution(
            resolution.width,
            resolution.height,
            FullScreenMode,
            resolution.refreshRateRatio
        );

        AudioManager.Instance.SetVolume(AudioGroup.Master, MasterVolume);
        AudioManager.Instance.SetVolume(AudioGroup.Music, MusicVolume);
        AudioManager.Instance.SetVolume(AudioGroup.SoundEffects, SoundEffectsVolume);
        AudioManager.Instance.SetVolume(AudioGroup.UI, UserInterfaceVolume);

        Save();
    }

    private void Save()
    {
        IsFirstTime = false;

        PlayerPrefs.SetInt(nameof(IsFirstTime), IsFirstTime ? 1 : 0);

        PlayerPrefs.SetInt(nameof(ResolutionIndex), ResolutionIndex);
        PlayerPrefs.SetInt(nameof(FullScreenMode), (int)FullScreenMode);

        PlayerPrefs.SetFloat(nameof(MasterVolume), MasterVolume);
        PlayerPrefs.SetFloat(nameof(MusicVolume), MusicVolume);
        PlayerPrefs.SetFloat(nameof(SoundEffectsVolume), SoundEffectsVolume);
        PlayerPrefs.SetFloat(nameof(UserInterfaceVolume), UserInterfaceVolume);

        PlayerPrefs.Save();
    }

    private void Load()
    {
        ResolutionIndex = PlayerPrefs.GetInt(nameof(ResolutionIndex));
        FullScreenMode = (FullScreenMode)PlayerPrefs.GetInt(nameof(FullScreenMode));

        MasterVolume = PlayerPrefs.GetFloat(nameof(MasterVolume));
        MusicVolume = PlayerPrefs.GetFloat(nameof(MusicVolume));
        SoundEffectsVolume = PlayerPrefs.GetFloat(nameof(SoundEffectsVolume));
        UserInterfaceVolume = PlayerPrefs.GetFloat(nameof(UserInterfaceVolume));
    }
}
