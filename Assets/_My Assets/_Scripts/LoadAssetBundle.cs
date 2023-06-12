using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LoadAssetBundle : MonoBehaviour
{
    APIManager_JU apiManager;
    [SerializeField] private string lightMapURL;
    [SerializeField] private string bundleURL;
    [SerializeField] private string assetName;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        //apiManager = APIManager_JU.instance;
        OnRecivingAPI();
    }

    //private void OnEnable()
    //{
    //    APIManager_JU.APIRecived += OnRecivingAPI;
    //}
    //
    //private void OnDisable()
    //{
    //    APIManager_JU.APIRecived -= OnRecivingAPI;
    //}

    void OnRecivingAPI()
    {
        //bundleURL = apiManager.museumDataList.data[0].file;
        StartCoroutine(LoadAssetBundleIntoScene(bundleURL, assetName));
    }

    IEnumerator LoadAssetBundleIntoScene(string bundleUrl, string assetName)
    {
        UnityWebRequest bundleRequest = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
        yield return bundleRequest.SendWebRequest();

        if (bundleRequest.error != null)
        {
            Debug.Log(bundleRequest.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(bundleRequest);
            if (bundle != null)
            {
                Instantiate(bundle.LoadAsset(assetName), spawnPoint.position, Quaternion.identity);
                bundle.Unload(false);
            }
        }
    }

    IEnumerator LoadB(string bundleUrl, string assetName)
    {
        var bundleRequest = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
        yield return bundleRequest.SendWebRequest();

        // Get an asset from the bundle and instantiate it.
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(bundleRequest);
        var loadAsset = bundle.LoadAssetAsync<GameObject>(assetName);
        yield return loadAsset;

        Instantiate(loadAsset.asset);
    }
}
