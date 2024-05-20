using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField]
    [Range(0f, 1f)]
    private float rotationTime = 0.2f;

    // (see Mini Unity Tutorial - How To Rotate The Skybox In Realtime, 2017)
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationTime);
    }

    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0f);
    }

    #region References
    /*

    Mini Unity Tutorial - How To Rotate The Skybox In Realtime. 2017.
    YouTube video, added by Jimmy Vegas. [Online].
    Available at: https://youtu.be/cqGq__JjhMM [Accessed 14 March 2024]

    */
    #endregion
}
