using UnityEngine;
using UnityEngine.SceneManagement;

public class MuseumSceneManager : MonoBehaviour
{
    [SerializeField] string currentSceneName;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
