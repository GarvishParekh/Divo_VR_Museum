using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TriLibCore.Samples;
using System.Collections.Generic;

public class LoadModels : MonoBehaviour
{
    APIManager_JU apiManager;
    TokenInformation tokenInformation;

    [Space]
    [SerializeField] private List<string> modelURLs;
    [SerializeField] private List<LoadModelFromURLSample> modelLoadingScripts;
    [SerializeField] int modelLoaded = 0;

    [Space]
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Image loadingBarImage;

    [Space]
    [SerializeField] private GameObject[] spotLights;

    float percentage = 0;
    int totalModel = 0;

    private void Start()
    {
        apiManager = APIManager_JU.instance;
        tokenInformation = TokenInformation.instance;

        if (spotLights != null)
        {
            for (int i = 0; i < spotLights.Length; i++)
            {
                spotLights[i].SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        APIManager_JU.APIRecived += OnApiRecived;                       // on reciving api
        LoadModelFromURLSample.modelLoaded += OnModelLoaded;            // after model is loaded
    }

    private void OnDisable()
    {
        APIManager_JU.APIRecived += OnApiRecived;
        LoadModelFromURLSample.modelLoaded -= OnModelLoaded;
    }

    private void Update()
    {
        //loadingBarImage.fillAmount = percentage / 100;
        loadingBarImage.fillAmount = Mathf.MoveTowards(loadingBarImage.fillAmount, percentage / 100, 0.05f * Time.deltaTime);
    }

    private void OnApiRecived ()
    {
        int listLenght = apiManager.museumDataList.data[0].slots.trophy.Count;
        totalModel = listLenght;

        for (int i = 0; i < listLenght; i++)
        {
            string currentToken = GetToken(i);

            for (int j = 0; j < listLenght; j++)
            {
                if (currentToken == apiManager.museumDataList.data[0].slots.trophy[j].token)
                {
                    modelURLs.Add(apiManager.museumDataList.data[0].slots.trophy[j].model);
                    modelLoadingScripts[i].ModelURL = modelURLs[j];
                    Debug.Log("TOKEN NUMBER: " + j);
                }
                else
                {
                    Debug.Log("Token Incorrect");
                }
            }
        }

        // start loading model on reciving the api's
        modelLoadingScripts[modelLoaded].StartLoading();
        Debug.Log("START LOADING MODEL");
    }

    private void OnModelLoaded ()
    {
        modelLoaded++;
        percentage = (modelLoaded * 100) / 25;
        Debug.Log(percentage);
        loadingText.text = $"Loading..{percentage}%";
        if (modelLoaded < totalModel)
            modelLoadingScripts[modelLoaded].StartLoading();
        else
        {
            Debug.Log("ALL MODELS LOADED!!!");
            UiManager.instance.CloseLoadingPanel();
            if (spotLights != null)
            {
                for (int i = 0; i < spotLights.Length; i++)
                {
                    spotLights[i].SetActive(true);
                }
            }
            
            return;
        }
    }

    private string GetToken (int _index)
    {
        return tokenInformation.trophyToken[_index];
    }
}
