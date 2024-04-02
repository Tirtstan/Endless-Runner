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
        PickupManager.OnJetpackTime += OnJetpackTime;
    }

    private void OnJetpackTime(float time)
    {
        pickupDisplay.SetActive(true);
        timeLeft.text = time.ToString();
        if (time <= 0)
        {
            pickupDisplay.SetActive(false);
        }
    }

    private void OnDisable()
    {
        PickupManager.OnJetpackTime -= OnJetpackTime;
    }
}
