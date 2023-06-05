using UnityEngine;
using TriLibCore.Samples;
using System.Collections.Generic;

public class LoadModels : MonoBehaviour
{
    APIManager_JU apiManager;

    [Space]
    [SerializeField] private List<string> modelURLs;
    [SerializeField] private List<LoadModelFromURLSample> modelLoadingScripts;
    [SerializeField] int modelLoaded = 0;

    int totalModel = 0;

    private void Start()
    {
        apiManager = APIManager_JU.instance;
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

    private void OnApiRecived ()
    {
        int listLenght = apiManager.museumDataList.data[0].slots.trophy.Count;
        totalModel = listLenght;

        for (int i = 0; i < listLenght; i++)
        {
            modelURLs.Add(apiManager.museumDataList.data[0].slots.trophy[i].model);
            modelLoadingScripts[i].ModelURL = modelURLs[i];
        }

        // start loading model on reciving the api's
        modelLoadingScripts[modelLoaded].StartLoading();
    }

    private void OnModelLoaded ()
    {
        modelLoaded++;
        if (modelLoaded < totalModel)
            modelLoadingScripts[modelLoaded].StartLoading();
        else
        {
            Debug.Log("ALL MODELS LOADED!!!");
            UiManager.instance.CloseLoadingPanel();
            return;
        }
    }
}
