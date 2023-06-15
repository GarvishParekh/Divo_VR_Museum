using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MuseumImageLoader : MonoBehaviour
{
    APIManager_JU apiManager;

    [SerializeField] private List<Material> materialList = new List<Material>();

    [SerializeField] private string imageURL = "";
    [SerializeField] private Texture2D myTexture;
    [SerializeField] private Texture2D loadedTexture;

    [Space]
    [Header("Time line components")]
    [SerializeField] private string timeLineURL;
    [SerializeField] private Material timeLineMat;

    private void Start()
    {
        apiManager = APIManager_JU.instance;
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
        int listLenght = materialList.Count;
        
        for (int i = 0; i < listLenght; i++)
        {
            string currentToken = GetToken(i);
            for (int j = 0; j < listLenght; j++)
            {
                if (currentToken == apiManager.museumDataList.data[0].slots.image[j].token)
                {
                    string imageURL = apiManager.museumDataList.data[0].slots.image[j].s3_value;
                    StartCoroutine(GetTexture(imageURL, j));
                }
            }
        }
        GetTimeLineURL();
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
            myTexture = ((DownloadHandlerTexture)imageRquest.downloadHandler).texture;

            loadedTexture = new Texture2D(myTexture.width, myTexture.height, myTexture.format, true);
            loadedTexture.anisoLevel = 16;
            loadedTexture.LoadImage(imageRquest.downloadHandler.data);

            materialList[matIndex].SetTexture("_MainTex", loadedTexture);
        }
    }

    private void GetTimeLineURL ()
    {
        timeLineURL = apiManager.museumDataList.data[0].slots.image[27].s3_value;
        StartCoroutine(GetTexture(timeLineURL, 27));
    }

    private string GetToken (int _index)
    {
        return TokenInformation.instance.imageToken[_index];
    }
}
