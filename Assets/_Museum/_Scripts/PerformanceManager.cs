using TMPro;
using UnityEngine;
using System.Collections;

public class PerformanceManager : MonoBehaviour
{
    [SerializeField] TMP_Text T_fpsCounter;
    float count = 0;

    private void Start()
    {
        Application.targetFrameRate = 120;

        StartCoroutine(nameof(FPSCounter));
    }

    private IEnumerator FPSCounter()
    {
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            T_fpsCounter.text = $"{count.ToString("0")}FPS";
            yield return new WaitForSeconds(0.1f);
        }
    }
}
