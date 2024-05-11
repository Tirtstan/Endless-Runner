using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageVignette : MonoBehaviour
{
    private Volume volume;
    private Vignette vignette;

    // (see How to Change Post Processing through Code (C# Unity Tutorial), 2023)
    private void Awake()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
    }

    private void Start()
    {
        PlayerHealth.OnPlayerHealth += OnPlayerHit;
    }

    private void OnPlayerHit(int currentHealth)
    {
        switch (currentHealth)
        {
            default:
            case >= 3:
                vignette.intensity.value = 0;
                break;
            case 2:
                vignette.intensity.value = 0.25f;
                break;
            case <= 1:
                vignette.intensity.value = 0.45f;
                break;
        }
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerHealth -= OnPlayerHit;
    }

    #region References
    /*

    How to Change Post Processing through Code (C# Unity Tutorial). 2023. YouTube video, added by Code Monkey. [Online]. Available at: https://youtu.be/51iJqn5Px1k [Accessed 10 May 2024]

    */
    #endregion
}
