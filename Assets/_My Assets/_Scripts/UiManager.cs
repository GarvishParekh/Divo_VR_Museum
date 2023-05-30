using TMPro;
using UnityEngine;
using TriLibCore.Samples;

public class UiManager : MonoBehaviour
{
    
    public static UiManager instance;
    [SerializeField] GameObject[] allPanels;
    [SerializeField] GameObject[] allVideoPanels;
    [SerializeField] GameObject[] allDescriptionPanels;

    [Space]
    [SerializeField] GameObject loadingPanel;

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

    // close all panel at once
    public void CloseAllpanels()
    {
        for (int i = 0; i < allPanels.Length; i++)
        {
            allPanels[i].SetActive(false);  
            allVideoPanels[i].SetActive(false);  
            allDescriptionPanels[i].SetActive(false);  
        }
    }

    public void _OpenVideoPanel (int _panelIndex)
    {
        CloseAllpanels();
        int panelIndex = _panelIndex - 1;

        allPanels[panelIndex].SetActive(true);
        allVideoPanels[panelIndex].SetActive(true);
    }

    public void _OpenDescriptionPanel(int _panelIndex)
    {
        CloseAllpanels();
        int panelIndex = _panelIndex - 1;

        allPanels[panelIndex].SetActive(true);
        allDescriptionPanels[panelIndex].SetActive(true);
    }

    // close specific panel
    public void ClosePanel (int _panelIndex)
    {
        int panelIndex = _panelIndex - 1;

        allPanels[panelIndex].SetActive(false);
        allVideoPanels[panelIndex].SetActive(false);
        allDescriptionPanels[panelIndex].SetActive(false);
    }

    public void B_Close(GameObject desireObject)
        => desireObject.SetActive(false);

    public void CloseLoadingPanel()
        => loadingPanel.SetActive(false);
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
