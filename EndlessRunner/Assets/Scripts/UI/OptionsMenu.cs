using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Components")]
    [Header("Dropdowns")]
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;

    [SerializeField]
    private TMP_Dropdown windowModeDropdown;

    [Header("Input Fields")]
    [SerializeField]
    private TMP_InputField userNameInputField;

    [Header("Buttons")]
    [SerializeField]
    private Button applyButton;

    [SerializeField]
    private Button resetButton;
    private CanvasGroup canvasGroup;
    private bool fadeIn;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        resolutionDropdown.onValueChanged.AddListener(OnResolutionValueChange);
        windowModeDropdown.onValueChanged.AddListener(OnWindowModeValueChange);

        applyButton.onClick.AddListener(Apply);
        resetButton.onClick.AddListener(Reset);
    }

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private void Start()
    {
        FillResolutionDropdown();
        FillWindowModeDropdown();

        LoadValues();
    }

    private void Update()
    {
        if (fadeIn)
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * 2);
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.7f);
        fadeIn = true;
        yield return new WaitForSeconds(3);
        fadeIn = false;
    }

    private void LoadValues()
    {
        resolutionDropdown.value =
            Screen.resolutions.Length - OptionsManager.Instance.ResolutionIndex - 1;
        resolutionDropdown.RefreshShownValue();

        windowModeDropdown.value = (int)OptionsManager.Instance.FullScreenMode;
        windowModeDropdown.RefreshShownValue();

        userNameInputField.text = AuthenticationService.Instance.PlayerName;
    }

    private void FillResolutionDropdown()
    {
        Resolution[] resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option =
                $"{resolutions[i].width} x {resolutions[i].height}@{resolutions[i].refreshRateRatio.value:0.##}Hz";
            options.Add(option);
        }

        options.Reverse();
        resolutionDropdown.AddOptions(options);
    }

    private void OnResolutionValueChange(int index) =>
        OptionsManager.Instance.ResolutionIndex = Screen.resolutions.Length - index - 1; // since dropdown is reversed

    private void FillWindowModeDropdown()
    {
        windowModeDropdown.ClearOptions();
        List<string> fullScreenModes =
            new()
            {
                "Exclusive Fullscreen",
                "Borderless Windowed",
                "Maximized Windowed",
                "Windowed"
            };

        windowModeDropdown.AddOptions(fullScreenModes);
    }

    private void OnWindowModeValueChange(int index) =>
        OptionsManager.Instance.FullScreenMode = (FullScreenMode)index;

    private async void Apply()
    {
        OptionsManager.Instance.Apply();

        try
        {
            if (
                string.IsNullOrEmpty(userNameInputField.text)
                || string.IsNullOrWhiteSpace(userNameInputField.text)
            )
                return;

            if (userNameInputField.text == AuthenticationService.Instance.PlayerName)
                return;

            await AuthenticationService.Instance.UpdatePlayerNameAsync(userNameInputField.text);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Failed to update player name!: {e.Message}");
        }

        LoadValues();
    }

    private void Reset()
    {
        OptionsManager.Instance.ResetToDefault();
        Apply();
    }

    private void OnDisable()
    {
        fadeIn = false;
        canvasGroup.alpha = 0;
    }
}
