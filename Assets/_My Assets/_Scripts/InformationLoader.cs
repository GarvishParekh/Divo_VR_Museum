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
    private void OnApiRecived()
    {
        int listLenght = T_information.Count;
        for (int i = 0; i < listLenght; i++)
        {
            string currentToken = GetToken(i);
            for (int j = 0; j < listLenght; j++)
            {
                if (currentToken == apiManager.museumDataList.data[0].slots.trophy[j].token)
                {
                    T_name[i].text = apiManager.museumDataList.data[0].slots.trophy[j].name;
                    T_information[i].text = apiManager.museumDataList.data[0].slots.trophy[j].description;
                }
            }
        }
    }

    private string GetToken(int _index)
    {
        return TokenInformation.instance.trophyToken[_index];
    }
}
