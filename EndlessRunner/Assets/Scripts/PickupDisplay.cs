using UnityEngine;
using TMPro;

public class PickupDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject pickupDisplay;

    [SerializeField]
    private TextMeshProUGUI pickupText;

    [SerializeField]
    private TextMeshProUGUI timeLeft;

    [SerializeField]
    private Color[] pickupColors;

    private void OnEnable()
    {
        PickupManager.OnPickupTime += OnPickupTime;
    }

    private void OnPickupTime(ItemPickup.Type type, float time)
    {
        pickupDisplay.SetActive(true);

        switch (type)
        {
            default:
            case ItemPickup.Type.Jetpack:
                pickupText.color = pickupColors[0];
                pickupText.text = "Jetpack";
                break;
            case ItemPickup.Type.LowGravity:
                pickupText.color = pickupColors[1];
                pickupText.text = "Low Gravity";
                break;
            case ItemPickup.Type.SpeedBoots:
                break;
        }

        timeLeft.text = time.ToString();
        if (time <= 0)
            pickupDisplay.SetActive(false);
    }

    private void OnDisable()
    {
        PickupManager.OnPickupTime -= OnPickupTime;
    }
}
