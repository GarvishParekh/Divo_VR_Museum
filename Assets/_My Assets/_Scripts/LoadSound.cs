using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LoadSound : MonoBehaviour
{
    APIManager_JU apiManager;

    [SerializeField] private List<string> audioUrls = new List<string>(); 
    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();

    [Space]
    [Header("Background music")]
    [SerializeField] private string bgMusicURL;
    [SerializeField] private AudioSource bgMusicAudioSource;

    [Space]
    [Range(0, 0.5f)]
    [SerializeField] private float normalVolume;
    [Range(0, 0.5f)]
    [SerializeField] private float lowVolume;


    private void Start()
    {
        apiManager = APIManager_JU.instance;
    }

    private void OnEnable()
    {
        APIManager_JU.APIRecived += OnRecivingAPi;
        UiManager.VideoPlaying += OnVideoPlaying;
        UiManager.VideoExit += OnVideoExit;
    }

    private void OnDisable()
    {
        APIManager_JU.APIRecived -= OnRecivingAPi;
        UiManager.VideoPlaying -= OnVideoPlaying;
        UiManager.VideoExit -= OnVideoExit;
    }

    private void OnVideoPlaying ()
    {
        bgMusicAudioSource.volume = lowVolume;
    }

    private void OnVideoExit()
    {
        Debug.Log("CHANGE THE VOLUME!!!!!!!!!!!!!");
        bgMusicAudioSource.volume = normalVolume;
    }

    private void OnRecivingAPi()
    {
        int listLength = apiManager.museumDataList.data[0].slots.trophy.Count;
        Debug.Log(listLength);
        for (int i = 0; i < 25; i++)
        {
            audioUrls.Add(apiManager.museumDataList.data[0].slots.trophy[i].audio);
            StartCoroutine(DownloadDesAudios(audioUrls[i], i));
        }

        bgMusicURL = apiManager.museumDataList.data[0].slots.audio[0].s3_value;

        SetAndPlayBgMusicFuntion();
    }

    private IEnumerator DownloadDesAudios(string _audioUrl, int _index)
    {
        UnityWebRequest audioRequest = UnityWebRequestMultimedia.GetAudioClip(_audioUrl, AudioType.WAV);
        yield return audioRequest.SendWebRequest();

        if (audioRequest.error != null)
        {
            Debug.LogError(audioRequest.error);
        }
        else
        {
            var clip = ((DownloadHandlerAudioClip)audioRequest.downloadHandler).audioClip;
            audioSources[_index].clip = clip;
        }
    }
    
    private IEnumerator SetAndPlayBgMusic(string _audioUrl)
    {
        UnityWebRequest audioRequest = UnityWebRequestMultimedia.GetAudioClip(_audioUrl, AudioType.MPEG);
        yield return audioRequest.SendWebRequest();

        if (audioRequest.error != null)
        {
            Debug.LogError(audioRequest.error);
        }
        else
        {
            var clip = ((DownloadHandlerAudioClip)audioRequest.downloadHandler).audioClip;
            bgMusicAudioSource.clip = clip;

            bgMusicAudioSource.Play();
        }
    }

    private void SetAndPlayBgMusicFuntion()
        => StartCoroutine(SetAndPlayBgMusic(bgMusicURL));


    public void _PlayAudio (int _audioIndex)
    {
        int index = _audioIndex - 1;

        if (audioSources[index].clip != null)
        {
            audioSources[index].Play();
        }
        else
        {
            return;
        }
    }

    public void _OnCloseAudio()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].Stop();
        }
    }
}
