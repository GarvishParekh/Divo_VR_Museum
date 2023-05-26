using TMPro;
using UnityEngine;
using TriLibCore.Samples;

public class UiManager : MonoBehaviour
{
    
    public static UiManager instance;
    [SerializeField] GameObject[] allPanels;

    [Space]
    [SerializeField] private GameObject loadingPlane;
    [SerializeField] private OVRPlayerController playerController;
    [SerializeField] private int modelCount = 0;

    public Material mat1;
    public Material mat2;

    public UITrophyData[] uiTrophyData;

    private void Start()
    {
        mat1.renderQueue = 2999;
        mat2.renderQueue = 2999;
    }


    private void OnEnable()
    {
        LoadModelFromURLSample.modelLoaded += OnModelLoaded;
    }

    private void OnDisable()
    {
        LoadModelFromURLSample.modelLoaded -= OnModelLoaded;
    }

    private void OnModelLoaded ()
    {
        modelCount++;
        if (modelCount >= 25)
        {
            loadingPlane.SetActive(false);
            playerController.enabled = true;
        }
    }

    private void Awake() => instance = this;

    public void CloseAllpanels()
    {
        for (int i = 0; i < allPanels.Length; i++)
        {
            allPanels[i].SetActive(false);  
        }
    }

    public void B_Close(GameObject desireObject)
        => desireObject.SetActive(false);

}

[System.Serializable]
public class UITrophyData
{
    public TMP_Text name;
    public TMP_Text description;

    [Space]
    [Header ("URL")]
    public string modelURL;
    public string audioURL;
    public string videoURL;
}
