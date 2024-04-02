using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField]
    private float rotationTime = 0.4f;

    // (see Mini Unity Tutorial - How To Rotate The Skybox In Realtime, 2017)
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationTime);
    }

    #region References
    /*

    Mini Unity Tutorial - How To Rotate The Skybox In Realtime. 2017.
    YouTube video, added by Jimmy Vegas. [Online].
    Available at: https://youtu.be/cqGq__JjhMM [Accessed 14 March 2024]

    */
    #endregion
}
