using UnityEngine;

public class TrophiesFunction : MonoBehaviour
{
    [SerializeField] Vector3 defaultPosition;
    [SerializeField] Quaternion defaultRotation;

    private void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    public void OnDeselected()
    {
        transform.position = defaultPosition;
        transform.rotation = defaultRotation;
    }

}