using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MuseumImageLoader : MonoBehaviour
{
    APIManager_JU apiManager;

    [SerializeField] private List<Material> materialList = new List<Material>();

    [SerializeField] private string imageURL = "";
    [SerializeField] private Texture myTexture;

    [Space]
    [Header("Time line components")]
    [SerializeField] private string timeLineURL;
    [SerializeField] private Material timeLineMat;

    private void Start()
    {
        apiManager = APIManager_JU.instance;
        GetTimeLineURL();
    }

    private void OnEnable()
    {
        APIManager_JU.APIRecived += OnRecivingApi;
    }

    private void OnDisable()
    {
        APIManager_JU.APIRecived -= OnRecivingApi;
    }

    private void OnRecivingApi()
    {
        int listLenght = apiManager.museumDataList.data[0].slots.image.Count;

        for (int i = 0; i < materialList.Count; i++)
        {
            string imageURL = apiManager.museumDataList.data[0].slots.image[i].s3_value;
            StartCoroutine(GetTexture(imageURL, i));
        }
    }

    IEnumerator GetTexture(string _imageURL, int matIndex)
    {
        UnityWebRequest imageRquest = UnityWebRequestTexture.GetTexture(_imageURL);
        yield return imageRquest.SendWebRequest();
        if (imageRquest.error != null)
        {
            Debug.LogError(imageRquest.error);
        }
        else
        {
            Debug.Log("SETTING IMAGE");
            myTexture = ((DownloadHandlerTexture)imageRquest.downloadHandler).texture;
            materialList[matIndex].SetTexture("_MainTex", myTexture);
        }
    }

    private void GetTimeLineURL ()
    {
        timeLineURL = apiManager.museumDataList.data[0].slots.image[27].s3_value;
        StartCoroutine(GetTexture(timeLineURL, 28));
    }
}
