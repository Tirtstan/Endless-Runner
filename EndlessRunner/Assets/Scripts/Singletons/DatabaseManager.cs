using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

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

        Initialize();
    }

    private async void Initialize()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services initialized successfully!");

            Login();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to initialize Unity Services!: {e.Message}");
        }
    }

    private async void Login()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log(
                $"User signed in anonymously as {AuthenticationService.Instance.PlayerName} with ID: {AuthenticationService.Instance.PlayerId}"
            );
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to sign in anonymously!: {e.Message}");
        }
    }
}
