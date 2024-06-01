using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance { get; private set; }
    public int ResolutionIndex { get; set; }
    public FullScreenMode FullScreenMode { get; set; }
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

        Save();
    }

    private void Save()
    {
        IsFirstTime = false;

        PlayerPrefs.SetInt(nameof(IsFirstTime), IsFirstTime ? 1 : 0);

        PlayerPrefs.SetInt(nameof(ResolutionIndex), ResolutionIndex);
        PlayerPrefs.SetInt(nameof(FullScreenMode), (int)FullScreenMode);

        PlayerPrefs.Save();
    }

    private void Load()
    {
        ResolutionIndex = PlayerPrefs.GetInt(nameof(ResolutionIndex));
        FullScreenMode = (FullScreenMode)PlayerPrefs.GetInt(nameof(FullScreenMode));
    }
}
