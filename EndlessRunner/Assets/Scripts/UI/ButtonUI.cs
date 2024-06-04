using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        AudioManager.Instance.PlayUISound();
    }
}
