using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;

public class VideoPlayer : MonoBehaviour
{
    APIManager_JU apiManager;

    [Header ("Media player information")]
    [SerializeField] private Image buffringImage;
    [SerializeField] MediaPlayer mediaPlayer;
    [SerializeField] Transform mediaPlayerUI;
    [SerializeField] Transform firstVideoPlayer;

    [Space]
    [Header("Data")]
    [SerializeField] int buttonIndex = 0;       // 0 = default
    [SerializeField] List<string> videoURL = new List<string>();
    [SerializeField] Transform[] pillarPosition;

    [Space]
    [Header("Testing")]
    [SerializeField] private int videoIndex = 0;
    [SerializeField] private Vector3 offsetPosition = new Vector3(0, 0 - 0.5f);

    private void Start()
    {
        apiManager = APIManager_JU.instance;
        firstVideoPlayer.gameObject.SetActive (false);
    }

    #region Actions and function

    private void OnEnable()
    {
        APIManager_JU.APIRecived += OnApiRecived;
    }

    private void OnDisable()
    {
        APIManager_JU.APIRecived -= OnApiRecived;
    }

    private void Update()
    {
        if (mediaPlayer.Control.IsPlaying() == true)
        {
            buffringImage.enabled = false;
        }
        else
        {
            buffringImage.enabled = true;
        }
    }

    private void OnApiRecived()
    {
        int listLenght = apiManager.museumDataList.data[0].slots.trophy.Count;
        for (int i = 0; i < listLenght; i++)
        {
            string currentToken = GetToken(i);
            for (int j = 0; j < listLenght; j++)
            {
                if (currentToken == apiManager.museumDataList.data[0].slots.trophy[j].token)
                {
                    videoURL.Add(apiManager.museumDataList.data[0].slots.trophy[j].video);
                }
            }
        }
    }

    private string GetToken(int _index)
    {
        return TokenInformation.instance.trophyToken[_index];
    }

    #endregion

    public void OpenMediaPlayer (int _videoIndexInput)
    {
        int _videoIndex = _videoIndexInput - 1;
        if (_videoIndex > videoURL.Count)
        {
            Debug.Log("Index is out of range");
        }

        // set the video ui position
        mediaPlayerUI.SetParent(pillarPosition[_videoIndex]);
        mediaPlayerUI.localPosition = offsetPosition;

        string videoPath = videoURL[_videoIndex];
        mediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, videoPath, autoPlay: true);
    }

    public void VideoExitButton ()
    {
        mediaPlayer.CloseMedia();
    }
}
