using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Camera mainCamera;
    private float shakeDuration;
    private float shakeAmount;
    private float decreaseFactor;
    private Vector3 originalPos;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        originalPos = mainCamera.transform.position;
    }

    // ftvs (2016) demonstrates how...
    private void Update()
    {
        if (shakeDuration > 0)
        {
            mainCamera.transform.position = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.unscaledDeltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            mainCamera.transform.position = originalPos;
        }
    }

    public void Shake(float duration, float amount = 0.05f, float decreaseFactor = 1f)
    {
        shakeDuration = duration;
        shakeAmount = amount;
        this.decreaseFactor = decreaseFactor;
    }

    #region References
    /*

    ftvs. (2016). CameraShake.cs (Version 5).  [Source Code]. https://gist.github.com/ftvs/5822103 (Accessed 10 May 2024).

    */
    #endregion
}
