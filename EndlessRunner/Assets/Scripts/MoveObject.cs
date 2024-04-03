using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private void Update()
    {
        transform.position -=
            LevelManager.Instance.CurrentLevelSpeed * Time.deltaTime * Vector3.forward;
    }
}
