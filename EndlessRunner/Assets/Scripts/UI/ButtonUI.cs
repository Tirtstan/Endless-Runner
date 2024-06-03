using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private AudioClip[] clickSounds;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        audioSource.PlayOneShot(clickSounds[Random.Range(0, clickSounds.Length)]);
    }
}
