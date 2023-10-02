using System;
using UnityEngine;

public class HandsState : MonoBehaviour
{
    public static Action HandsInPillar;
    public static Action HandsOutsidePillar;

    [SerializeField] string pillarTag;

    private void OnTriggerEnter(Collider info)
    {
        if (info.CompareTag (pillarTag))
            HandsInPillar?.Invoke();
    }

    private void OnTriggerExit(Collider info)
    {
        if (info.CompareTag(pillarTag))
            HandsOutsidePillar?.Invoke();
    }
}