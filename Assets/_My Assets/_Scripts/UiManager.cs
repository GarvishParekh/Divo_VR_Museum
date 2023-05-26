using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    
    public static UiManager instance;
    [SerializeField] GameObject[] allPanels;

    public UITrophyData[] uiTrophyData;

    private void Awake() => instance = this;

    public void CloseAllpanels()
    {
        for (int i = 0; i < allPanels.Length; i++)
        {
            allPanels[i].SetActive(false);  
        }
    }
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
