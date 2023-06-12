using UnityEngine;
using UnityEngine.UI;

public class AudioPlayPause : MonoBehaviour
{
    UiManager uiManager;
    [SerializeField] private AudioSource audioSource;

    [Header("UI")]
    [SerializeField] private Image I_playButton;
    [SerializeField] private Image I_MuteButton;
    [SerializeField] private Slider seekBar;

    public bool isPlaying = true;
    public bool isMuted = false;

    private void OnEnable()
    {
        UiManager.AudioPlaying += OnAudioPlaying;
    }

    private void OnDisable()
    {
        UiManager.AudioPlaying -= OnAudioPlaying;
    }

    private void OnAudioPlaying()
    {
        ForceUnMute();
    }

    float normalVolume = 0.2f;
    private void Start()
    {
        uiManager = UiManager.instance;
        I_playButton = transform.GetChild(0).GetComponent<Image>();
        I_MuteButton = transform.GetChild(1).GetComponent<Image>();
        seekBar = transform.GetChild(2).GetComponent<Slider>();

        I_playButton.sprite = uiManager.pauseButtonImage;
        I_MuteButton.sprite = uiManager.unMuteButtonImage;

        normalVolume = audioSource.volume;
    }

    private void Update()
    {
        if (audioSource.clip != null)
        {
            seekBar.value = audioSource.time / audioSource.clip.length;
            if (!audioSource.isPlaying)
            {
                isPlaying = false;
                I_playButton.sprite = uiManager.playButtonImage;
                audioSource.Pause();
            }
        }
    }

    public void _PlayPause()
    {
        if (isPlaying)
        {
            isPlaying = false;
            I_playButton.sprite = uiManager.playButtonImage;
            audioSource.Pause(); 
        }
        else
        {
            isPlaying = true;
            I_playButton.sprite = uiManager.pauseButtonImage;
            audioSource.Play();
        }
    }

    public void _MuteUnMute()
    {
        if (isMuted)
        {
            isMuted = false;
            audioSource.volume = normalVolume;
            I_MuteButton.sprite = uiManager.unMuteButtonImage;
        }
        else
        {
            isMuted = true;
            audioSource.volume = 0;
            I_MuteButton.sprite = uiManager.muteButtonImage;
        }
    }

    public void ForceUnMute ()
    {
        isMuted = false;
        audioSource.volume = normalVolume;
        I_MuteButton.sprite = uiManager.unMuteButtonImage;

        isPlaying = true;
        I_playButton.sprite = uiManager.pauseButtonImage;
    }
}
