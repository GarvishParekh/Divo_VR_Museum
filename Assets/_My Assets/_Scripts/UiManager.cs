using UnityEngine;

public class UiManager : MonoBehaviour
{
    
    public static UiManager instance;
    [SerializeField] GameObject[] allPanels;

    private void Awake() => instance = this;

    public void CloseAllpanels()
    {
        for (int i = 0; i < allPanels.Length; i++)
        {
            allPanels[i].SetActive(false);  
        }
    }
}
