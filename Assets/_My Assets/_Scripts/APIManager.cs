using SimpleJSON;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ApiManager : MonoBehaviour
{
    public UiManager uiManager;

    [SerializeField] private string url;
    [SerializeField] private string token;
    [SerializeField] private string contentType;

    [SerializeField] private string jsonString;
    [SerializeField] private List<MuseumData> data = new List<MuseumData>();

    private void Start()
    {
        StartCoroutine(SendRequest(url));
    }

    private IEnumerator SendRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Authorization", token);
        request.SetRequestHeader("Content-Type", contentType);
        //request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Connection Error!!!!");
        }
        else
        {
            jsonString = request.downloadHandler.text;
            //data = JsonUtility.FromJson<MuseumData[]>(jsonString);

            DeserializeDCollection(jsonString);
        }
    }

    private void DeserializeDCollection(string jsonString)
    {
        JSONNode root = JSONNode.Parse(jsonString);

        data.Add(new MuseumData()
        {
            id = root["data"][0]["id"],
            name = root["data"][0]["name"],
            file = root["data"][0]["file"],

            slots = new SlotsData(),
        });

        int trophyCount = root["data"][0]["slots"]["trophy"].Count;
        Debug.Log("T" + trophyCount);
        for (int i = 0; i < trophyCount; i++)
        {
            data[0].slots.trophy.Add(new TrophyData()
            {
                id = root["data"][0]["slots"]["trophy"][i]["id"],
                name = root["data"][0]["slots"]["trophy"][i]["name"],
                description = root["data"][0]["slots"]["trophy"][i]["description"],

                model = root["data"][0]["slots"]["trophy"][i]["model"],
                audio = root["data"][0]["slots"]["trophy"][i]["audio"],
                video = root["data"][0]["slots"]["trophy"][i]["video"]
            });
        }

        for (int i = 0; i < trophyCount; i++)
        {
            SetDataOnUI(i);
        }
    }

    private void SetDataOnUI(int count)
    {
        if (count < uiManager.uiTrophyData.Length)
        {
            uiManager.uiTrophyData[count].name.text = data[0].slots.trophy[count].name;
            uiManager.uiTrophyData[count].description.text = data[0].slots.trophy[count].description;

            uiManager.uiTrophyData[count].modelURL = data[0].slots.trophy[count].model;
            uiManager.uiTrophyData[count].audioURL = data[0].slots.trophy[count].audio;
            uiManager.uiTrophyData[count].videoURL = data[0].slots.trophy[count].video;
        }
    }
}


[System.Serializable]
public class MuseumData
{
    public int id;
    public string name;
    public string file;

    public SlotsData slots;
}

[System.Serializable]
public class SlotsData
{
    public List<TrophyData> trophy = new List<TrophyData>();
}

[System.Serializable]
public class TrophyData
{
    public int id;
    public string name;
    public string description;

    [Space]
    [Header ("Trophy data")]
    public string model;
    public string audio;
    public string video;
}



