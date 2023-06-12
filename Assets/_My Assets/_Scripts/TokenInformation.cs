using UnityEngine;
using System.Collections.Generic;

public class TokenInformation : MonoBehaviour
{
    public static TokenInformation instance;
    APIManager_JU apiManager;

    [Header ("Tokens")]
    public List<string> trophyToken = new List<string>();
    public List<string> imageToken = new List<string>();

    [Space]
    [SerializeField] private bool storeAPI = false;

    private void Awake()
        => instance = this;

    private void Start()
        => apiManager = APIManager_JU.instance;

    private void OnEnable()
    {
        if (storeAPI)
        {
            APIManager_JU.APIRecived += OnRecivingAPI;
        }
    }

    private void OnDisable()
    {
        if (storeAPI)
        {
            APIManager_JU.APIRecived -= OnRecivingAPI;
        }
    }

    private void OnRecivingAPI()
    {
        int listLenght = apiManager.museumDataList.data[0].slots.image.Count;
        for (int i = 0; i < listLenght; i++)
        {
            imageToken.Add(apiManager.museumDataList.data[0].slots.image[i].token);
        }
    }
}
