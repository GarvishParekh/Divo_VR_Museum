using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class InformationLoader : MonoBehaviour
{
    APIManager_JU apiManager;

    [SerializeField] private List<TMP_Text> T_name;
    [SerializeField] private List<TMP_Text> T_information;

    private void Start()
        => apiManager = APIManager_JU.instance;

    private void OnEnable()
    {
        APIManager_JU.APIRecived += OnApiRecived;
    }

    private void OnDisable()
    {
        APIManager_JU.APIRecived -= OnApiRecived;
    }
    private void OnApiRecived ()
    {
        for (int i = 0; i < T_information.Count; i++)
        {
            T_name[i].text = apiManager.museumDataList.data[0].slots.trophy[i].name;
            T_information[i].text = apiManager.museumDataList.data[0].slots.trophy[i].description;
        }
    }
}
