using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MuseumImageLoader : MonoBehaviour
{
    APIManager_JU apiManager;

    [SerializeField] private List<Material> materialList = new List<Material>();
    [SerializeField] private List<Material> timeLineMaterialList = new List<Material>();

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
        int timeLineMatLength = timeLineMaterialList.Count;
        int apiImageLenght = apiManager.museumDataList.data[0].slots.image.Count;


        for (int i = 0; i < listLenght; i++)
        {
            string currentToken = GetToken(i);
            
            // set normal material
            for (int j = 0; j < apiImageLenght; j++)
            {
                if (currentToken == apiManager.museumDataList.data[0].slots.image[j].token)
                {
                    string imageURL = apiManager.museumDataList.data[0].slots.image[j].s3_value;
                    StartCoroutine(GetTexture(imageURL, i, false));
                }
            }
        }

        for (int i = 0;i < timeLineMatLength; i++)
        {
            string timeLinetoken = GetTimeLineToken(i);
            for (int k = 0; k < apiImageLenght; k++)
            {
                if (timeLinetoken == apiManager.museumDataList.data[0].slots.image[k].token)
                {
                    string imageURL = apiManager.museumDataList.data[0].slots.image[k].s3_value;
                    StartCoroutine(GetTexture(imageURL, i, true));
                }
            }
        }
        //GetTimeLineURL();
    }

    IEnumerator GetTexture(string _imageURL, int matIndex, bool isTimeline)
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

            if (!isTimeline)
            {
                materialList[matIndex].SetTexture("_MainTex", loadedTexture);
            }
            else
            {
                timeLineMaterialList[matIndex].SetTexture("_MainTex", loadedTexture);
            }
        }
    }

    /*private void GetTimeLineURL ()
    {
        timeLineURL = apiManager.museumDataList.data[0].slots.image[27].s3_value;
        StartCoroutine(GetTexture(timeLineURL, 27));
    }
    */
    private string GetToken (int _index)
    {
        return TokenInformation.instance.imageToken[_index];
    }

    private string GetTimeLineToken(int _index)
    {
        return TokenInformation.instance.timeLideImageToken[_index];
    }
}
