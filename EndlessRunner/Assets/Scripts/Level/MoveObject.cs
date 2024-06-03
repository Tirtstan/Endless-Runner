using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private void Update()
    {
        transform.position -= LevelSpeedManager.Instance.CurrentLevelSpeed * Time.deltaTime * Vector3.forward;
    }
}
