using UnityEngine;

public class PillarUiManager : MonoBehaviour
{
    UiManager uiManager;
    [SerializeField] string playerTag;
    [SerializeField] Animator pillarUiAnimation;
    [SerializeField] Animator pannelUiAnimation;

    [Header("Buttton animation tag")]
    [SerializeField] string growTag;
    [SerializeField] string shrinkTag;

    [Space]
    [Header("Pannel animation tag")]
    [SerializeField] string startTag;

    [Header("Pannels")]
    [SerializeField] GameObject informationPanel;
    [SerializeField] GameObject videoPanel;

    GameObject mainpanel;

    private void Start()
    {
        informationPanel = pannelUiAnimation.GetComponent<Transform>().GetChild(0).gameObject;
        videoPanel= pannelUiAnimation.GetComponent<Transform>().GetChild(1).gameObject;
        
        informationPanel.SetActive(false);
        videoPanel.SetActive(false);

        // get all components
        uiManager = UiManager.instance;
        mainpanel = pannelUiAnimation.GetComponent<Transform>().gameObject;
    }

    private void OnTriggerEnter(Collider info)
    {
        if (info.CompareTag(playerTag))
            pillarUiAnimation.SetTrigger(growTag);
    }

    private void OnTriggerExit(Collider info)
    {
        if (info.CompareTag(playerTag))
            pillarUiAnimation.SetTrigger(shrinkTag);
    }

    public void B_Open(GameObject desireObject)
    {
        // close all the local panels
        CloseAllPanel();

        // close if any panel is open
        uiManager.CloseAllpanels();
        mainpanel.SetActive(true); 
        
        // triggers an starting animation
        pannelUiAnimation.SetTrigger(startTag);
        desireObject.SetActive(true);
    }

    public void B_Close (GameObject desireObject)
        => desireObject.SetActive(false);

    private void CloseAllPanel ()
    {
        informationPanel.SetActive(false);
        videoPanel.SetActive(false);
    }
}
