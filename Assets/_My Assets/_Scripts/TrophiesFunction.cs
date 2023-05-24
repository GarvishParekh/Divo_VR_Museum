using UnityEngine;

public class TrophiesFunction : MonoBehaviour
{
    [SerializeField] Vector3 defaultPosition;

    private void Start()
        => defaultPosition = transform.position;

    public void OnDeselected() => transform.position = defaultPosition;
}