using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [SerializeField] string playerTag;

    [Header ("Pannel components")]
    [SerializeField] Animator quizPanelAnimation;
    [SerializeField] GameObject quizCanvas;

    [Space]
    [SerializeField] string startTag;
    [SerializeField] string endTag;

    [Space]
    [Header ("Quiz UI")]
    [SerializeField] private TMP_Text T_quizQuestion;
                             
    [Space]                  
    [SerializeField] private TMP_Text T_quizOption1;
    [SerializeField] private TMP_Text T_quizOption2;
    [SerializeField] private TMP_Text T_quizOption3;
    [SerializeField] private TMP_Text T_quizOption4;

    [SerializeField] private int questionIndex = 0;
    [Space]
    [Header("Quiz API information")]
    [SerializeField] private string token;
    [SerializeField] private string contentType;
    [Space]
    [SerializeField] private string jsonString;
    private string URL;

    [Space]
    [SerializeField] private QuestionData questionData;

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

    private void Start()
    {
        URL = URLs.testingQuizURL;
        StartCoroutine(GetQuizRequest(URL));
    }

    IEnumerator GetQuizRequest(string url)
    {
        UnityWebRequest quizRequest = UnityWebRequest.Get(url);

        quizRequest.SetRequestHeader("Authorization", token);
        quizRequest.SetRequestHeader("Content-Type", contentType);

        yield return quizRequest.SendWebRequest();
        if (quizRequest.error != null)
        {
            Debug.Log($"Error: {quizRequest.error}");
        }
        else
        {
            jsonString = quizRequest.downloadHandler.text;
            questionData = JsonUtility.FromJson<QuestionData>(jsonString);

            SetQuizUI(questionIndex);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.P))
        {
            _NextQuestion();
        }
    }


    private void SetQuizUI(int _index)
    {
        T_quizQuestion.text = questionData.data[_index].question;

        T_quizOption1.text = questionData.data[_index].answers[0].answer;
        T_quizOption2.text = questionData.data[_index].answers[1].answer;
        T_quizOption3.text = questionData.data[_index].answers[2].answer;
        T_quizOption4.text = questionData.data[_index].answers[3].answer;
    }

    public void _NextQuestion()
    {
        if (questionIndex < questionData.data.Count)
        {
            questionIndex++;
            SetQuizUI(questionIndex);
        }
        else
        {
            quizCanvas.SetActive(false);
        }
    }
}

[System.Serializable]
public class QuestionData
{
    public int current_page;
    public List<Data> data = new List<Data>();
}

[System.Serializable]
public class Data
{
    public int id;
    public string question;

    [Space]
    public List<Options> answers = new List<Options>();
}

[System.Serializable]
public class Options
{
    public string answer;
}

