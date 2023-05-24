using UnityEngine;

public class QuizManager : MonoBehaviour
{
    [SerializeField] string playerTag;

    [Header ("Pannel components")]
    [SerializeField] Animator quizPanelAnimation;

    [Space]
    [SerializeField] string startTag;
    [SerializeField] string endTag;

    private void OnTriggerEnter(Collider info)
    {
        if (info.CompareTag (playerTag))
            quizPanelAnimation.SetTrigger(startTag);
    }

    private void OnTriggerExit(Collider info)
    {
        if (info.CompareTag (playerTag))        
            quizPanelAnimation.SetTrigger(endTag);
    }
}
