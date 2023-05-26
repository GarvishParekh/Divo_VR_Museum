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

    private void DeserializeDCollection (string jsonString)
    {
        JSONNode root = JSONNode.Parse(jsonString);

        data.Add(new MuseumData()
        {
            id = root["data"][0]["id"],
            name = root["data"][0]["name"],
            file = root["data"][0]["file"]
        });

        int slotsLength = root["data"][0]["slots"].Count;

        for (int i = 0; i < slotsLength; i++)
        {
            data[0].slots.Add(new SlotsData()
            {
                id = root["data"][0]["slots"][i]["id"],
                name = root["data"][0]["slots"][i]["name"]
            });

            data[0].slots[i].trophy.id = root["data"][0]["slots"][i]["trophy"]["id"];
            data[0].slots[i].trophy.name = root["data"][0]["slots"][i]["trophy"]["name"];
            data[0].slots[i].trophy.description = root["data"][0]["slots"][i]["trophy"]["description"];

            data[0].slots[i].trophy.model = root["data"][0]["slots"][i]["trophy"]["model"];
            data[0].slots[i].trophy.audio = root["data"][0]["slots"][i]["trophy"]["audio"];
            data[0].slots[i].trophy.video = root["data"][0]["slots"][i]["trophy"]["video"];
        }

        Debug.Log("All data loaded");

        for (int i = 0; i <= 25; i++)
        {
            Debug.Log(i);
            SetDataOnUI(i);
        }
    }

    private void SetDataOnUI(int count)
    {
        if (count < uiManager.uiTrophyData.Length)
        {
            uiManager.uiTrophyData[count].name.text = data[0].slots[count].trophy.name;
            uiManager.uiTrophyData[count].description.text = data[0].slots[count].trophy.description;

            uiManager.uiTrophyData[count].modelURL = data[0].slots[count].trophy.model;
            uiManager.uiTrophyData[count].audioURL = data[0].slots[count].trophy.audio;
            uiManager.uiTrophyData[count].videoURL = data[0].slots[count].trophy.video;
        }
    }
}


[System.Serializable]
public class MuseumData
{
    public int id;
    public string name;
    public string file;

    public List<SlotsData> slots = new List<SlotsData>();
}

[System.Serializable]
public class SlotsData
{
    public int id;
    public string name;

    public TrophyData trophy = new TrophyData();
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



