using UnityEngine;
using TMPro;

public class StartTimeUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TextMeshProUGUI timeText;

    private void Start()
    {
        GameManager.OnStartTime += UpdateTimeText;
    }

    private void UpdateTimeText(int time)
    {
        panel.SetActive(true);
        timeText.text = time.ToString();

        if (time == 0)
            panel.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.OnStartTime -= UpdateTimeText;
    }
}
