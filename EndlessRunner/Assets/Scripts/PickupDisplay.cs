using UnityEngine;
using TMPro;

public class PickupDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject pickupDisplay;

    [SerializeField]
    private TextMeshProUGUI timeLeft;

    private void OnEnable()
    {
        PickupManager.OnPickupTime += OnPickupTime;
    }

    private void OnPickupTime(float time)
    {
        pickupDisplay.SetActive(true);
        timeLeft.text = time.ToString();
        if (time <= 0)
            pickupDisplay.SetActive(false);
    }

    private void OnDisable()
    {
        PickupManager.OnPickupTime -= OnPickupTime;
    }
}
