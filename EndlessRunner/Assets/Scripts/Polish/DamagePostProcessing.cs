using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamagePostProcessing : MonoBehaviour
{
    private Volume volume;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;

    // (see How to Change Post Processing through Code (C# Unity Tutorial), 2023)
    private void Awake()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromaticAberration);
    }

    private void Start()
    {
        PlayerHealth.OnPlayerHealthChanged += OnPlayerHit;
    }

    private void OnPlayerHit(int currentHealth)
    {
        switch (currentHealth)
        {
            default:
            case >= 3:
                vignette.intensity.value = 0;
                chromaticAberration.intensity.value = 0;
                break;
            case 2:
                vignette.intensity.value = 0.3f;
                chromaticAberration.intensity.value = 0.15f;
                break;
            case <= 1:
                vignette.intensity.value = 0.45f;
                chromaticAberration.intensity.value = 0.45f;
                break;
        }
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerHealthChanged -= OnPlayerHit;
    }

    #region References
    /*

    How to Change Post Processing through Code (C# Unity Tutorial). 2023. YouTube video, added by Code Monkey. [Online]. Available at: https://youtu.be/51iJqn5Px1k [Accessed 10 May 2024]

    */
    #endregion
}
