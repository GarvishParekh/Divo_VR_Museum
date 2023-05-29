using UnityEngine;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;

public class VideoPlayer : MonoBehaviour
{
    APIManager_JU apiManager;

    [Header ("Media player information")]
    [SerializeField] MediaPlayer mediaPlayer;
    [SerializeField] Transform mediaPlayerUI;

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

    private void OnApiRecived ()
    {
        int listLenght = apiManager.museumDataList.data[0].slots.trophy.Count;
        for (int i = 0; i < listLenght; i++)
        {
            videoURL.Add(apiManager.museumDataList.data[0].slots.trophy[i].video);
        }
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
