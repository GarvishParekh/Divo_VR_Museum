using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [SerializeField] string playerTag;

    [Space]
    [Header("Quiz API information")]
    [SerializeField] private string token;
    [SerializeField] private string contentType;
    [Space]
    [SerializeField] private string questionJsonString;

    [Header("ID for form")]
    [SerializeField] private string postURL;
    [SerializeField] List<int> questionID = new List<int>();
    [SerializeField] List<int> answerID = new List<int>();

    [Space]
    [SerializeField] string answerJson;
    [SerializeField] QuizData quizData;

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
    
    [Space]
    [SerializeField] private int correctAnswer = 0;
    [SerializeField] private TMP_Text T_score;
    [SerializeField] private GameObject scorePanel;

    [Space]
    [SerializeField] private Toggle[] optionToggle;
    [SerializeField] private TMP_Text[] optionText;

    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;

    [SerializeField] private int questionIndex = 0;
    
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
        ResetOptionMarks();
        URL = URLs.testingQuizURL;
        StartCoroutine(GetQuizRequest(URL));
    }

    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.P))
        {
            OptionSelected(1);
            _NextQuestion();
        }
        else if (Input.GetKeyDown (KeyCode.U))
        {
            Debug.LogError("API Sent");
            _SendAnswers();
        }
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
            questionJsonString = quizRequest.downloadHandler.text;
            questionData = JsonUtility.FromJson<QuestionData>(questionJsonString);

            SetQuizUI(questionIndex);
        }
    }

    IEnumerator UploadAnswers (string url)
    {
        WWWForm answerForm = new WWWForm();

        answerForm.AddField($"quiz_data[0][question_id]", 46);
        answerForm.AddField($"quiz_data[0][answer_id]", 48);
        
        UnityWebRequest request = UnityWebRequest.Post(url, answerForm);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Authorization", token);
        request.SetRequestHeader("Content-Type", contentType);
        
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.LogError(request.error);
        }
        else 
        {
            if (answerForm.data.Length == 0)
            {
                Debug.LogError("FORM IS NULL");
            }

            answerJson = request.downloadHandler.text;

            //quizData = JsonUtility.FromJson<QuizData>(answerJson);

            T_score.text = $"Your score is: <color=#F3A101>{correctAnswer}";
        }
    }

    public void OptionSelected (int _optionIndex)
    {
        optionToggle[_optionIndex].graphic.enabled = true;
        if (questionData.data[questionIndex].answers[_optionIndex].is_correct == "2")
        {
            correctAnswer++;
            T_score.text = $"Your score is: <color=#F3A101>{correctAnswer}";
            optionText[_optionIndex].color = greenColor;
            optionToggle[_optionIndex].graphic.color = greenColor;
        }
        else
        {
            optionText[_optionIndex].color = redColor;
            optionToggle[_optionIndex].graphic.color = redColor;
        }
        questionID.Add(questionData.data[questionIndex].id);
        answerID.Add(questionData.data[questionIndex].answers[_optionIndex].id);
    }

    private void SetQuizUI(int _index)
    {
        T_quizQuestion.text = questionData.data[_index].question;

        T_quizOption1.text = questionData.data[_index].answers[0].answer;
        T_quizOption2.text = questionData.data[_index].answers[1].answer;
        T_quizOption3.text = questionData.data[_index].answers[2].answer;
        T_quizOption4.text = questionData.data[_index].answers[3].answer;
    }

    IEnumerator NextQuestion ()
    {
        for (int i = 0; i < optionToggle.Length; i++)
        {
            optionToggle[i].interactable = false;
        }

        yield return new WaitForSeconds(0.4f);

        if (questionIndex < 24)
        {
            questionIndex++;
            SetQuizUI(questionIndex);
            ResetOptionMarks();
        }
        else
        {
            _SkipButton();
        }

        for (int i = 0; i < optionToggle.Length; i++)
        {
            optionToggle[i].interactable = true;
            optionText[i].color = Color.white;
        }
    }
    public void _NextQuestion()
    {
        StartCoroutine(nameof(NextQuestion));
    }

    private void ResetOptionMarks()
    {
        for (int i = 0; i < optionToggle.Length; i++)
        {
            optionToggle[i].graphic.enabled = false;
        }
    }

    public void _SkipButton ()
    {
        quizCanvas.SetActive(false);
        scorePanel.SetActive(true);

        Invoke(nameof(CloseScorePanel), 10);
    }

    public void _SendAnswers()
    {
        StartCoroutine(UploadAnswers(postURL));
        scorePanel.SetActive(true);
        Invoke(nameof(CloseScorePanel), 10);
    }

    private void CloseScorePanel()
    { 
        scorePanel.SetActive(false);
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
    public int id;
    public string answer;
    public string is_correct;
}

[System.Serializable]
public class QuizData
{
    public string message;
    public MainData data;
}

[System.Serializable]
public class MainData
{
    public int questions;
    public int correct;
}


public class AnswerData
{
}


