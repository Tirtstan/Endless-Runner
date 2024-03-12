using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField]
    private float rotationTime = 0.4f;

    // in-text here
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationTime);
    }

    #region References
    /*

    https://youtu.be/cqGq__JjhMM

    */
    #endregion
}
