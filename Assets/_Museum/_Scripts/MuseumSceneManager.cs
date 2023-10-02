using UnityEngine;
using UnityEngine.SceneManagement;

public class MuseumSceneManager : MonoBehaviour
{
    [SerializeField] string currentSceneName;
    [SerializeField] bool isTesting = false;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (!isTesting)
            return;

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
