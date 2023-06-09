using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class APIManager_JU : MonoBehaviour
{
    public static APIManager_JU instance;
    public static Action APIRecived;
    public enum UrlType
    {
        Testing,
        Development,
        Production
    }
    [SerializeField] private UrlType urlType;

    private string URL;
    [SerializeField] private string token;
    [SerializeField] private string contentType;
    
    [Space]
    [SerializeField] string jsonString;

    [Header("Data collection")]
    public MuseumList museumDataList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        URL = GetUrlType();
        StartCoroutine(GetRequest(URL));
    }

    IEnumerator GetRequest (string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Authorization", token);
        request.SetRequestHeader("Content-Type", contentType);

        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.Log($"Error: {request.error}");
        }
        else
        {
            jsonString = request.downloadHandler.text;
            museumDataList = JsonUtility.FromJson<MuseumList>(jsonString);

            // event for reciving and loading api
            APIRecived?.Invoke();
        }
    }

    private string GetUrlType()
    {
        if (urlType == UrlType.Testing)
            return URLs.testingURL;
        else if (urlType == UrlType.Development)
            return URLs.developmentURL;
        else if (urlType == UrlType.Production)
            return URLs.productionURL;
        else
            return null;
    }
}

[System.Serializable]
public class MuseumList
{
    public List<MuseumData> data;
}

[System.Serializable]
public class MuseumData
{
    public int id;
    public string name;
    public string file;
    public Slots slots;
}

[System.Serializable]
public class SlotsImages
{
    public int id;
    public string name;
    public string token;
    public string s3_value;
    public string type;
}

[System.Serializable]
public class Slots
{
    public List<SlotsImages> image = new List<SlotsImages>();
    public List<SlotsAudio> audio = new List<SlotsAudio>();
    public List<SlotsTrophy> trophy = new List<SlotsTrophy>();
}

[System.Serializable]
public class SlotsTrophy
{
    public int id;
    public string name;
    public string token;
    public string s3_value;
    public string type;
    public string description;
    public string model;
    public string audio;
    public string video;
}

[System.Serializable]
public class SlotsAudio
{
    public int id;
    public string s3_value;
}


