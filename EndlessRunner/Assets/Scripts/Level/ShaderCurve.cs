using UnityEngine;
using UnityEngine.SceneManagement;

public class ShaderCurve : MonoBehaviour
{
    private void Awake()
    {
        Material mat = GetComponent<Renderer>().material;
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                mat.SetFloat("_CurveStrength", 0f);
                break;
            case 1:
                mat.SetFloat("_CurveStrength", 0.0004f);
                break;
            case 2:
                mat.SetFloat("_CurveStrength", 0f);
                break;
        }
    }
}
