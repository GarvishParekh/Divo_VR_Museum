using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEditor;

public class LoadAssetBundle : MonoBehaviour
{
    [SerializeField] private string bundleURL;

    [Space]
    [SerializeField] private string modelName;
    [SerializeField] private string lightMapName;

    [Space]
    [SerializeField] private GameObject museumModel;
    [SerializeField] private Texture2D lightMap;

    [Space]

    [Space]
    [SerializeField] private Transform spawnPoint;
    public LightmapData lightmapData;

    private void Start()
    {
        StartCoroutine(StartLoadingBundle(bundleURL, modelName, lightMapName));
    }

    IEnumerator StartLoadingBundle (string _bundleURL, string _modelName, string _lightMapName)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(_bundleURL);
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.LogError(request.error);
        }
        else
        {
            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            if (assetBundle != null)
            {
                museumModel = (GameObject)Instantiate(assetBundle.LoadAsset(_modelName), spawnPoint.position, Quaternion.identity);

                if (museumModel != null)
                {
                    museumModel.SetActive(true);
                    Debug.Log("Light map loaded");

                    lightMap = (Texture2D)assetBundle.LoadAsset(_lightMapName);
                    lightmapData.lightmapDir = lightMap;
                }
                assetBundle.Unload(false);
            }
            else if (assetBundle == null)
            {
                Debug.LogError("asset bundle empty");
            }
        }
    }
}
