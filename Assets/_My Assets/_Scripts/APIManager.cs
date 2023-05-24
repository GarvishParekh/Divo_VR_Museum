using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    [SerializeField] private string url;
    [SerializeField] private string token;
    [SerializeField] private string contentType;

    [Space]
    [SerializeField] private string jsonString = "";
    [SerializeField] private MuseumData[] data;

    private void Start() => StartCoroutine(SendRequest(url));

    IEnumerator SendRequest (string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Authorization", token);
        request.SetRequestHeader("Content-Type", contentType);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Connection Error!!!");
        }
        else
        {
            jsonString = request.downloadHandler.text;

            yield return null;
            if (jsonString != null)
            {
                data = JsonUtility.FromJson<MuseumData[]>(jsonString);
            }

        }
    }
}

[System.Serializable]
public class MuseumData
{
    public int id;
    public string name;
}
